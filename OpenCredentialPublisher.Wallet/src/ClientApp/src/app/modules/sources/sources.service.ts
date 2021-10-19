import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { SourceCallback } from '@shared/models/sourceCallback';
import { SourceConnectInput } from '@shared/models/sourceConnectInput';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SourcesService {
  private debug: false;
  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  connect(input: SourceConnectInput): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Register`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
      );
  }
  deleteConnection(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/connection/Delete/${id}`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  deleteSource(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Delete/${id}`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  refreshClrs(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Refresh/${id}`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  postCallback(data: SourceCallback): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Callback`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, data)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getAuthorizationDetail(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Detail/${id}`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getAuthorizationList(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Authorizations`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getSourceList(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Sources`;
    if (this.debug) console.log(`sources service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getBadgesForSelection(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Sources/GetBadgesForSelect/${id}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  selectBadges(id: number, ids: number[]): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}sources/SelectBadges/${id}`;
    console.log(`sources service ${JSON.stringify(ids)}`);
    return this.http.post<ApiResponse>(urlApi, ids)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
}
