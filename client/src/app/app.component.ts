import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { UserToken } from './_models/userToken';
import { User } from './_models/user';
import { Observable } from 'rxjs';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title:string = 'Bank app';
  users: any;
 

  constructor(private http: HttpClient, private accountService: AccountService){
    
  }

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers(){
    this.http.get('https://localhost:7146/api/users/6', this.getHttpOptions()).subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    })
  }

  getHttpOptions(){
    const userString = localStorage.getItem('user');
    if(!userString){
      console.log(userString);
      return;
    } 
    const user = JSON.parse(userString);
    console.log(user.token)
    return{
      headers: new HttpHeaders({
        Authorization: `Bearer ` + user.token
      })
    }
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
