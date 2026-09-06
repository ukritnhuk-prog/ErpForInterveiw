import { DatePipe } from '@angular/common';
import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, finalize, firstValueFrom, forkJoin } from 'rxjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Department, Employee } from '../../core/models/erp.model';
import { ErpService, errorMessage } from '../../core/services/erp.service';
import { employmentDates, localToday, nonBlank } from '../../core/validation';

@Component({
  selector: 'app-employees', standalone: true,
  imports: [DatePipe, FormsModule, ReactiveFormsModule, NzButtonModule, NzInputModule, NzTableModule, NzModalModule],
  templateUrl: './employees.component.html'
})
export class EmployeesComponent implements OnInit {
  private readonly api = inject(ErpService);
  private readonly fb = inject(FormBuilder);
  private readonly modal = inject(NzModalService);
  private readonly message = inject(NzMessageService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private loadSubscription?: Subscription;
  employees: Employee[] = [];
  departments: Department[] = [];
  search = '';
  filterDepartment: number | null = null;
  loading = false;
  saving = false;
  error = '';
  formError = '';
  editingId: number | null = null;
  visible = false;
  detail: Employee | null = null;
  readonly today = localToday();
  readonly genders = ['Male', 'Female', 'Other', 'Prefer not to say'];
  readonly form = this.fb.nonNullable.group({
    firstName: ['', [nonBlank, Validators.maxLength(100)]],
    lastName: ['', [nonBlank, Validators.maxLength(100)]],
    departmentId: [0, [Validators.required, Validators.min(1)]],
    gender: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    dateJoined: ['', Validators.required],
    employeeAddress: ['', Validators.maxLength(500)]
  }, { validators: employmentDates });

  ngOnInit(): void {
    this.load();
    this.route.queryParamMap.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(params => {
      if (params.get('add') === 'true') {
        this.add();
        void this.router.navigate([], { queryParams: {}, replaceUrl: true });
      }
    });
  }
  load(): void {
    this.loadSubscription?.unsubscribe();
    this.loading = true;
    this.error = '';
    this.loadSubscription = forkJoin({
      employees: this.api.employees(this.search, this.filterDepartment),
      departments: this.api.departments()
    }).pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.loading = false)).subscribe({
      next: data => { this.employees = data.employees; this.departments = data.departments; },
      error: e => this.error = errorMessage(e)
    });
  }
  resetSearch(): void { this.search = ''; this.filterDepartment = null; this.load(); }
  add(): void {
    this.editingId = null;
    this.form.reset();
    this.formError = '';
    this.visible = true;
  }
  edit(row: Employee): void {
    forkJoin({ employee: this.api.employee(row.employeeId), departments: this.api.departments() })
      .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
        next: ({ employee, departments }) => {
          this.departments = departments;
          this.editingId = employee.employeeId;
          this.form.reset({
            firstName: employee.firstName, lastName: employee.lastName, departmentId: employee.departmentId,
            gender: employee.gender, dateOfBirth: employee.dateOfBirth, dateJoined: employee.dateJoined,
            employeeAddress: employee.employeeAddress ?? ''
          });
          this.formError = '';
          this.visible = true;
        },
        error: e => this.message.error(errorMessage(e))
      });
  }
  view(row: Employee): void {
    this.api.employee(row.employeeId).pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({ next: data => this.detail = data, error: e => this.message.error(errorMessage(e)) });
  }
  save(): void {
    if (this.saving) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.saving = true;
    this.formError = '';
    const wasEditing = this.editingId !== null;
    this.api.saveEmployee(this.editingId, this.form.getRawValue())
      .pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.saving = false)).subscribe({
        next: () => {
          this.visible = false;
          this.message.success(wasEditing ? 'Employee updated successfully.' : 'Employee created successfully.');
          this.load();
        },
        error: e => this.formError = errorMessage(e)
      });
  }
  remove(row: Employee): void {
    this.modal.confirm({
      nzTitle: 'Delete employee?',
      nzContent: 'Are you sure you want to delete this employee? This action cannot be undone.',
      nzOkText: 'Delete employee', nzOkDanger: true,
      nzOnOk: async () => {
        try {
          await firstValueFrom(this.api.deleteEmployee(row.employeeId));
          this.message.success('Employee deleted successfully.');
          this.load();
        } catch (e) {
          this.message.error(errorMessage(e));
          throw e;
        }
      }
    });
  }
}
