import { DatePipe } from '@angular/common';
import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { forkJoin, finalize } from 'rxjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { Department, Employee } from '../../core/models/erp.model';
import { ErpService, errorMessage } from '../../core/services/erp.service';

@Component({
  selector: 'app-dashboard', standalone: true,
  imports: [DatePipe, RouterLink, NzButtonModule, NzSpinModule],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  private readonly api = inject(ErpService);
  private readonly destroyRef = inject(DestroyRef);
  departments: Department[] = [];
  employees: Employee[] = [];
  recent: Employee[] = [];
  loading = false;
  error = '';
  ngOnInit(): void { this.load(); }
  load(): void {
    this.loading = true;
    this.error = '';
    forkJoin({ departments: this.api.departments(), employees: this.api.employees() })
      .pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.loading = false))
      .subscribe({
        next: data => {
          this.departments = data.departments;
          this.employees = data.employees;
          this.recent = [...data.employees].sort((a, b) => b.dateJoined.localeCompare(a.dateJoined)).slice(0, 5);
        },
        error: e => this.error = errorMessage(e)
      });
  }
}
