import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Admin } from './pages/admin/admin';
import { NewsDetail } from './pages/news-detail/news-detail';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  { path: '', component: Home },
  {
    path: 'admin',
    component: Admin,
    canActivate: [AuthGuard]
  },
  {
    path: 'news/:id',
    component: NewsDetail
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
