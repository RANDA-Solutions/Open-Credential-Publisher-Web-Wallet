import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { VerifyVM } from '@shared/models/verifyVM';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class VerificationService {
  private debug = false;

  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  verify(vm: VerifyVM): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Verification/Verify`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, vm)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  verifyVC(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Verification/VerifyVC/${id}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
}
