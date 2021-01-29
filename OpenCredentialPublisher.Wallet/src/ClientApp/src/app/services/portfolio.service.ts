import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PortfolioViewModel } from '../models/portfolio-view.model';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PortfolioService {
  private path = 'api/portfolio/';
  private portfolioViewModel: PortfolioViewModel;
  private portfolioSubject: BehaviorSubject<PortfolioViewModel> = new BehaviorSubject<PortfolioViewModel>(null);
  public portfolioViewModel$ = this.portfolioSubject.asObservable();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
      
  }

  get() {
      return this.http.get(this.baseUrl + this.path).pipe(tap((model: PortfolioViewModel) => {
        this.portfolioViewModel = model;
        this.portfolioSubject.next(this.portfolioViewModel);
      }));
  }
}
