import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-authenticated-layout',
  standalone: true,

  imports: [
    RouterOutlet
  ],

  template: `
    <div class="container-fluid">

      <nav class="navbar navbar-dark bg-dark">
        <span class="navbar-brand">
          Todo App
        </span>
      </nav>

      <div class="container mt-4">
        <router-outlet />
      </div>

    </div>
  `
})
export class AuthenticatedLayoutComponent {

}
