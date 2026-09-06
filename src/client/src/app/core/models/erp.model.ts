export interface ApiResponse<T> {
  succeeded: boolean;
  message?: string;
  data: T;
}

export interface Department {
  departmentId: number;
  departmentName: string;
  departmentAddress: string | null;
  employeeCount: number;
}

export interface DepartmentRequest {
  departmentName: string;
  departmentAddress: string;
}

export interface Employee {
  employeeId: number;
  departmentId: number;
  departmentName: string;
  firstName: string;
  lastName: string;
  fullName: string;
  gender: string;
  dateOfBirth: string;
  dateJoined: string;
  employeeAddress: string | null;
  photo: string | null;
}

export interface EmployeeRequest {
  departmentId: number;
  firstName: string;
  lastName: string;
  gender: string;
  dateOfBirth: string;
  dateJoined: string;
  employeeAddress: string;
}
