import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { tap } from "rxjs/operators";
import { Result } from "../../directives/models/result";

@Injectable({
    providedIn: 'root'
})
export class PayflowService {
    private path = 'api/payflow';
    public requestId: string;
    public credits: number;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    start(form: any) {
        return this.http.post(this.baseUrl + this.path, form).pipe(tap((result: Result<any>) => {
            if (result.success) {
                this.requestId = result.value;
                this.credits = form.credits;
            }
        }));
    }

    complete() {
        return this.http.post(this.baseUrl + this.path + "/complete", { requestId: this.requestId, actualCredits: this.credits });
    }
}