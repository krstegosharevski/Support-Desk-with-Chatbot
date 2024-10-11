import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { UserToken } from '../_models/userToken';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  loggedIn: boolean = false;

  constructor(public accountService: AccountService, private router: Router) {
   }

  ngOnInit(): void {
    // this.accountService.loggedIn$.subscribe(loggedIn => {
    //   this.loggedIn = loggedIn;
    // });
  }

  logout(){
    this.accountService.logout();
    this.loggedIn = false;
    this.router.navigate(['/']).then(() => {
      window.location.reload();
    });
  }

}
