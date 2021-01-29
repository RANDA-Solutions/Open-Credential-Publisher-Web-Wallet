import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { VerificationPostModel } from '../models/verification-post.model';
import { VerificationResultModel } from '../models/verification-result.model';
import { VerificationModel } from '../models/verification.model';

@Injectable({
  providedIn: 'root'
})
export class VerificationService {
  private path = 'api/verification/';
  private verificationModel: VerificationModel;
  private verificationSubject: BehaviorSubject<VerificationModel> = new BehaviorSubject<VerificationModel>(null);
  public verificationModel$ = this.verificationSubject.asObservable();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
      
  }

  get(id: string) {
      return this.http.get(this.baseUrl + this.path + id).pipe(tap((model: VerificationModel) => {
        this.verificationModel = model;
        this.verificationSubject.next(this.verificationModel);
      }));
  }

  post(postModel: VerificationPostModel) : Observable<VerificationResultModel> {
    return this.http.post(this.baseUrl + this.path, postModel).pipe(tap((result: VerificationResultModel) => {
      
    }));
  }
}
