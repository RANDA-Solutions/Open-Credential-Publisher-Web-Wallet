import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { ConnectionViewModel } from '@shared/models/connectionViewModel';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WalletService {
  private debug = false;


  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  connect() {
    const urlApi = `${environment.apiEndPoint}Wallets/Connect`;
    if (this.debug) console.log(`WalletService connect ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err)
      )
    );
  }
  getWallets(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/walletlist`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getConnectionVM(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/Connection/${id}`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getInvitationVM(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/Invitation/${id}`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getSendWalletVM(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/WalletSendVM/${id}`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getRelationshipVM(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/Relationship/${id}`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  delete(id: number): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/Delete/${id}`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService
        .handleError(err))
    );
  }
  saveConnection(id: number, input: ConnectionViewModel): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/saveConnection/${id}`;
    if (this.debug) console.log(`WalletService ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
  }
  send(id: number, pkgId: number) {
    const urlApi = `${environment.apiEndPoint}Wallets/${id}/Send/${pkgId}`;
    if (this.debug) console.log(`WalletService send ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
    .pipe(
      catchError(err => this.utilsService.handleError(err)
      )
    );
  }
}
