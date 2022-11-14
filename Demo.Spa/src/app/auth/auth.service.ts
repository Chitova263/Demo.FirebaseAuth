import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {}

  public login(username: string, password: string) {
    return this.http.post('/api/auth/login', {username, password});
  }

  public register(username: string, password: string) {
    return this.http.post('/api/auth/register', {username, password});
  }
}
