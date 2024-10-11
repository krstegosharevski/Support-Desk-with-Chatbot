import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Ticket } from '../_models/ticket';
import { TicketsService } from '../_services/tickets.service';
import { Router } from '@angular/router';
import { UserUsernameDto1 } from '../_models/userUsernameDto1';
import { UserUsernameDto } from '../_models/userUsernameDto';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { usersToBeApproved } from '../_models/usersToBeApproved';
import { ProjectDto } from '../_models/projectDto';
import { ProjectsService } from '../_services/projects.service';
import { th } from 'date-fns/locale';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  tickets: Ticket[] = [];
  username: UserUsernameDto1 | undefined;
  users: Map<number, string> = new Map();
  usersArray: { id: number, username: string }[] = [];
  role: number = 0;
  userId: number | undefined;
  usersToBeApproved: usersToBeApproved[] = []
  status: number = 0;
  projects: ProjectDto[] = []
  selectedProject: number = 0


  constructor(private authService: AuthService,
    private ticketsService: TicketsService,
    private router: Router,
    private accountService: AccountService,
    private projectService: ProjectsService) { }

  ngOnInit() {
    this.getAllProjects();
    this.getSelectedRole();
    this.getAllUsers();
    this.loadUsers();
    this.getUserIdFromToken();
  }

  getAllTickets(): void {
    this.status = 1;

    if (this.role === 2) {
      this.accountService.getUserId().subscribe({
        next: (response) => {
          this.userId = response;
          console.log(this.userId);
          this.ticketsService.getAssignedTicketsToUser(this.userId).subscribe(
            (response: Ticket[]) => {
              this.tickets = response;
              console.log(this.tickets);
            },
            (error) => { console.log("Error listing tickets!") }
          )
        },
        error: (error) => console.log(error)
      });


    } else if (this.role === 0) {

      this.accountService.getUserId().subscribe({
        next: (response) => {
          this.userId = response;
          console.log(this.userId);

          this.ticketsService.getCreatedTicketsByUser(this.userId).subscribe(
            (response: Ticket[]) => {
              this.tickets = response;
              console.log(this.tickets);
            },
            (error) => { console.log("Error listing tickets!") }
          )
        },
        error: (error) => console.log(error)
      });

    } else {
      this.ticketsService.getTickets().subscribe(
        (response: Ticket[]) => {
          this.tickets = response;
          console.log(this.tickets);
        },
        (error) => { console.log("Error listing tickets!") }
      )
      this.accountService.getNotAuthenticatedUsers().subscribe(
        (response: usersToBeApproved[]) => {
          this.usersToBeApproved = response;
          console.log("successfuly fetched the users!")
        },
        (error) => { console.log("Error fetching the users!") }
      )
    }

  }


  getAllUsers(): void {
    this.ticketsService.getUsers().subscribe(
      (users: UserUsernameDto[]) => {
        // Претворање на одговорот во Map
        this.users = new Map(users.map(user => [user.id, user.username]));

        // Конвертирање на Map во масив за рендерирање
        this.convertMapToArray();
      },
      (error) => console.log("Error listing users!")
    );
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Assigned';
      case 1:
        return 'Pending';
      case 2:
        return 'Closed';
      default:
        return 'Unknown Status';
    }
  }

  getButtonLabel(status: number): string {
    switch (status) {
      case 0:
        return 'View';
      case 1:
        return 'Continue';
      case 2:
        return 'View';
      default:
        return 'Unknown Status';
    }
  }

  onTicketClick(ticketId: number) {
    this.router.navigate(['/ticket', ticketId]);
  }

  // findTicketUser(userId: number){
  //   this.ticketsService.findUsernameById(userId).subscribe(
  //     response => {
  //       this.username = response
  //       console.log("Successfully get the user")
  //     },
  //     error => console.log(error)
  //   )
  // }

  findTicketUser(userId: number): string {
    return this.users.get(userId) || 'Unknown User'; // Вратете името на корисникот или 'Unknown User'
  }


  loadUsers() {
    // Вашиот код за добивање на корисници
    this.ticketsService.getUsers().subscribe(userList => {
      this.users = new Map(userList.map(user => [user.id, user.username]));
      this.convertMapToArray();
    });
  }

  convertMapToArray() {
    this.usersArray = Array.from(this.users, ([id, username]) => ({ id, username }));
  }

  getSelectedRole() {
    this.accountService.getSelectedRole().subscribe({
      next: response => {
        this.role = response
        console.log("The selected role is number: " + this.role);
        this.getAllTickets();
      },
      error: error => console.log(error)
    })



  }

  // getUserIdFromToken(): Observable<number> {

  //   return this.accountService.getUserId(); // Пример за враќање на ID-то
  // }

  getUserIdFromToken() {
    this.accountService.getUserId().subscribe({
      next: (response) => {
        this.userId = response;
        console.log(this.userId);
      },
      error: (error) => console.log(error)
    });
  }

  approveNewMember(username: string, projectId: number){
    this.accountService.authenticateUser(username, projectId).subscribe({
      next: () => {
        console.log(`User ${username} successfully authenticated`);
        this.getAllTickets(); // Префрли го ова тука, за да обновиш податоците по успешна акција
      },
      error: (error) => {
        console.error('Error authenticating user:', error);
      }
    }    
    )
  }

  getAllProjects(){
    this.projectService.getTickets().subscribe({
      next: response => {
        this.projects = response
        console.log("Successfully fetched the projects")
      },
      error: error => console.log(error)
    })
  }

}