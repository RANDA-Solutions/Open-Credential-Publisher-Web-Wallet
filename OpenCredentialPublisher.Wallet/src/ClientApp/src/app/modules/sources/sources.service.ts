import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { AuthorizationVM } from '@shared/models/authorization';
import { ModelError } from '@shared/models/modelError';
import { SourceVM } from '@shared/models/source';
import { SourceCallback } from '@shared/models/sourceCallback';
import { SourceConnectInput } from '@shared/models/sourceConnectInput';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SourcesService {

  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  connect(input: SourceConnectInput): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Register`;
    console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
      );
  }
  deleteConnection(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/connection/Delete/${id}`;
    console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  deleteSource(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Delete/${id}`;
    console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  refreshClrs(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Refresh/${id}`;
    console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  postCallback(data: SourceCallback): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Callback`;
    console.log(`sources service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, data)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getAuthorizationDetail(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Detail/${id}`;
    console.log(`sources service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getAuthorizationList(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Authorizations`;
    console.log(`sources service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getSourceList(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}sources/Sources`;
    console.log(`sources service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
}
