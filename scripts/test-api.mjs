import assert from 'node:assert/strict';

const base = process.env.ERP_API_URL || 'http://localhost:5028';
const tag = 'ApiDemo' + Date.now();
const createdDepartments = [];
const createdEmployees = [];
let passed = 0;

async function request(method, path, body, status = 200) {
  const response = await fetch(base + path, {
    method, headers: { 'Content-Type': 'application/json', Origin: 'http://localhost:4200' },
    body: body === undefined ? undefined : JSON.stringify(body)
  });
  const text = await response.text();
  assert.equal(response.status, status, method + ' ' + path + ': ' + text);
  let json;
  if (text) json = JSON.parse(text);
  if (status >= 400) assert.ok(!/StackTrace|\\.cs:|SqlException|C:\\\\Users/i.test(text), 'Error must not expose internals');
  passed++;
  return { response, json, data: json?.data };
}

try {
  const health = await fetch(base + '/health');
  assert.equal(health.status, 200);
  passed++;
  const departments = await request('GET', '/api/departments');
  assert.ok(departments.data.length >= 8);
  const employees = await request('GET', '/api/employees');
  assert.ok(employees.data.length >= 10);
  assert.ok(employees.data.every(e => e.departmentName && e.fullName));
  assert.equal(departments.response.headers.get('access-control-allow-origin'), 'http://localhost:4200');
  await request('POST', '/api/departments', { departmentName: '   ' }, 400);
  await request('POST', '/api/departments', { departmentName: 'x'.repeat(201) }, 400);

  const first = await request('POST', '/api/departments', {
    departmentName: '  ' + tag + '  ', departmentAddress: 'Demo office'
  }, 201);
  const departmentId = first.data.departmentId;
  createdDepartments.push(departmentId);
  assert.equal(first.data.departmentName, tag);
  assert.ok(first.response.headers.get('location').endsWith('/api/departments/' + departmentId));
  assert.equal((await request('GET', '/api/departments/' + departmentId)).data.employeeCount, 0);

  const second = await request('POST', '/api/departments', { departmentName: tag + 'Second' }, 201);
  createdDepartments.push(second.data.departmentId);
  const valid = { departmentId, firstName: tag, lastName: 'Tester', gender: 'Other',
    dateOfBirth: '1995-05-10', dateJoined: '2024-01-15', employeeAddress: 'Fictional address' };
  for (const invalid of [
    { firstName: '   ' }, { lastName: '' }, { firstName: 'x'.repeat(101) }, { departmentId: 0 },
    { departmentId: 2147483647 }, { gender: 'Invalid' }, { dateOfBirth: '2999-01-01' },
    { dateJoined: '2999-01-01' }, { dateJoined: '1990-01-01' }, { dateJoined: valid.dateOfBirth },
    { dateOfBirth: '0001-01-01' }, { dateOfBirth: 'not-a-date' }, { employeeAddress: 'x'.repeat(501) }
  ]) await request('POST', '/api/employees', { ...valid, ...invalid }, 400);

  const created = await request('POST', '/api/employees', valid, 201);
  const employeeId = created.data.employeeId;
  createdEmployees.push(employeeId);
  assert.equal(created.data.departmentName, tag);
  assert.equal(created.data.photo, null);
  assert.ok(created.response.headers.get('location').endsWith('/api/employees/' + employeeId));
  assert.equal((await request('GET', '/api/employees/' + employeeId)).data.fullName, tag + ' Tester');
  assert.equal((await request('GET', '/api/departments/' + departmentId)).data.employeeCount, 1);
  const blocked = await request('DELETE', '/api/departments/' + departmentId, undefined, 400);
  assert.match(blocked.json.message, /employees.*assigned/i);

  await request('PUT', '/api/departments/' + departmentId, { departmentName: tag + 'Renamed' });
  assert.equal((await request('GET', '/api/employees/' + employeeId)).data.departmentName, tag + 'Renamed');
  assert.equal((await request('GET', '/api/employees?search=' + tag)).data.length, 1);
  assert.equal((await request('GET', '/api/employees?search=' + tag + 'Renamed')).data.length, 1);
  assert.equal((await request('GET', '/api/employees?departmentId=' + departmentId)).data.length, 1);
  assert.equal((await request('GET', '/api/employees?search=NoMatch' + tag)).data.length, 0);
  await request('PUT', '/api/employees/' + employeeId, { ...valid, firstName: '  Updated  ', departmentId: second.data.departmentId });
  const updated = (await request('GET', '/api/employees/' + employeeId)).data;
  assert.equal(updated.firstName, 'Updated');
  assert.equal(updated.departmentName, tag + 'Second');
  assert.equal((await request('GET', '/api/departments/' + departmentId)).data.employeeCount, 0);

  for (const path of ['/api/departments', '/api/employees']) {
    await request('GET', path + '/2147483647', undefined, 404);
    await request('DELETE', path + '/2147483647', undefined, 404);
    await request('PUT', path + '/2147483647', path.endsWith('departments') ? { departmentName: 'Test' } : valid, 404);
  }

  await request('DELETE', '/api/employees/' + employeeId, undefined, 204);
  createdEmployees.splice(createdEmployees.indexOf(employeeId), 1);
  await request('GET', '/api/employees/' + employeeId, undefined, 404);
  for (const id of [...createdDepartments]) {
    await request('DELETE', '/api/departments/' + id, undefined, 204);
    createdDepartments.splice(createdDepartments.indexOf(id), 1);
    await request('GET', '/api/departments/' + id, undefined, 404);
  }
  if (process.env.ERP_FAILURE_API_URL) {
    // Optional separate host started with an intentionally invalid connection string.
    const failure = await fetch(process.env.ERP_FAILURE_API_URL + '/api/employees');
    assert.equal(failure.status, 500);
    const body = await failure.json();
    assert.equal(body.succeeded, false);
    assert.equal(body.message, 'An unexpected error occurred. Please try again.');
    passed++;
  }
  console.log('PASS: ' + passed + ' HTTP checks covering CRUD, search, CORS, validation, FK protection and safe errors.');
} finally {
  // Only remove records created by this run; never remove seed or pre-existing records.
  for (const id of createdEmployees) await fetch(base + '/api/employees/' + id, { method: 'DELETE' });
  for (const id of createdDepartments) await fetch(base + '/api/departments/' + id, { method: 'DELETE' });
}
