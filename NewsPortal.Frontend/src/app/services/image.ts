import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from './auth';

export interface Image {
  id: string;
  url: string;
}

export interface ImageRequest {
  url: string;
}

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private apiUrl = '/api/images';
  private baseUrl = '';

  constructor(
    private http: HttpClient,
    private authService: AuthService // Добавляем AuthService
  ) { }

  // Получение заголовков с авторизацией
  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getSliderImages(): Observable<Image[]> {
    console.log(' Запрос к API images');

    return this.http.get<Image[]>(`${this.apiUrl}/slider`).pipe(
      map(images => {
        console.log(' Ответ от API:', images);
        return this.processImages(images);
      }),
      catchError(error => {
        console.error(' Ошибка API:', error);
        return of([]);
      })
    );
  }

  private processImages(images: Image[]): Image[] {
    return images.map(img => {
      let finalUrl = img.url;

      if (finalUrl.startsWith('http://') || finalUrl.startsWith('https://')) {
        return img;
      }

      if (finalUrl.startsWith('/')) {
        finalUrl = finalUrl;
      }
      else if (finalUrl.includes('\\')) {
        const fileName = finalUrl.split('\\').pop();
        finalUrl = `/images/${fileName}`;
      }
      else {
        finalUrl = `/images/${finalUrl}`;
      }

      console.log(` Обработан URL: ${img.url} -> ${finalUrl}`);

      return {
        ...img,
        url: finalUrl
      };
    });
  }

  // Получение всех изображений
  getAllImages(): Observable<Image[]> {
    return this.http.get<Image[]>(this.apiUrl).pipe(
      map(images => this.processImages(images)),
      catchError(error => {
        console.error('Ошибка получения изображений:', error);
        return of([]);
      })
    );
  }

  // Создание изображения 
  createImage(imageRequest: ImageRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, imageRequest, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка создания изображения:', error);
        throw error;
      })
    );
  }

  // Обновление изображения
  updateImage(id: string, imageRequest: ImageRequest): Observable<string> {
    return this.http.put<string>(`${this.apiUrl}/${id}`, imageRequest, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка обновления изображения:', error);
        throw error;
      })
    );
  }

  // Удаление изображения
  deleteImage(id: string): Observable<string> {
    return this.http.delete<string>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(error => {
        console.error('Ошибка удаления изображения:', error);
        throw error;
      })
    );
  }

  // Загрузка файла 
  uploadImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post(this.apiUrl, formData, { headers }).pipe(
      catchError(error => {
        console.error('Ошибка загрузки изображения:', error);
        throw error;
      })
    );
  }
}
