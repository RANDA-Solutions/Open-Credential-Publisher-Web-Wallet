import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { SmartResumePostModel } from '@shared/models/SmartResumePostModel';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SmartResumeService {
  private debug = false;

  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  submit(post: SmartResumePostModel): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}SmartResume/`;
    if (this.debug) console.log(`smart resume service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, post)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
}
