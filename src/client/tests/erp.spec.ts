import { test, expect } from '@playwright/test';

test('interview flow: dashboard, department and employee CRUD, FK protection and search', async ({ page, request }) => {
  const errors: string[] = [];
  page.on('pageerror', error => errors.push(error.message));
  const tag = 'A UI Demo ' + Date.now();
  const api = 'http://localhost:5028/api';
  let departmentId: number | undefined;
  let employeeId: number | undefined;
  try {
    await page.goto('/dashboard');
    await expect(page.getByRole('heading', { name: 'Dashboard', exact: true })).toBeVisible();
    await expect(page.getByRole('link', { name: /Total employees/ })).toContainText('10');
    await expect(page.getByRole('link', { name: /Total departments/ })).toContainText('8');
    await expect(page.locator('.ant-spin-blur')).toHaveCount(0);
    await page.screenshot({ path: '../../.artifacts/dashboard-desktop.png', fullPage: true, animations: 'disabled' });

    await page.getByRole('navigation').getByRole('link', { name: 'Departments', exact: true }).click();
    await page.getByRole('button', { name: '+ Add department' }).click();
    await page.getByRole('button', { name: 'Save department', exact: true }).click();
    await expect(page.getByText('Enter a department name (up to 200 characters).')).toBeVisible();
    await page.getByLabel('Department name').fill(tag);
    await page.getByLabel('Department address').fill('Fictional UI office');
    await page.getByRole('button', { name: 'Save department', exact: true }).click();
    await expect(page.getByText('Department created successfully.')).toBeVisible();
    const allDepartments = (await (await request.get(api + '/departments')).json()).data;
    departmentId = allDepartments.find((d: any) => d.departmentName === tag).departmentId;
    let departmentRow = page.getByRole('row').filter({ hasText: tag });
    await departmentRow.getByRole('button', { name: 'View', exact: true }).click();
    await expect(page.getByRole('heading', { name: tag, exact: true })).toBeVisible();
    await page.getByRole('button', { name: 'Close' }).click();
    await departmentRow.getByRole('button', { name: 'Edit', exact: true }).click();
    await expect(page.getByLabel('Department name')).toHaveValue(tag);
    await page.getByLabel('Department name').fill(tag + ' Revised');
    await page.getByRole('button', { name: 'Save department', exact: true }).click();
    await expect(page.getByText('Department updated successfully.')).toBeVisible();

    await page.getByRole('navigation').getByRole('link', { name: 'Employees', exact: true }).click();
    await page.getByRole('button', { name: '+ Add employee' }).click();
    await page.getByRole('button', { name: 'Save employee', exact: true }).click();
    await expect(page.getByText('First name is required (up to 100 characters).')).toBeVisible();
    await page.getByLabel('First name', { exact: false }).fill('AUi');
    await page.getByLabel('Last name', { exact: false }).fill(tag);
    await page.locator('#employee-department').selectOption({ label: tag + ' Revised' });
    await page.getByLabel('Gender', { exact: false }).selectOption('Other');
    await page.getByLabel('Date of birth', { exact: false }).fill('1995-05-10');
    await page.getByLabel('Date joined', { exact: false }).fill('1995-05-10');
    await page.getByRole('button', { name: 'Save employee', exact: true }).click();
    await expect(page.getByText('Date joined must be later than date of birth.')).toBeVisible();
    await page.getByLabel('Date joined', { exact: false }).fill('2024-01-15');
    await page.locator('#employee-address').fill('Fictional employee address');
    await page.getByRole('button', { name: 'Save employee', exact: true }).click();
    await expect(page.getByText('Employee created successfully.')).toBeVisible();
    const matches = (await (await request.get(api + '/employees?search=' + encodeURIComponent(tag))).json()).data;
    employeeId = matches[0].employeeId;
    let employeeRow = page.getByRole('row').filter({ hasText: 'AUi ' + tag });
    await expect(employeeRow).toContainText(tag + ' Revised');
    await employeeRow.getByRole('button', { name: 'Edit', exact: true }).click();
    await expect(page.locator('#employee-department option:checked')).toHaveText(tag + ' Revised');
    await page.getByLabel('First name', { exact: false }).fill('AUiUpdated');
    await page.getByRole('button', { name: 'Save employee', exact: true }).click();
    await expect(page.getByText('Employee updated successfully.')).toBeVisible();
    employeeRow = page.getByRole('row').filter({ hasText: 'AUiUpdated ' + tag });
    await employeeRow.getByRole('button', { name: 'View', exact: true }).click();
    await expect(page.getByRole('heading', { name: 'AUiUpdated ' + tag, exact: true })).toBeVisible();
    await expect(page.getByText('Fictional employee address', { exact: true })).toBeVisible();
    await expect(page.locator('.ant-message-notice')).toHaveCount(0);
    await page.screenshot({ path: '../../.artifacts/employee-detail.png', fullPage: true, animations: 'disabled' });
    await page.getByRole('button', { name: 'Close' }).click();
    await page.getByRole('textbox', { name: 'Search employees' }).fill(tag);
    await page.getByRole('button', { name: 'Search', exact: true }).click();
    await expect(page.locator('tbody tr.ant-table-row')).toHaveCount(1);
    await page.reload();
    await expect(page.getByRole('row').filter({ hasText: 'AUiUpdated ' + tag })).toBeVisible();

    await page.getByRole('navigation').getByRole('link', { name: 'Departments', exact: true }).click();
    departmentRow = page.getByRole('row').filter({ hasText: tag + ' Revised' });
    await departmentRow.getByRole('button', { name: 'Delete', exact: true }).click();
    await page.getByRole('button', { name: 'Delete department', exact: true }).click();
    await expect(page.getByText('Cannot delete this department because employees are currently assigned to it.')).toBeVisible();
    await page.getByRole('button', { name: 'Cancel', exact: true }).click();

    await page.getByRole('navigation').getByRole('link', { name: 'Employees', exact: true }).click();
    employeeRow = page.getByRole('row').filter({ hasText: 'AUiUpdated ' + tag });
    await employeeRow.getByRole('button', { name: 'Delete', exact: true }).click();
    await page.getByRole('button', { name: 'Cancel', exact: true }).click();
    await expect(employeeRow).toBeVisible();
    await employeeRow.getByRole('button', { name: 'Delete', exact: true }).click();
    await page.getByRole('button', { name: 'Delete employee', exact: true }).click();
    await expect(page.getByText('Employee deleted successfully.')).toBeVisible();
    employeeId = undefined;
    await page.getByRole('navigation').getByRole('link', { name: 'Departments', exact: true }).click();
    await page.getByRole('row').filter({ hasText: tag + ' Revised' }).getByRole('button', { name: 'Delete', exact: true }).click();
    await page.getByRole('button', { name: 'Delete department', exact: true }).click();
    await expect(page.getByText('Department deleted successfully.')).toBeVisible();
    departmentId = undefined;
    expect(errors).toEqual([]);
  } finally {
    if (employeeId) await request.delete(api + '/employees/' + employeeId);
    if (departmentId) await request.delete(api + '/departments/' + departmentId);
  }
});

