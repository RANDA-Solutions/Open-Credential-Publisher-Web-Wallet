
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment/environment';
import { AngularError } from '@shared/models/angularerror';
import { ApiResponse } from '@shared/models/apiResponse';
import { Observable, of } from 'rxjs';
import { catchError, map, take } from 'rxjs/operators';

@Injectable()
export class LogService {
  private debug = false;
  constructor(private http: HttpClient) {
  }

  logNgError(error: string): Observable<any> {
    const apiUrl = `${environment.apiEndPoint}ClientLog/LogAngularError`;
    const angErr = new AngularError('OpenCredentialPublisher.Wallet', error);
    if (this.debug) console.log(`LogService.logNgError url:${apiUrl} message:${JSON.stringify(angErr)}`);
    return this.http.post<ApiResponse>(apiUrl, angErr)
      .pipe(
        catchError(err => this.handleLoggingError(err, 'Angular'))
      )
      ;
  }
  logUploadError(callerInfo: string) {
    let errorId: string;
    const apiUrl = `${environment.apiEndPoint}ClientLog/LogHttpError`;
    const message = `Upload Error ${callerInfo}`;
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post<string>(apiUrl, { message }, { headers })
      .pipe(take(1), map(
        data => {
          errorId = data;
          return errorId;
        },
        err => {
          console.error(`An error occurred trying to log an Upload Error to the server.`);
        }
      ));

  }
  logHttpError(callerInfo: string, error: HttpErrorResponse) {
    let errorId: string;
    const apiUrl = `${environment.apiEndPoint}ClientLog/LogHttpError`;
    const message = `Http Error ${callerInfo} ${error.message}`;
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post<string>(apiUrl, { message }, { headers })
      .pipe(take(1), map(
        data => {
          errorId = data;
          return errorId;
        },
        err => {
          console.error(`An error occurred trying to log an Http Error to the server.`);
        }
      ));

  }
  handleLoggingError(error: Error | HttpErrorResponse, callType: string) {
    console.error(`An error occurred trying to log an ${callType} Error to the server. ${error.message}`);
    return of();
  }
}
