import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', loadComponent: () => import('./home/home.component').then(m => m.HomeComponent) },
  { path: 'quiz', canActivate: [authGuard], loadComponent: () => import('./quiz/quiz.component').then(m => m.QuizComponent) },
  { path: 'auth', loadComponent: () => import('./auth/auth.component').then(m => m.AuthComponent) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
