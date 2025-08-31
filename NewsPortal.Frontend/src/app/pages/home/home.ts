import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NewsService, NewsArticle } from '../../services/news';
import { CategoryService, Category } from '../../services/category';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrls: ['./home.scss'],
  standalone: false
})
export class Home implements OnInit {
  newsArticles: NewsArticle[] = [];
  filteredNews: NewsArticle[] = [];
  categories: Category[] = [];
  paginatedNews: NewsArticle[] = [];
  
  // Фильтры
  selectedCategory: string = 'all';
  dateFilter: string = 'all';
  searchQuery: string = '';
  
  currentPage: number = 1;
  itemsPerPage: number = 9;
  totalPages: number = 0;
  isLoading: boolean = true;
  error: string = '';

  constructor(
    private router: Router,
    private newsService: NewsService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadNews();
    }

  loadNews(): void {
    this.isLoading = true;
    this.error = '';
    
    this.newsService.getNews().subscribe({
      next: (news) => {
        this.newsArticles = news;
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading news:', error);
        this.error = 'Ошибка загрузки новостей';
        this.isLoading = false;
        this.newsArticles = [];
        this.applyFilters();
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }

  applyFilters(): void {
    let filtered = [...this.newsArticles];

    // Фильтр по категории
    if (this.selectedCategory !== 'all') {
      filtered = filtered.filter(news => news.categoryId === this.selectedCategory);
    }

    // Фильтр по дате
    if (this.dateFilter !== 'all') {
      const now = new Date();
      filtered = filtered.filter(news => {
        const newsDate = new Date(news.createdAt);
        
        switch (this.dateFilter) {
          case 'today':
            return this.isSameDay(newsDate, now);
          case 'week':
            return this.isThisWeek(newsDate);
          case 'month':
            return this.isThisMonth(newsDate);
          default:
            return true;
        }
      });
    }

    // Поиск по заголовку
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase().trim();
      filtered = filtered.filter(news => 
        news.title.toLowerCase().includes(query) ||
        (news.shortPhrase && news.shortPhrase.toLowerCase().includes(query))
      );
    }

    this.filteredNews = filtered;
    this.currentPage = 1; // Сбрасываем на первую страницу при фильтрации
    this.updatePagination();
  }

  // Вспомогательные методы для фильтрации по дате
  private isSameDay(date1: Date, date2: Date): boolean {
    return date1.getDate() === date2.getDate() &&
           date1.getMonth() === date2.getMonth() &&
           date1.getFullYear() === date2.getFullYear();
  }

  private isThisWeek(date: Date): boolean {
    const now = new Date();
    const startOfWeek = new Date(now);
    startOfWeek.setDate(now.getDate() - now.getDay());
    startOfWeek.setHours(0, 0, 0, 0);
    
    const endOfWeek = new Date(now);
    endOfWeek.setDate(now.getDate() + (6 - now.getDay()));
    endOfWeek.setHours(23, 59, 59, 999);
    
    return date >= startOfWeek && date <= endOfWeek;
  }

  private isThisMonth(date: Date): boolean {
    const now = new Date();
    return date.getMonth() === now.getMonth() && 
           date.getFullYear() === now.getFullYear();
  }

  onCategoryChange(categoryId: string): void {
    this.selectedCategory = categoryId;
    this.applyFilters();
  }

  onDateFilterChange(filter: string): void {
    this.dateFilter = filter;
    this.applyFilters();
  }

  onSearch(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.selectedCategory = 'all';
    this.dateFilter = 'all';
    this.searchQuery = '';
    this.applyFilters();
  }

  getCategoryName(categoryId: string): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.name : 'Общее';
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredNews.length / this.itemsPerPage);
    
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    
    this.paginatedNews = this.filteredNews.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
      window.scrollTo(0, 0);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
      window.scrollTo(0, 0);
    }
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.updatePagination();
    window.scrollTo(0, 0);
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    
    if (this.totalPages <= maxVisiblePages) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
      return pages;
    }
    
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);
    
    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    
    return pages;
  }

  openNewsPage(id: string): void {
    this.router.navigate(['/news', id]);
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
        year: 'numeric'
      });
    } catch (error) {
      console.error('Date formatting error:', error);
      return 'Ошибка даты';
    }
    }
    getDateFilterText(): string {
        switch (this.dateFilter) {
            case 'today': return 'Сегодня';
            case 'week': return 'Неделя';
            case 'month': return 'Месяц';
            default: return '';
        }
    }
}
