import { Component, OnInit } from '@angular/core';
import { TicketsService } from '../_services/tickets.service';
import { AccountService } from '../_services/account.service';
import { TicketDto } from '../_models/ticketDto';
import { Route, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { addTicketDto } from '../_models/addTicketDto';

@Component({
  selector: 'app-number-generator',
  templateUrl: './number-generator.component.html',
  styleUrls: ['./number-generator.component.css']
})
export class NumberGeneratorComponent implements OnInit {
  
  // ticket = {
  //     title: '',
  //     message: '',
  //     picture: null,
  //     createdByid: 7,
  //     assignedToId: 8 
  //   };

    title: string = '';
    message: string = '';
    selectedFile: File | null = null;

    private baseUrl = 'https://localhost:7146/api/ticket/add';  // Adjust with your API URL

  constructor(private http: HttpClient, 
      private jwtService: AccountService,
      private ticketService: TicketsService,
      private router: Router) { }

  ngOnInit(): void {
    
  }


  generateTicket(): void{
    if(this.title == null){
      console.log('Please fill the title');
      return;
    }

    const token = this.jwtService.getToken(); // Преместете ја JWT токенот од сервисот
    const username = this.jwtService.getUserUsername(); 

    console.log(username);

    if (!token || !username) {
      console.error('Token or username is not available.');
      return;
    }



    // const addTicketDto: addTicketDto = {
    //   title: this.title,
    //   message: this.message,
    //   picturePath: undefined,
    //   createdByUsername : username
    // }

    const formData = new FormData();
    formData.append('title', this.title);
    formData.append('message', this.message);
    formData.append('createdByUsername', username);

    if (this.selectedFile) {
        formData.append('picturePath', this.selectedFile, this.selectedFile.name);
    }


    this.ticketService.createTicket(formData, token).subscribe(
      response => {
        console.log('Ticket is added successfully: ',response);
        this.router.navigate(['/']);
      },
      error => {
        console.log('Error creating ticket: ', error);
        this.router.navigate(['/']);
      }
    )
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
        this.selectedFile = file;
    }
}

  
  

  // onSubmit() {
  //   const formData = new FormData();
  //   formData.append('title', this.ticket.title);
  //   formData.append('message', this.ticket.message);
  //   if (this.ticket.picture) {
  //     formData.append('file', this.ticket.picture);
  //   }

  //   this.http.post(this.baseUrl, formData).subscribe(response => {
  //     console.log('Ticket created successfully', response);
  //     // Handle success
  //   }, error => {
  //     console.error('Error creating ticket', error);
  //     // Handle error
  //   });
  // }
  

}
