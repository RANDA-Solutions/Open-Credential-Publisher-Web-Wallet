import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClrService {
  private debug = false;

  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  getAssertionResultsVM(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Assertions/Results/${clrId}?id=${assertionId}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getAssertionEndorsementVMList(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Assertions/Endorsements/${clrId}?id=${assertionId}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getAssertionEvidenceVMList(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Assertions/Evidence/${clrId}?id=${assertionId}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getAchievementAlignmentVMList(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Achievements/Alignments/${clrId}?id=${assertionId}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getAssertionAchievementAlignmentVMList(clrId: number, assertionId: number, id: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Assertions/Achievements/Alignments/${clrId}?assertionId=${assertionId}&id=${id}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getAchievementAssociationVMList(clrId: number, assertionId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Achievements/Associations/${clrId}?id=${assertionId}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getAchievementEndorsementVMList(clrId: number, achievementId: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Clrs/Achievements/Endorsements/${clrId}?id=${achievementId}`;
    if (this.debug) console.log(`ClrService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
}
