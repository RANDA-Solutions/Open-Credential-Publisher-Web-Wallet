import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiResponse } from '@shared/models/apiResponse';
import { ChangePasswordVM } from '@shared/models/changePasswordVM';
import { RegisterAccountVM } from '@shared/models/registerAccountVM';
import { TwoFactorAuthenticationModelInput } from '@shared/models/TwoFactorAuthModelInput';
import { VerifyEmailVM } from '@shared/models/verifyEmailVM';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private debug = false;


  constructor(private http: HttpClient, private utilsService: UtilsService) { }

  confirmEmailAccount(userId: string, code: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/confirmEmail/${userId}?code=${code}`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  confirmEmailChange(input: VerifyEmailVM): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}email/ConfirmEmailChange`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  getProfile(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/getProfile`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  saveProfile(args: any): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/saveProfile`;
    console.log(`links service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, args)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
   );

  }

  getEmail(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}email/getEmail`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  changeEmail(data: VerifyEmailVM): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}Public/Account/ChangeEmail`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, data)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
   );

  }
  saveEmail(args: any): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}email/saveEmail`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, args)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
   );

  }
  sendVerificationEmail(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}email/verificationEmail`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  getProfileData() {
    const urlApi = `${environment.apiEndPoint}Downloads/UserJson`;
      if (this.debug) console.log(`DownloadService service ${urlApi}`);
      return this.http.post(urlApi, null, {responseType: 'json', observe: 'response'});
  }

  getProfileImage(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/getProfileImage`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }

  registerAccount(input: RegisterAccountVM): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/register`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
   );

  }
  changePassword(data: ChangePasswordVM): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/changePassword`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, data)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
   );

  }

  saveProfileImage(args: any): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/saveProfileImage`;
    console.log(`links service ${urlApi}`);
    console.log(args)
    return this.http.post<ApiResponse>(urlApi, args)
      .pipe(
        catchError(err => this.utilsService.handleError(err))
   );

  }

  getTwoFAVM(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}account/getTwoFAVM`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
    }

  getTwoFAKeyNCode(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}TwoFactorAuthentication/KeyAndQRCode`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  generateRecoveryCodes(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}TwoFactorAuthentication/GenerateRecoveryCodes`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
  login2FA(input: TwoFactorAuthenticationModelInput): Observable<any> {
    const urlApi = `${environment.publicEndPoint}account/Login/Login2FA`;
    console.log(`account service ${urlApi}`);
    return this.http.post<any>(urlApi, input)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
    }

    loginRecovery(input: TwoFactorAuthenticationModelInput): Observable<any> {
      const urlApi = `${environment.publicEndPoint}account/Login/LoginRecovery`;
      console.log(`account service ${urlApi}`);
      return this.http.post<any>(urlApi, input)
        .pipe(
          catchError(err => this.utilsService.handleError(err)
          )
        );
      }
  sendCode(code: string): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}TwoFactorAuthentication/SendCode/${encodeURIComponent(code)}`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
    }
  resetAuthenticator(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}TwoFactorAuthentication/ResetAuthenticator`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
    }
  disable2FA(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}TwoFactorAuthentication/Disable`;
    console.log(`account service ${urlApi}`);
    return this.http.post<ApiResponse>(urlApi, null)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
    }

  getWallets(): Observable<ApiResponse> {
    const urlApi = `${environment.apiEndPoint}wallets/walletlist`;
    console.log(`account service ${urlApi}`);
    return this.http.get<ApiResponse>(urlApi)
      .pipe(
        catchError(err => this.utilsService.handleError(err)
        )
      );
  }
}
