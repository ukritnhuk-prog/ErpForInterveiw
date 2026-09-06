import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse, Department, DepartmentRequest, Employee, EmployeeRequest } from '../models/erp.model';

export function errorMessage(error: unknown): string {
  if (!(error instanceof HttpErrorResponse)) return 'Something went wrong. Please try again.';
  if (error.status === 0) return 'Cannot connect to the API. Check that the backend is running, then retry.';
  const body = error.error;
  if (body?.errors) return Object.values(body.errors).flat().join(' ');
  return body?.message || body?.title || 'The request failed. Please try again.';
}

@Injectable({ providedIn: 'root' })
export class ErpService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  departments() {
    return this.http.get<ApiResponse<Department[]>>(this.base + '/api/departments').pipe(map(r => r.data));
  }
  department(id: number) {
    return this.http.get<ApiResponse<Department>>(this.base + '/api/departments/' + id).pipe(map(r => r.data));
  }
  saveDepartment(id: number | null, data: DepartmentRequest) {
    return (id === null
      ? this.http.post<ApiResponse<Department>>(this.base + '/api/departments', data)
      : this.http.put<ApiResponse<Department>>(this.base + '/api/departments/' + id, data)).pipe(map(r => r.data));
  }
  deleteDepartment(id: number) { return this.http.delete<void>(this.base + '/api/departments/' + id); }

  employees(search = '', departmentId: number | null = null) {
    let params = new HttpParams();
    if (search.trim()) params = params.set('search', search.trim());
    if (departmentId !== null) params = params.set('departmentId', departmentId);
    return this.http.get<ApiResponse<Employee[]>>(this.base + '/api/employees', { params }).pipe(map(r => r.data));
  }
  employee(id: number) {
    return this.http.get<ApiResponse<Employee>>(this.base + '/api/employees/' + id).pipe(map(r => r.data));
  }
  saveEmployee(id: number | null, data: EmployeeRequest) {
    return (id === null
      ? this.http.post<ApiResponse<Employee>>(this.base + '/api/employees', data)
      : this.http.put<ApiResponse<Employee>>(this.base + '/api/employees/' + id, data)).pipe(map(r => r.data));
  }
  deleteEmployee(id: number) { return this.http.delete<void>(this.base + '/api/employees/' + id); }
}
