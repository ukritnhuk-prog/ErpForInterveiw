import { Component } from '@angular/core';
import { LucideBuilding2, LucideLayoutDashboard, LucideUser } from '@lucide/angular';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root', standalone: true,
  imports: [LucideBuilding2, LucideLayoutDashboard, LucideUser, RouterLink, RouterLinkActive, RouterOutlet],
  template: `
    <div class="app-shell">
      <aside class="sidebar">
        <a class="brand" routerLink="/dashboard"><span class="brand-mark">ERP</span><span>ERP Demo<small>EMPLOYEE MANAGEMENT</small></span></a>
        <p class="nav-label">WORKSPACE</p>
        <nav aria-label="Main navigation">
          <a routerLink="/dashboard" routerLinkActive="active">
            <span class="nav-icon" aria-hidden="true"><svg lucideLayoutDashboard></svg></span>
            Dashboard
          </a>
          <a routerLink="/employees" routerLinkActive="active">
            <span class="nav-icon" aria-hidden="true"><svg lucideUser></svg></span>
            Employees
          </a>
          <a class="sub-nav" routerLink="/employees" [queryParams]="{add: 'true'}">+ Add employee</a>
          <a routerLink="/departments" routerLinkActive="active">
            <span class="nav-icon" aria-hidden="true"><svg lucideBuilding2></svg></span>
            Departments
          </a>
          <a class="sub-nav" routerLink="/departments" [queryParams]="{add: 'true'}">+ Add department</a>
        </nav>
        <div class="sidebar-note"><span class="status-dot"></span> Technical demonstration<br><small>Fictional data only</small></div>
      </aside>
      <div class="main-shell">
        <header class="topbar"><span>Employee & Department Management</span><span class="demo-pill">DEMO WORKSPACE</span></header>
        <main id="main-content" class="main-content"><router-outlet /></main>
        <footer>ERP Demo · Employee & Department Management · Technical interview project</footer>
      </div>
    </div>`
})
export class AppComponent {}
