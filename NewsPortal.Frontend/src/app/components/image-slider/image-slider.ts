import { Component, OnInit } from '@angular/core';
import { ImageService } from '../../services/image';
import { Image } from '../../services/image'; 

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.html',
  styleUrls: ['./image-slider.scss'],
  standalone: false
})
export class ImageSlider implements OnInit {
  images: Image[] = []; 
  currentIndex = 0;
  loading = true;
  error: string | null = null;

  constructor(private imageService: ImageService) { }

  ngOnInit(): void {
    console.log('🎬 Инициализация слайдера');
    this.loadSliderImages();
  }

  handleImageError(image: Image, event: Event): void {
    console.error('❌ Ошибка загрузки изображения:', image.url);

    const imageElement = event.target as HTMLImageElement;
    imageElement.style.display = 'none';

    const errorDiv = document.createElement('div');
    errorDiv.className = 'image-error';
    errorDiv.innerHTML = `
      <p>❌ Не удалось загрузить изображение</p>
      <p><small>${image.url}</small></p>
    `;

    imageElement.parentNode?.insertBefore(errorDiv, imageElement.nextSibling);
  }

  loadSliderImages(): void {
    console.log('🔄 Загрузка изображений для слайдера...');
    this.loading = true;
    this.error = null;

    this.imageService.getSliderImages().subscribe({
      next: (images) => {
        console.log('✅ Получены изображения:', images);

        if (images && images.length > 0) {
          this.images = images;
          console.log(`📦 Загружено ${images.length} изображений`);
        } else {
          this.error = 'Нет доступных изображений';
          console.warn('⚠️ Нет изображений для отображения');
        }

        this.loading = false;

        if (this.images.length > 0) {
          this.startAutoSlide();
        }
      },
      error: (err) => {
        console.error('❌ Фатальная ошибка загрузки:', err);
        this.error = 'Не удалось подключиться к серверу';
        this.loading = false;

        this.images = [{
          id: 'fallback',
          url: 'https://localhost:7176/images/957f36b90eecfe955ff5342440e34b1d.jpg'
        }];
      }
    });
  }

  startAutoSlide(): void {
    if (this.images.length > 1) {
      console.log('⏰ Автопрокрутка включена');
      setInterval(() => {
        this.nextSlide();
      }, 5000);
    }
  }

  nextSlide(): void {
    this.currentIndex = (this.currentIndex + 1) % this.images.length;
  }

  prevSlide(): void {
    this.currentIndex = (this.currentIndex - 1 + this.images.length) % this.images.length;
  }

  goToSlide(index: number): void {
    this.currentIndex = index;
  }
}
