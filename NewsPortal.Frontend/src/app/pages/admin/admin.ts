import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { ImageService, Image, ImageRequest } from '../../services/image';
import { CategoryService, Category, CategoryRequest } from '../../services/category';
import { NewsService, NewsArticle, NewsRequest } from '../../services/news';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.html',
  styleUrls: ['./admin.scss'],
  standalone: false
})
export class Admin implements OnInit {
  authToken: string | null = null;
  currentSection: string = 'dashboard';
  images: Image[] = [];
  categories: Category[] = [];
  newsArticles: NewsArticle[] = [];

  newImage: ImageRequest = { url: '' };
  newCategory: CategoryRequest = { name: '' };
  newNews: NewsRequest = {
    title: '',
    categoryId: '',
    shortPhrase: '',
    description: ''
  };

  selectedFile: File | null = null;

  editingImage: Image | null = null;
  editingCategory: Category | null = null;
  editingNews: NewsArticle | null = null;
  editingFile: File | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private imageService: ImageService,
    private categoryService: CategoryService,
    private newsService: NewsService
  ) { }

    ngOnInit(): void {
        // Проверяем валидность токена при загрузке компонента
        this.authService.checkTokenValidity();

        this.authToken = this.authService.getToken();

        if (!this.authToken || !this.authService.isTokenValid()) {
            this.router.navigate(['/']);
            return;
        }

        this.loadImages();
        this.loadCategories();
        this.loadNews();
    }

  selectSection(section: string): void {
    this.currentSection = section;
  }

  loadImages(): void {
    this.imageService.getAllImages().subscribe({
      next: (images) => {
        this.images = images;
        console.log('Images loaded:', images);
      },
      error: (error) => {
        console.error('Error loading images:', error);
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
        console.log('Categories loaded:', categories);
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }

  loadNews(): void {
    this.newsService.getNews().subscribe({
      next: (news) => {
        this.newsArticles = news;
        console.log('News loaded:', news);
      },
      error: (error) => {
        console.error('Error loading news:', error);
      }
    });
  }

  getCategoryName(categoryId: string): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.name : 'Неизвестная категория';
  }

  onFileSelected(event: any, isEdit: boolean = false): void {
    const file = event.target.files[0];
    if (file) {
      if (isEdit) {
        this.editingFile = file;
        if (this.editingImage) {
          this.editingImage.url = `/images/${file.name}`;
        }
      } else {
        this.selectedFile = file;
        this.newImage.url = `/images/${file.name}`;
      }
    }
  }

  onEditFileSelected(event: any): void {
    this.onFileSelected(event, true);
  }

  createImage(): void {
    if (!this.newImage.url) {
      alert('Пожалуйста, выберите файл для формирования пути');
      return;
    }

    this.imageService.createImage(this.newImage).subscribe({
      next: (id) => {
        console.log('Image created with ID:', id);
        this.newImage.url = '';
        this.selectedFile = null;

        const fileInput = document.getElementById('imageFile') as HTMLInputElement;
        if (fileInput) {
          fileInput.value = '';
        }

        this.loadImages();
        alert('Путь к изображению успешно сохранен!');
      },
      error: (error) => {
        console.error('Error creating image:', error);
        alert('Ошибка сохранения пути к изображению: ' + error.message);
      }
    });
  }

  startEditImage(image: Image): void {
    this.editingImage = { ...image };
    this.editingFile = null;
  }

  updateImage(): void {
    if (!this.editingImage) return;

    const request: ImageRequest = { url: this.editingImage.url };
    this.imageService.updateImage(this.editingImage.id, request).subscribe({
      next: (id) => {
        console.log('Image updated:', id);
        this.editingImage = null;
        this.editingFile = null;
        this.loadImages();
      },
      error: (error) => {
        console.error('Error updating image:', error);
        alert('Ошибка обновления пути к изображению: ' + error.message);
      }
    });
  }

  deleteImage(id: string): void {
    if (!confirm('Удалить запись об изображении?')) return;

    this.imageService.deleteImage(id).subscribe({
      next: (result) => {
        console.log('Image deleted:', result);
        this.loadImages();
      },
      error: (error) => {
        console.error('Error deleting image:', error);
        alert('Ошибка удаления записи об изображении: ' + error.message);
      }
    });
  }

  createCategory(): void {
    if (!this.newCategory.name) return;

    this.categoryService.createCategory(this.newCategory).subscribe({
      next: (id) => {
        console.log('Category created with ID:', id);
        this.newCategory.name = '';
        this.loadCategories();
      },
      error: (error) => {
        console.error('Error creating category:', error);
        alert('Ошибка создания категории: ' + error.message);
      }
    });
  }

  startEditCategory(category: Category): void {
    this.editingCategory = { ...category };
  }

  updateCategory(): void {
    if (!this.editingCategory) return;

    const request: CategoryRequest = { name: this.editingCategory.name };
    this.categoryService.updateCategory(this.editingCategory.id, request).subscribe({
      next: (id) => {
        console.log('Category updated:', id);
        this.editingCategory = null;
        this.loadCategories();
      },
      error: (error) => {
        console.error('Error updating category:', error);
        alert('Ошибка обновления категории: ' + error.message);
      }
    });
  }

  deleteCategory(id: string): void {
    if (!confirm('Удалить категорию?')) return;

    this.categoryService.deleteCategory(id).subscribe({
      next: (result) => {
        console.log('Category deleted:', result);
        this.loadCategories();
      },
      error: (error) => {
        console.error('Error deleting category:', error);
        alert('Ошибка удаления категории: ' + error.message);
      }
    });
  }

  createNews(): void {
    if (!this.newNews.title || !this.newNews.categoryId || !this.newNews.description) {
      alert('Заполните все обязательные поля');
      return;
    }

    this.newsService.createNews(this.newNews).subscribe({
      next: (id) => {
        console.log('News created with ID:', id);
        this.newNews = { title: '', categoryId: '', shortPhrase: '', description: '' };
        this.loadNews();
      },
      error: (error) => {
        console.error('Error creating news:', error);
        alert('Ошибка создания новости: ' + error.message);
      }
    });
  }

  startEditNews(news: NewsArticle): void {
    this.editingNews = { ...news };
  }

  updateNews(): void {
    if (!this.editingNews) return;

    const request: NewsRequest = {
      title: this.editingNews.title,
      categoryId: this.editingNews.categoryId,
      shortPhrase: this.editingNews.shortPhrase,
      description: this.editingNews.description
    };

    this.newsService.updateNews(this.editingNews.id, request).subscribe({
      next: (id) => {
        console.log('News updated:', id);
        this.editingNews = null;
        this.loadNews();
      },
      error: (error) => {
        console.error('Error updating news:', error);
        alert('Ошибка обновления новости: ' + error.message);
      }
    });
  }

  deleteNews(id: string): void {
    if (!confirm('Удалить новость?')) return;

    this.newsService.deleteNews(id).subscribe({
      next: (result) => {
        console.log('News deleted:', result);
        this.loadNews();
      },
      error: (error) => {
        console.error('Error deleting news:', error);
        alert('Ошибка удаления новости: ' + error.message);
      }
    });
  }
}
