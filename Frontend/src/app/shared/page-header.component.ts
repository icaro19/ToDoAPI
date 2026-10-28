import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-page-header',
  standalone: true,

  template: `
    <div class="mb-4">
      <h2>{{title}}</h2>
    </div>
  `
})
export class PageHeaderComponent {

  @Input()
  title = '';
}
