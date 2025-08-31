import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NewsService, NewsArticle } from '../../services/news';
import { CategoryService, Category } from '../../services/category';

@Component({
  selector: 'app-news-detail',
  templateUrl: './news-detail.html',
  styleUrls: ['./news-detail.scss'],
  standalone: false
})
export class NewsDetail implements OnInit {
  newsArticle: NewsArticle | null = null;
  category: Category | null = null;
  isLoading: boolean = true;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private newsService: NewsService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.loadNewsArticle();
  }

  loadNewsArticle(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.error = 'Новость не найдена';
      this.isLoading = false;
      return;
    }

    this.newsService.getNewsById(id).subscribe({
      next: (news) => {
        this.newsArticle = news;
        this.loadCategory(news.categoryId);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading news:', error);

        if (error.status === 404) {
          this.error = 'Новость не найдена';
        } else {
          this.error = 'Ошибка загрузки новости';
        }

        this.isLoading = false;
      }
    });
  }

  loadCategory(categoryId: string): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.category = categories.find(c => c.id === categoryId) || null;
      },
      error: (error) => {
        console.error('Error loading category:', error);
      }
    });
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'Дата не указана';

    try {
      let date: Date;

      if (dateString.includes('+')) {
        const cleanedDateString = dateString.split('.')[0];
        date = new Date(cleanedDateString + 'Z');
      } else {
        date = new Date(dateString);
      }

      if (isNaN(date.getTime())) {
        return 'Неверный формат даты';
      }

      return date.toLocaleDateString('ru-RU', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (error) {
      console.error('Date formatting error:', error);
      return 'Ошибка даты';
    }
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
