import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { jsPDF } from 'jspdf';

interface QuizOption {
  label: string;
  value: string;
}

interface QuizQuestion {
  question: string;
  imageUrl?: string;
  imageAltText?: string;
  options: QuizOption[];
  correctAnswer: string;
}

interface TestRecord {
  studentName: string;
  subject: string;
  className: string;
  score: number;
  total: number;
  date: string;
}

interface QuizDraft {
  studentName: string;
  board: string;
  className: string;
  subject: string;
  questions: QuizQuestion[];
  currentIndex: number;
  score: number;
  selectedAnswers: { [key: number]: string };
}

@Component({
  selector: 'app-quiz',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './quiz.component.html',
  styleUrl: './quiz.component.scss'
})
export class QuizComponent {
  selectedSubject = 'Mathematics';
  selectedClass = 'Class 1';
  selectedBoard = 'CBSE';
  studentName = '';

  questions: QuizQuestion[] = [
    {
      question: 'Which number is the smallest prime number?',
      imageUrl: 'https://images.unsplash.com/photo-1503676260728-1c00da094a0b?auto=format&fit=crop&w=800&q=80',
      options: [
        { label: 'A', value: '1' },
        { label: 'B', value: '2' },
        { label: 'C', value: '3' },
        { label: 'D', value: '4' }
      ],
      correctAnswer: '2'
    },
    {
      question: 'What is 7 × 8?',
      imageUrl: 'https://images.unsplash.com/photo-1513258496099-48168024aec0?auto=format&fit=crop&w=800&q=80',
      options: [
        { label: 'A', value: '54' },
        { label: 'B', value: '56' },
        { label: 'C', value: '58' },
        { label: 'D', value: '60' }
      ],
      correctAnswer: '56'
    },
    {
      question: 'Which shape has 3 sides?',
      imageUrl: 'https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=800&q=80',
      options: [
        { label: 'A', value: 'Square' },
        { label: 'B', value: 'Triangle' },
        { label: 'C', value: 'Circle' },
        { label: 'D', value: 'Rectangle' }
      ],
      correctAnswer: 'Triangle'
    }
  ];

  currentIndex = 0;
  score = 0;
  isLoading = true;
  isComplete = false;
  certificateDownloaded = false;
  certificateNumber = '';
  selectedAnswers: { [key: number]: string } = {};
  imageLoaded = false;
  private readonly userEmail = localStorage.getItem('smartquiz.userEmail') || '';
  private readonly quizDraftKey = `smartquiz.quizDraft.${encodeURIComponent(this.userEmail.toLowerCase())}`;
  private readonly testRecordsKey = `smartquiz.testRecords.${encodeURIComponent(this.userEmail.toLowerCase())}`;

  constructor(private route: ActivatedRoute, private http: HttpClient) {
    this.route.queryParams.subscribe(params => {
      this.selectedBoard = params['board'] || 'CBSE';
      this.selectedClass = params['class'] || 'Class 1';
      this.selectedSubject = params['subject'] || 'Mathematics';
      this.studentName = params['student'] || localStorage.getItem('smartquiz.studentName') || 'Student';
      this.loadQuiz();
    });
  }

  private loadQuiz(): void {
    if (this.restoreDraft()) {
      this.isLoading = false;
      this.imageLoaded = false;
      return;
    }

    this.isLoading = true;
    this.http.post<{ questions: Array<{ question: string; optionA: string; optionB: string; optionC: string; optionD: string; correctAnswer: string; imageUrl?: string; imageAltText?: string }> }>('http://localhost:5214/api/Quiz/start', {
      studentName: this.studentName,
      board: this.selectedBoard,
      className: this.selectedClass,
      subject: this.selectedSubject,
      questionCount: 20
    }, {
      headers: new HttpHeaders({ Authorization: `Bearer ${localStorage.getItem('smartquiz.token') || ''}` })
    }).subscribe({
      next: response => {
        if (response.questions?.length === 20) {
          this.questions = response.questions.map(item => ({
            question: item.question,
            imageUrl: item.imageUrl,
            imageAltText: item.imageAltText,
            options: [
              { label: 'A', value: item.optionA },
              { label: 'B', value: item.optionB },
              { label: 'C', value: item.optionC },
              { label: 'D', value: item.optionD }
            ],
            correctAnswer: item.correctAnswer
          }));
        }
        this.isLoading = false;
        this.imageLoaded = false;
      },
      error: error => {
        this.isLoading = false;
        if (error.status === 401) {
          localStorage.removeItem('smartquiz.token');
          window.location.href = '/auth';
        }
      }
    });
  }

