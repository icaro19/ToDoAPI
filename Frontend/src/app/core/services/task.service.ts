import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import { Task } from '../models/task/task';
import { TaskStatus } from '../models/task-status.enum';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(
    private readonly http: HttpClient
  ) { }

  getAll(): Observable<Task[]> {
    return this.http.get<Task[]>(
      `${environment.apiUrl}/tasks`
    );
  }

  getById(id: string): Observable<Task> {
    return this.http.get<Task>(
      `${environment.apiUrl}/tasks/${id}`
    );
  }

  getByStatus(
    status: TaskStatus
  ): Observable<Task[]> {

    return this.http.get<Task[]>(
      `${environment.apiUrl}/tasks/status/${status}`
    );
  }
}
