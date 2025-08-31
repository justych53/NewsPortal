import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth';

export interface Category {
  id: string;
  name: string;
}

export interface CategoryRequest {
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = '/api/category'; 

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

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Ошибка получения категорий:', error);
        throw error;
      })
    );
  }

  createCategory(categoryRequest: CategoryRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, categoryRequest, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка создания категории:', error);
        throw error;
      })
    );
  }

  updateCategory(id: string, categoryRequest: CategoryRequest): Observable<string> {
    return this.http.put<string>(`${this.apiUrl}/${id}`, categoryRequest, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка обновления категории:', error);
        throw error;
      })
    );
  }

  deleteCategory(id: string): Observable<string> {
    return this.http.delete<string>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка удаления категории:', error);
        throw error;
      })
    );
  }
}
