import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProjectDto } from '../_models/projectDto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  private baseUrl = 'https://localhost:7146/api/projects';

  constructor(private http: HttpClient) { }

  getTickets(): Observable<ProjectDto[]>{
    return this.http.get<ProjectDto[]>(this.baseUrl);
  }

}
