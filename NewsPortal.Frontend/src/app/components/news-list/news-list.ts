//import { Component, OnInit } from '@angular/core';
//import { NewsService, News } from '../../services/news';

//@Component({
//  selector: 'app-news-list',
//  templateUrl: './news-list.html',
//  styleUrls: ['./news-list.scss'],
//  standalone: false
//})
//export class NewsList implements OnInit {
//  news: News[] = [];
//  loading = true;

//  constructor(private newsService: NewsService) { }

//  ngOnInit(): void {
//    this.loadNews();
//  }

//  loadNews(): void {
//    this.newsService.getNews().subscribe({
//      next: (news) => {
//        this.news = news;
//        this.loading = false;
//      },
//      error: (error) => {
//        console.error('Error loading news:', error);
//        this.loading = false;
//      }
//    });
//  }
//}
