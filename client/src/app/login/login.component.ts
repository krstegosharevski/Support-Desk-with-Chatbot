import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { UserToken } from '../_models/userToken';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {};


  constructor(public accountService : AccountService, private router: Router) { }

  ngOnInit(): void {
  }

 

  login(){
    if (this.model.selectedRole) {
      this.model.selectedRole = Number(this.model.selectedRole);
    }

    console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        this.router.navigate(['/']);
      },
      error : error => console.log(error)
      
    })
   // this.router.navigate(['/']);
  }

  logout(){
    this.accountService.logout();
    this.router.navigate(['/']).then(() => {
      window.location.reload();
    });
  }

}
