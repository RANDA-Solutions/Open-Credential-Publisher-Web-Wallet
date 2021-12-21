import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResult } from '@shared/models/apiOkResponse';
import { ApiResponse } from '@shared/models/apiResponse';
import { AssertionHeaderVM } from '@shared/models/assertionHeaderVM';
import { ClrAssertionVM } from '@shared/models/clrAsserrtionVM';
import { ClrCollectionVM } from '@shared/models/clrCollectionVM';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CredentialService {
  private debug = false;

  constructor(private http: HttpClient, private utilsService: UtilsService) { }
  getAssertionDetail(assertionId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/Assertions/${assertionId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getAllClrs(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/Clrs`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  create(input: ClrCollectionVM): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}credentials/clrcollection`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
      );
  }
  getClrAssertion(clrId: number, id: string): Observable<ApiOkResult<ClrAssertionVM>> {
    const urlApi = `${environment.apiEndPoint}credentials/Clr/${clrId}/AssertionDetail?assertionId=${id}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiOkResult<ClrAssertionVM>>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleObjectError(err)
        )
      );
  }
  getClrAssertions(clrId: number): Observable<ApiOkResult<Array<AssertionHeaderVM>>> {
    const urlApi = `${environment.apiEndPoint}credentials/ClrAssertions/${clrId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiOkResult<Array<AssertionHeaderVM>>>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleObjectError(err)
        )
      );
  }
  getClrViewModel(clrId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/Clrs/${clrId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getClrViewModelPlusAchievements(clrId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Public/ClrWithAchievementIds/${clrId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getClrViewModels(packageId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/PackageClrs/${packageId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getPackageVCVM(packageId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/PackageVerifiableCredential/${packageId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getPackage(packageId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/Package/${packageId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getPackageList(): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}credentials/PackageList`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getBackpackPackage(id: number): Observable<ApiResponse> {

    const urlApi = `${environment.apiEndPoint}credentials/BackpackPackage/${id}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  deletePackage(packageId: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/Package/Delete/${packageId}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  clrEmbed(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}credentials/OpenBadge/ClrEmbed/${id}`;
    if (this.debug) console.log(`credentials service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
}
