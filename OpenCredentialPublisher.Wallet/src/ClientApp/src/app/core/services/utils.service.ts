import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiResponse } from '@shared/models/apiResponse';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { Observable, of } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { LogService } from '../error-handling/logerror.service';
import { AuthorizationService } from './authorization.service';
import { ErrorService } from './error.service';
import jwt_decode from 'jwt-decode';
// https://angular.io/tutorial/toh-pt6#handleerror
/**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
@Injectable()
export class UtilsService implements OnDestroy {
  errorId = 'N/A';
  private debug = false;
  constructor(public logService: LogService, public authService: AuthorizationService, public messageService: MessageService,
               public errorService: ErrorService) { }

  ngOnDestroy(): void {
  }
  handleError(error: Error | HttpErrorResponse): Observable<ApiResponse> {
    if (error instanceof HttpErrorResponse) {
      if (this.debug) console.log(`UtilsService.handleError HttpErrorResponse`);
        // Server Error
        this.logService.logNgError(`Http error occurred: ${this.errorService.getServerMessage(error)}`)
          .pipe(take(1))
          .subscribe(
            data => {
              this.errorId = data;
              this.showError(error.message, this.errorId);
            },
            logError => {
              if (error.status === 400) { // check the original error, logError is the logging error
                this.showError(`An error occurred and logging the error failed.(${this.errorService.getServerMessage(error)})`);
              } else { // 404
                this.showError(`An error occurred and logging the error failed.(${this.errorService.getServerMessage(error)})`);
              }
            }
          );
        // Let the app keep running by returning an empty result.
        return of(new ApiBadRequestResponse(error.status, error.message, []));
    } else {
      if (this.debug) console.log(`UtilsService.handleError NOT HttpErrorResponse`);
        this.showError(this.errorService.getClientMessage(error));
        // Let the app keep running by returning an empty result.
        return of(new ApiBadRequestResponse(400, 'Unhandled client error.', []));
      }
  }
  handleErrorNoLog<T>(error: Error | HttpErrorResponse, result?: T): Observable<T> {
      if (error instanceof HttpErrorResponse) {
        if (error.status === 401) {
          this.authService.refresh();
        } else {
          this.showError(this.errorService.getServerMessage(error));
          // Let the app keep running by returning an empty result.
          return of(result as T);
        }
      } else {
        this.showError(this.errorService.getClientMessage(error));
        // Let the app keep running by returning an empty result.
        return of(result as T);
      }
  }

  showError(message: string, id?: string) {
    let header: string;
    if (id) {
      header = `ErrorId: ${id}`;
    } else {
      header = `Unlogged error`;
    }
    this.messageService.add({
      key: 'main', severity: 'error', summary: header
      , detail: message.substring(0, 255), life: environment.errorMessageLife
    });
  }

  showAlert(message: string) {
    this.messageService.add({
      key: 'main', severity: 'info', summary: 'Alert Message'
      , detail: message
    });
  }

  showSuccess(message: string) {
    this.messageService.add({
      key: 'main', severity: 'success', summary: 'Success Message'
      , detail: message
    });
  }

  showFailure(message: string) {
    this.messageService.add({
      key: 'main', severity: 'inferroro', summary: 'Failure Message'
      , detail: message, life: environment.errorMessageLife
    });
  }



  trimWhiteSpacesFromObjProperties<T>(obj: T) {
    const keys = Object.keys(obj);
    const propertyNames = Object.getOwnPropertyNames(obj);
    if (keys.length > 0) {
      for (const prop of propertyNames) {
        if (typeof obj[prop] === 'string') {
          obj[prop] = obj[prop].trim();
        }
      }
      return obj;
    } else {
      return obj;
    }
  }

  sortingDataAccessor(rawData, header, numericColumns: string) {
    return numericColumns.includes(header) ? rawData[header] ? Number(rawData[header]) : 0
    : rawData[header] ? rawData[header].toString().toLowerCase() : '';
  }
  formatDateShort(val: Date) {
    if (val) {
      return moment.utc(val).local().format('MM/DD/YY');
    } else {
      return '';
    }
  }
  formatDate(val: Date) {
    if (val) {
      return moment.utc(val).local().format('MM/DD/YYYY h:mm:ss A');
    } else {
      return '';
    }
  }
  formatInt(val: number, digits: number) {
    if (val) {
      return ('00000000000000000000' + val.toString()).slice(-digits)
    } else {
      return '';
    }
  }
  safeId(text: string) {
    let x = text.replace(new RegExp(/\:/g), '');
    x = x.replace(new RegExp(/\//g), '');
    x = x.replace(new RegExp(/\./g), '');
    return x;
  }
  deserializePayload(value: string): string{
    var decoded = JSON.stringify(jwt_decode(value));
    return decoded;
  }
}
