import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { ClrLinkVM } from '@shared/models/clrLinkVM';
import { LinkDisplayVM } from '@shared/models/linkDisplayVM';
import { LinkShareVM } from '@shared/models/linkShareVM';
import { RecipientModel } from '@shared/models/recipientModel';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LinksService {
  private debug = false;

  constructor(private http: HttpClient, private utilsService: UtilsService) { }
  create(input: ClrLinkVM): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Links/ClrLink`;
    console.log(`links service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
      );
  }
  createRecipient(input: RecipientModel): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Recipients/Create`;
    console.log(`links service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
      );
  }
  deleteLink(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Links/Delete/${id}`;
    console.log(`links service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getClrLinks(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Links/ClrsLinkList`;
    console.log(`links service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getLinks(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Links/LinkList`;
    console.log(`links service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }

  getShareVM(id: string): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Links/ShareVM/${id}`;
    console.log(`links service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getLinkDisplay(id: string, accessKey: string): Observable<ApiResponse> {
    let urlApi = `${environment.apiEndPoint}Public/Links/Display/${id}`;
    if (accessKey != ''){
      urlApi += `?accessKey=${accessKey}`;
    }
    console.log(`links service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  getLinkDisplayDetail(link: LinkDisplayVM): Observable<ApiResponse> {

    let urlApi = `${environment.apiEndPoint}Public/Links/DisplayDetail/${link.id}`;
    if (link.accessKey != '') {
      urlApi = `${environment.apiEndPoint}Public/Links/DisplayDetail/${link.id}?key=${encodeURIComponent(link.accessKey)}`;
    }
    console.log(`links service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }

  linkAccess(id: string, accessKey: string): Observable<ApiResponse> {

    let urlApi = `${environment.apiEndPoint}Public/Links/Access/${id}`;
    if (accessKey != ''){
      urlApi += `?accessKey=${encodeURIComponent(accessKey)}`;
    }
    if (this.debug) console.log(`links service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, accessKey)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
      );
  }
  shareLink(vm: LinkShareVM): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}Links/Share`;
    console.log(`links service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, vm)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
}
