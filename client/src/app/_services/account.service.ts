import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { UserToken } from '../_models/userToken';
import { User } from '../_models/user';
import { usersToBeApproved } from '../_models/usersToBeApproved';
import { UserRegister } from '../_models/userRegister';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:7146/api/';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();


  constructor(private http: HttpClient) { }

  login(model: any){
    //this.loggedInSource.next(true);
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post<UserRegister>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUserSource.next(user);
        }
       // return user;
      })
    )
  }

  // setCurrentUser(user: UserToken){
  //   this.currentUserSource.next(user);
  // }
  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    // Your logout logic here.
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    //this.loggedInSource.next(false);
  }

  getUserUsername(): string {
    const userString = localStorage.getItem('user');
    if (!userString) return '';
  
    const user = JSON.parse(userString);
    return user ? user.username || '' : '';
  }

  getToken(): string | null {
    const userString = localStorage.getItem('user');
    if (!userString) return null;
    
    const user = JSON.parse(userString);
    return user ? user.token : null;
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]))
  }

  getUserId(): Observable<number>{
    const username = this.getUserUsername();
    return this.http.get<number>(this.baseUrl+"users/get-id?username="+username);
  }

  getSelectedRole(): Observable<number>{
    const username = this.getUserUsername();
    return this.http.get<number>(this.baseUrl+"users/get-role?username="+username)
  }

  getNotAuthenticatedUsers(): Observable<usersToBeApproved[]>{
    return this.http.get<usersToBeApproved[]>(this.baseUrl+"users/get-not/authenticated");
  }

  authenticateUser(username: string, projectId: number) : Observable<void>{
    return this.http.put<void>(this.baseUrl+"users/authorize/"+username+"/"+projectId, {});
  }

  getAuthenticatedUsers(): Observable<usersToBeApproved[]> {
    return this.http.get<usersToBeApproved[]>(this.baseUrl + 'users/get/authenticated');
  }
  
  
  // getCurrentUserValue(): UserToken | null {
  //   return this.currentUserSource.getValue();
  // }
  
}
