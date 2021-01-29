import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { CodeFlowModel } from '../models/code-flow.model';
import { Result } from '../modules/directives/models/result';

@Injectable({
  providedIn: 'root'
})
export class CodeFlowService {
  private path = 'api/codeflow/';
  private codeFlowModel: CodeFlowModel|null;
  private codeFlowSubject: BehaviorSubject<CodeFlowModel> = new BehaviorSubject<CodeFlowModel>(null);
  public codeFlowModel$ = this.codeFlowSubject.asObservable();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
      
  }

  get() {
      return this.http.get(this.baseUrl + this.path).pipe(tap((model: Result<CodeFlowModel>) => {
        if (model.success) {
          console.log(model.value);
          this.codeFlowModel = model.value;
          this.codeFlowSubject.next(this.codeFlowModel);
        }
      }));
  }
}
