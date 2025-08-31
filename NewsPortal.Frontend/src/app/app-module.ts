import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Header } from './components/header/header';
import { Home } from './pages/home/home';
import { ImageSlider } from './components/image-slider/image-slider';
import { Admin } from './pages/admin/admin';
import { NewsDetail } from './pages/news-detail/news-detail';
import { AuthService } from './services/auth';
import { AuthGuard } from './services/auth.guard';
import { ImageService } from './services/image';
import { CategoryService } from './services/category';
import { NewsService } from './services/news';
import { TokenInterceptorService } from './services/token-interceptor';

@NgModule({
    declarations: [
        App,
        Header,
        Home,
        ImageSlider,
        Admin,
        NewsDetail
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        AppRoutingModule
    ],
    providers: [
        AuthService,
        AuthGuard,
        ImageService,
        CategoryService,
        NewsService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptorService,
            multi: true
        }
    ],
    bootstrap: [App]
})
export class AppModule { }
