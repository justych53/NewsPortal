import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth';

export interface NewsArticle {
  id: string;
  title: string;
  categoryId: string;
  shortPhrase: string;
  description: string;
  createdAt: string;
}

export interface NewsRequest {
  title: string;
  categoryId: string;
  shortPhrase: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  private apiUrl = '/api/news';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getNews(): Observable<NewsArticle[]> {
    return this.http.get<NewsArticle[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Ошибка получения новостей:', error);
        throw error;
      })
    );
  }
  createNews(newsRequest: NewsRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, newsRequest, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка создания новости:', error);
        throw error;
      })
    );
  }

  updateNews(id: string, newsRequest: NewsRequest): Observable<string> {
    return this.http.put<string>(`${this.apiUrl}/${id}`, newsRequest, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка обновления новости:', error);
        throw error;
      })
    );
  }

  deleteNews(id: string): Observable<string> {
    return this.http.delete<string>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка удаления новости:', error);
        throw error;
      })
    );
  }
  getNewsById(id: string): Observable<NewsArticle> {
    return this.http.get<NewsArticle>(`${this.apiUrl}/${id}`).pipe(
      catchError(error => {
        console.error('Ошибка получения новости:', error);
        throw error;
      })
    );
  }
}