test('mobile layout and empty search', async ({ page }) => {
  await page.setViewportSize({ width: 390, height: 844 });
  await page.goto('/dashboard');
  await expect(page.getByRole('heading', { name: 'Dashboard', exact: true })).toBeVisible();
  await expect(page.getByRole('link', { name: /Total employees/ })).toContainText('10');
  await expect(page.locator('.ant-spin-blur')).toHaveCount(0);
  await page.screenshot({ path: '../../.artifacts/dashboard-mobile.png', fullPage: true, animations: 'disabled' });
  expect(await page.evaluate(() => document.documentElement.scrollWidth)).toBeLessThanOrEqual(390);
  await page.getByRole('navigation').getByRole('link', { name: 'Employees', exact: true }).click();
  await page.getByRole('textbox', { name: 'Search employees' }).fill('No matching employee xyz');
  await page.getByRole('button', { name: 'Search', exact: true }).click();
  await expect(page.locator('.ant-empty')).toBeVisible();
  expect(await page.evaluate(() => document.documentElement.scrollWidth)).toBeLessThanOrEqual(390);
});

test('connection error is readable and Retry recovers', async ({ page }) => {
  await page.route('**/api/departments', route => route.abort());
  await page.goto('/departments');
  await expect(page.getByRole('alert')).toContainText('Cannot connect to the API');
  await page.unroute('**/api/departments');
  await page.getByRole('button', { name: 'Retry' }).click();
  await expect(page.getByRole('row').filter({ hasText: 'Information Technology' })).toBeVisible();
  await expect(page.getByRole('alert')).toHaveCount(0);
});
