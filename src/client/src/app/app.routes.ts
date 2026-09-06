import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent), title: 'Dashboard | ERP Demo' },
  { path: 'departments', loadComponent: () => import('./features/departments/departments.component').then(m => m.DepartmentsComponent), title: 'Departments | ERP Demo' },
  { path: 'employees', loadComponent: () => import('./features/employees/employees.component').then(m => m.EmployeesComponent), title: 'Employees | ERP Demo' },
  { path: '**', redirectTo: 'dashboard' }
];
