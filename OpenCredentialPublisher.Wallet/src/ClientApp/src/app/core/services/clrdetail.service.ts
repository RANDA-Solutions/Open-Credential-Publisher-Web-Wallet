import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResult } from '@shared/models/apiOkResponse';
import { ApiResponse } from '@shared/models/apiResponse';
import { AssertionHeaderVM } from '@shared/models/assertionHeaderVM';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClrDetailService {
  private debug = false;
  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  getAssociationVM(clrId: number, targetId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}clrDetail/${clrId}/Association/${targetId}`;
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getClrAchievement(clrId: number, achievementId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/Achievement/${clrId}?achievementId=${encodeURIComponent(achievementId)}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getClrAssertion(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/ClrAssertion/${clrId}?assertionId=${assertionId}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getClrVerification(clrId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/Verification/${clrId}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getAchievementIssuer(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/AchievementIssuer/${clrId}?assertionId=${assertionId}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getLearner(clrId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/Learner/${clrId}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getParentAssertions(clrId: number, isShare: boolean): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/ParentAssertions/${clrId}?isShare=${isShare}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getChildAssertions(clrId: number, assertionId: number, isShare: boolean): Observable<ApiOkResult<Array<AssertionHeaderVM>>> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/ChildAssertions/${clrId}?assertionId=${assertionId}&isShare=${isShare}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiOkResult<Array<AssertionHeaderVM>>>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleObjectError(err)
        )
      );
  }
  getPublisher(clrId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}ClrDetail/Publisher/${clrId}`;
    if (this.debug) console.log(`ClrDetailService service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
}