  get currentQuestion(): QuizQuestion {
    return this.questions[this.currentIndex];
  }

  get progressPercent(): number {
    return ((this.currentIndex + 1) / this.questions.length) * 100;
  }

  private restoreDraft(): boolean {
    const rawDraft = localStorage.getItem(this.quizDraftKey);
    if (!rawDraft) {
      return false;
    }

    try {
      const draft = JSON.parse(rawDraft) as QuizDraft;
      const matchesQuiz = draft.studentName === this.studentName && draft.board === this.selectedBoard && draft.className === this.selectedClass && draft.subject === this.selectedSubject && draft.questions?.length === 20;
      if (!matchesQuiz) {
        return false;
      }

      this.questions = draft.questions;
      this.currentIndex = draft.currentIndex;
      this.score = draft.score;
      this.selectedAnswers = draft.selectedAnswers || {};
      return true;
    } catch {
      localStorage.removeItem(this.quizDraftKey);
      return false;
    }
  }

  private saveDraft(): void {
    localStorage.setItem(this.quizDraftKey, JSON.stringify({
      studentName: this.studentName,
      board: this.selectedBoard,
      className: this.selectedClass,
      subject: this.selectedSubject,
      questions: this.questions,
      currentIndex: this.currentIndex,
      score: this.score,
      selectedAnswers: this.selectedAnswers
    } as QuizDraft));
  }

  handleImageError(event: Event): void {
    const image = event.target as HTMLImageElement;
    this.imageLoaded = true;
    image.src = 'https://images.unsplash.com/photo-1503676260728-1c00da094a0b?auto=format&fit=crop&w=800&q=80';
  }

  handleImageLoad(): void {
    this.imageLoaded = true;
  }

  goHome(): void {
    window.location.href = '/home';
  }

  chooseAnswer(value: string): void {
    this.selectedAnswers[this.currentIndex] = value;

    if (value === this.currentQuestion.correctAnswer) {
      this.score += 1;
    }

    if (this.currentIndex < this.questions.length - 1) {
      this.currentIndex += 1;
      this.imageLoaded = false;
      this.saveDraft();
      return;
    }

    this.isComplete = true;
    localStorage.removeItem(this.quizDraftKey);
    this.certificateNumber = `SQ-${Date.now().toString().slice(-8)}`;
    this.saveTestRecord();
  }

  private saveTestRecord(): void {
    const records: TestRecord[] = JSON.parse(localStorage.getItem(this.testRecordsKey) || '[]');
    records.unshift({ studentName: this.studentName, subject: this.selectedSubject, className: this.selectedClass, score: this.score, total: this.questions.length, date: new Date().toLocaleDateString() });
    localStorage.setItem(this.testRecordsKey, JSON.stringify(records.slice(0, 5)));
  }

  downloadCertificate(): void {
    if (this.certificateDownloaded) {
      return;
    }

    const pdf = new jsPDF();
    pdf.setFillColor(240, 247, 255);
    pdf.rect(0, 0, 210, 297, 'F');
    pdf.setDrawColor(91, 99, 239);
    pdf.setLineWidth(2);
    pdf.rect(12, 12, 186, 273);
    pdf.setTextColor(38, 53, 105);
    pdf.setFontSize(28);
    pdf.text('SMARTQUIZ', 105, 52, { align: 'center' });
    pdf.setFontSize(16);
    pdf.setTextColor(239, 139, 0);
    pdf.text('CERTIFICATE OF ACHIEVEMENT', 105, 68, { align: 'center' });
    pdf.setTextColor(45, 55, 90);
    pdf.setFontSize(13);
    pdf.text('This certificate is proudly presented to', 105, 100, { align: 'center' });
    pdf.setFontSize(25);
    pdf.setTextColor(49, 85, 231);
    pdf.text(this.studentName, 105, 120, { align: 'center' });
    pdf.setTextColor(45, 55, 90);
    pdf.setFontSize(13);
    pdf.text(`for completing the ${this.selectedSubject} quiz`, 105, 142, { align: 'center' });
    pdf.text(`${this.selectedBoard} ${this.selectedClass}`, 105, 154, { align: 'center' });
    pdf.setFontSize(18);
    pdf.text(`Score: ${this.score}/${this.questions.length}`, 105, 188, { align: 'center' });
    pdf.setFontSize(11);
    pdf.text(`Certificate number: ${this.certificateNumber}`, 105, 224, { align: 'center' });
    pdf.text(`Issued: ${new Date().toLocaleDateString()}`, 105, 234, { align: 'center' });
    pdf.save(`${this.studentName}-${this.selectedSubject}-certificate.pdf`);
    this.certificateDownloaded = true;
  }
}
