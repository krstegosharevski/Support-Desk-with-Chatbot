import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TicketsService } from '../_services/tickets.service';
import { Ticket } from '../_models/ticket';
import { TicketDto } from '../_models/ticketDto';
import { UserUsernameDto1 } from '../_models/userUsernameDto1';
import { CommentService } from '../_services/comment.service';
import { CommentDto } from '../_models/commentDto';
import { formatDistanceToNow } from 'date-fns';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { usersToBeApproved } from '../_models/usersToBeApproved';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css']
})
export class TicketComponent implements OnInit {

  ticket: Ticket | undefined;
  username: UserUsernameDto1 | undefined;
  comments: CommentDto[] = [];
  newCommentText: string = '';
  userId : number | undefined;
  assignedToTicket: boolean = false;
  authenticatedUsers: usersToBeApproved[] = [];
  selectedUsername: string | undefined;
  ticket_id : number = 0;



  constructor(private route: ActivatedRoute,
    private ticketService: TicketsService,
    private commentService: CommentService,
    private ticketsService: TicketsService,
    private accountService: AccountService) { }

  ngOnInit() {
    const ticketId = this.route.snapshot.paramMap.get('id');
    
    this.loadAuthenticatedUsers();
    this.ticketService.getTicketById(ticketId).subscribe(ticket => {
      this.ticket = ticket;

      if (this.ticket?.createdById) {
        this.findTicketUser(this.ticket.createdById);
      }

      this.findAllComments(this.ticket.ticketId);
    });

    this.getUserIdFromToken().subscribe(userId => {
      const ticketId2 = ticketId ? +ticketId : 0;
      if (ticketId && userId) {
        
        this.ticketService.getTicketsAssignedToUser(ticketId2, userId).subscribe(
          (isAssigned: boolean) => {
            this.assignedToTicket = isAssigned;
          },
          error => console.log('Грешка при проверка дали корисникот е назначен:', error)
        );
      }
    });

  }

  saveTicket(): void {
    if (this.ticket) {
      const ticketUpdateDto: TicketDto = {
        ticketId: this.ticket.ticketId,
        response: this.ticket.response,
        status: this.ticket.status,
      };
      this.ticketService.updateTicket(ticketUpdateDto).subscribe({
        next: () => console.log('Ticket updated successfully'),
        error: (err) => console.error('Error updating ticket:', err)
      });
    }
  }

  findTicketUser(userId: number) {
    this.ticketService.findUsernameById(userId).subscribe(
      response => {
        this.username = response
        console.log("Successfully get the user")
      },
      error => console.log(error)
    )
  }

  findAllComments(ticketId: number) {
    this.commentService.getCommentsForTicket(ticketId).subscribe(
      (response: CommentDto[]) => {
        this.comments = response
        console.log("Successfully added comments!")
      },
      (error) => console.log(error)
    )
  }

  getRelativeTime(dateString: Date): string {
    const date = new Date(dateString);
    return formatDistanceToNow(date, { addSuffix: true });
  }

  

  addComment() {
    if (this.ticket && this.newCommentText.trim()) {
      this.getUserIdFromToken().subscribe({
        next: (userId) => {
          const commentDto: CommentDto = {
            ticketId: this.ticket?.ticketId ?? 0,
            userId: userId, // Use the userId obtained from the Observable
            commentText: this.newCommentText,
            createdAt: new Date()
          };
  
          this.commentService.addCommentForTicket(commentDto).subscribe({
            next: (comment) => {
              this.comments.push(comment); // Add the new comment to the list
              this.newCommentText = ''; // Clear the textarea
              console.log('Comment added successfully');
            },
            error: (err) => console.error('Error adding comment:', err)
          });
        },
        error: (err) => console.error('Error fetching user ID:', err)
      });
    }
  }

  getUserIdFromToken(): Observable<number> {
    
    return this.accountService.getUserId(); // Пример за враќање на ID-то
  }

  loadAuthenticatedUsers() {
    this.accountService.getAuthenticatedUsers().subscribe({
      next: (response) => {
        this.authenticatedUsers = response;
      },
      error: (error) => console.log('Error loading users:', error)
    });
  }

  assignToUser(username: string | undefined){
    if(this.ticket?.ticketId && username)
    this.ticketService.assignTicketToUser(this.ticket?.ticketId, username).subscribe({
      next: () => {
        console.log("Successfuly assigned")
      },
      error: error => console.log(error)
    })
  }

}
