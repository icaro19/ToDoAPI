import { Component, Input } from '@angular/core';

import { TaskStatus } from '../core/models/task-status.enum';

@Component({
  selector: 'app-task-status-badge',
  standalone: true,

  template: `
    <span class="badge bg-primary">
      {{getLabel()}}
    </span>
  `
})
export class TaskStatusBadgeComponent {

  @Input({ required: true })
  status!: TaskStatus;

  getLabel(): string {

    switch (this.status) {

      case TaskStatus.Pending:
        return 'Pendente';

      case TaskStatus.InProgress:
        return 'Em andamento';

      case TaskStatus.Completed:
        return 'Concluída';

      case TaskStatus.Cancelled:
        return 'Cancelada';

      default:
        return '';
    }
  }
}
