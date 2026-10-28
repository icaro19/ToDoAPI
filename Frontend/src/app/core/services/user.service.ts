import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { User } from '../models/user/user';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private readonly http: HttpClient
  ) { }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(
      `${environment.apiUrl}/users`
    );
  }

  getById(id: string): Observable<User> {
    return this.http.get<User>(
      `${environment.apiUrl}/users/${id}`
    );
  }
}
