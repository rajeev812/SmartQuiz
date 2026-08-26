import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  studentName = localStorage.getItem('smartquiz.studentName') || '';
  private readonly userEmail = localStorage.getItem('smartquiz.userEmail') || '';
  private readonly testRecordsKey = `smartquiz.testRecords.${encodeURIComponent(this.userEmail.toLowerCase())}`;
  testRecords: Array<{ subject: string; className: string; score: number; total: number; date: string }> = JSON.parse(localStorage.getItem(this.testRecordsKey) || '[]');
  board = 'CBSE';
  className = 'Class 1';
  subject = 'Mathematics';

  classes = Array.from({ length: 10 }, (_, index) => `Class ${index + 1}`);
  subjects = ['Mathematics', 'Science', 'English', 'Social Studies', 'Computer', 'General Knowledge'];

  constructor(private router: Router) {}

  startQuiz(): void {
    if (!localStorage.getItem('smartquiz.token')) {
      this.router.navigate(['/auth']);
      return;
    }
    this.studentName = this.studentName.trim();
    if (!this.studentName) {
      return;
    }
    localStorage.setItem('smartquiz.studentName', this.studentName);
    this.router.navigate(['/quiz'], {
      queryParams: {
        student: this.studentName,
        board: this.board,
        class: this.className.trim() || 'Class 1',
        subject: this.subject
      }
    });
  }

  logout(): void {
    localStorage.removeItem('smartquiz.token');
    localStorage.removeItem('smartquiz.studentName');
    localStorage.removeItem('smartquiz.userEmail');
    this.router.navigate(['/auth']);
  }

}
