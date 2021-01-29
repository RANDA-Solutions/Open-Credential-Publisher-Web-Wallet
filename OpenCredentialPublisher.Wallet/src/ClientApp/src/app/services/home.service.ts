import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CodeFlowModel } from '../models/code-flow.model';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { HomeModel } from '../models/home.model';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private path = 'api/home/';
  private homeModel: HomeModel;
  private homeModelSubject: BehaviorSubject<HomeModel> = new BehaviorSubject<HomeModel>(null);
  public homeModel$ = this.homeModelSubject.asObservable();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
      
  }

  get() {
      return this.http.get(this.baseUrl + this.path).pipe(tap((model: HomeModel) => {
        this.homeModel = model;
        this.homeModelSubject.next(this.homeModel);
      }));
  }
}