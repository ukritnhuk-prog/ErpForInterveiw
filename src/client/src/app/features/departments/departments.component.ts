import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, firstValueFrom } from 'rxjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Department } from '../../core/models/erp.model';
import { ErpService, errorMessage } from '../../core/services/erp.service';
import { nonBlank } from '../../core/validation';

@Component({
  selector: 'app-departments', standalone: true,
  imports: [ReactiveFormsModule, NzButtonModule, NzInputModule, NzTableModule, NzModalModule],
  templateUrl: './departments.component.html'
})
export class DepartmentsComponent implements OnInit {
  private readonly api = inject(ErpService);
  private readonly fb = inject(FormBuilder);
  private readonly modal = inject(NzModalService);
  private readonly message = inject(NzMessageService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  departments: Department[] = [];
  loading = false;
  saving = false;
  error = '';
  formError = '';
  editingId: number | null = null;
  visible = false;
  detail: Department | null = null;
  readonly form = this.fb.nonNullable.group({
    departmentName: ['', [nonBlank, Validators.maxLength(200)]],
    departmentAddress: ['', Validators.maxLength(500)]
  });

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
    this.loading = true;
    this.error = '';
    this.api.departments().pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.loading = false))
      .subscribe({ next: data => this.departments = data, error: e => this.error = errorMessage(e) });
  }
  add(): void {
    this.editingId = null;
    this.form.reset();
    this.formError = '';
    this.visible = true;
  }
  edit(row: Department): void {
    this.api.department(row.departmentId).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: data => {
        this.editingId = data.departmentId;
        this.form.reset({ departmentName: data.departmentName, departmentAddress: data.departmentAddress ?? '' });
        this.formError = '';
        this.visible = true;
      },
      error: e => this.message.error(errorMessage(e))
    });
  }
  view(row: Department): void {
    this.api.department(row.departmentId).pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({ next: data => this.detail = data, error: e => this.message.error(errorMessage(e)) });
  }
  save(): void {
    if (this.saving) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.saving = true;
    this.formError = '';
    const wasEditing = this.editingId !== null;
    this.api.saveDepartment(this.editingId, this.form.getRawValue())
      .pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.saving = false)).subscribe({
        next: () => {
          this.visible = false;
          this.message.success(wasEditing ? 'Department updated successfully.' : 'Department created successfully.');
          this.load();
        },
        error: e => this.formError = errorMessage(e)
      });
  }
  remove(row: Department): void {
    this.modal.confirm({
      nzTitle: 'Delete department?',
      nzContent: 'Are you sure you want to delete this department? Departments with assigned employees cannot be deleted.',
      nzOkText: 'Delete department', nzOkDanger: true,
      nzOnOk: async () => {
        try {
          await firstValueFrom(this.api.deleteDepartment(row.departmentId));
          this.message.success('Department deleted successfully.');
          this.load();
        } catch (e) {
          this.message.error(errorMessage(e));
          throw e;
        }
      }
    });
  }
}
