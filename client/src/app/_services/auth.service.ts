import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  isAdmin(): boolean {
    // Логика за проверка дали корисникот е администратор
    const userRole = localStorage.getItem('userRole'); // Пример, улогата може да ја добиеш од локално складиште
    return userRole === 'admin';
  }
}
