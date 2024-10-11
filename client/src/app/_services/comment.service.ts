import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentDto } from '../_models/commentDto';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private baseUrl = 'https://localhost:7146/api/comment/';

  constructor(private http: HttpClient) { }

  getCommentsForTicket(ticketId : number) : Observable<CommentDto[]> {
    return this.http.get<CommentDto[]>(this.baseUrl+ticketId);
  }

  addCommentForTicket(comment : CommentDto): Observable<CommentDto>{
    return this.http.post<CommentDto>(this.baseUrl+"add-comment", comment);
  }
}
