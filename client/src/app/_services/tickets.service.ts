import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Ticket } from '../_models/ticket';
import { TicketDto } from '../_models/ticketDto';
import { addTicketDto } from '../_models/addTicketDto';
import { UserUsernameDto1 } from '../_models/userUsernameDto1';
import { UserUsernameDto } from '../_models/userUsernameDto';

@Injectable({
  providedIn: 'root'
})
export class TicketsService {

  private baseUrl = 'https://localhost:7146/api/ticket/';

  constructor(private http: HttpClient) { }

  getTickets(): Observable<Ticket[]>{
    return this.http.get<Ticket[]>(this.baseUrl);
  }

  getCreatedTicketsByUser(id: number): Observable<Ticket[]>{
    return this.http.get<Ticket[]>(this.baseUrl+"created/"+id);
  }

  getAssignedTicketsToUser(id: number): Observable<Ticket[]>{
    return this.http.get<Ticket[]>(this.baseUrl+"assigned/"+id);
  }
 
  getTicketById(id: string | null) : Observable<Ticket>{
    return this.http.get<Ticket>(this.baseUrl + id);
  }

  getTicketsAssignedToUser(ticketId: number, assignedId: number) : Observable<boolean>{
    return this.http.get<boolean>(this.baseUrl+"assigned/"+ticketId+"/"+assignedId);
  }

  assignTicketToUser(ticketId: number, username: string) : Observable<void>{
    return this.http.put<void>(this.baseUrl+"assigned/"+ticketId+"/"+username, {});
  }

  // updateTicket(ticket: Ticket): Observable<void> {
  //   return this.http.put<void>(this.baseUrl + ticket.ticketId, ticket);
  // }
  updateTicket(ticket: TicketDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}${ticket.ticketId}`, ticket);
  }

  findUsernameById(ticketId: number): Observable<UserUsernameDto1>{
    return this.http.get<UserUsernameDto1>(this.baseUrl+'find/'+ticketId);
  }

  getUsers(): Observable<UserUsernameDto[]> {
    return this.http.get<UserUsernameDto[]>(this.baseUrl+'findAll'); // Променете ја URL-адресата според вашиот API
  }

  createTicket(addTicketDto : FormData, token: string): Observable<any>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    
    return this.http.post(this.baseUrl+'add', addTicketDto, {headers});
  }
}
