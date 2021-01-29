import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { CandidateModel } from "../models/candidate.model";
import { CandidatesModel } from "../models/candidates.model";
import { Result } from "../modules/directives/models/result";

@Injectable({
    providedIn: 'root'
})
export class CandidateService {
    private path = 'api/candidate';

    private model: CandidatesModel|null;
    private modelSubject = new BehaviorSubject<CandidatesModel>(null);
    public model$ = this.modelSubject.asObservable();

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        
    }

    get(): Observable<CandidatesModel> {
        return this.http.get(this.baseUrl + this.path).pipe(tap((model: CandidatesModel) => {
            this.model = model;
            this.modelSubject.next(this.model);
        }));
    }

    add(candidate: any) {
        return this.http.post(this.baseUrl + this.path, candidate).pipe(tap((result: Result<CandidateModel>) => {
            if (result.success) {
                if (this.model == null)
                    this.model = new CandidatesModel();
                this.model.candidates.push(result.value);
                this.modelSubject.next(this.model);
            }
        }));
    }

    delete(candidateId: string) {
        return this.http.delete(`${this.baseUrl}${this.path}/delete/${candidateId}`).pipe(tap((result: Result<CandidateModel>) => {
            if (result.success) {
                if (result.value.isDeleted) {
                    var x = this.model.candidates.findIndex(x => x.candidateId === candidateId);

                    this.model.candidates.splice(x, 1);
                    this.modelSubject.next(this.model);
                }
                else {console.log('delete failed.');}
            }
            
        }));
    }

}