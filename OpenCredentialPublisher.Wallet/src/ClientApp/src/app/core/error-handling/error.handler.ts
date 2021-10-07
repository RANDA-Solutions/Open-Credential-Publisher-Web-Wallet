import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import { ErrorService } from '@core/services/error.service';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { MessageService } from 'primeng/api';
import { take } from 'rxjs/operators';
import { LogService } from './logerror.service';

@Injectable()
export class GlobalErrorHandler extends ErrorHandler {
  private errorId: string;
  private debug = false;

  constructor(private logService: LogService, public messageService: MessageService, public errorService: ErrorService) {
    super();
  }

  handleError(error: Error | HttpErrorResponse) {
    /* eslint-disable no-console */
    if (error instanceof HttpErrorResponse) {
      if (this.debug) console.log(`GlobalErrorHandler.handleError HttpErrorResponse: ${JSON.stringify(error)}`);
      console.error(error);
      this.logService.logNgError(`${this.errorService.getServerMessage(error)} from Server`)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(`GlobalErrorHandler.handleError logNgError returned:${JSON.stringify(data)}`);
        if (data.statusCode == 200) {
          this.messageService.add({
            key: 'main', severity: 'error', summary: 'Unhandled Error'
            , detail: `ErrorId: ${(<ApiOkResponse>data).result}\n\n ${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`, sticky: false
          });
        } else {
          this.messageService.add({
            key: 'main', severity: 'error', summary: 'Unable to log Unhandled Error'
            , detail: `An error occurred trying to log an Http Error to the server.\n\n ${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`, sticky: false
          });
        }
      });
    } else {
      if (this.debug) console.log(`GlobalErrorHandler.handleError Error: ${JSON.stringify(error)}`);
      console.error(error);
      this.logService.logNgError(`${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`)
        .pipe(take(1)).subscribe(data => {
          if (this.debug) console.log(`GlobalErrorHandler.handleError logNgError returned:${JSON.stringify(data)}`);
          if (data.statusCode == 200) {
            this.messageService.add({
              key: 'main', severity: 'error', summary: 'Unhandled Error'
              , detail: `ErrorId: ${(<ApiOkResponse>data).result}\n\n ${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`, sticky: false
            });
          } else {
            this.messageService.add({
              key: 'main', severity: 'error', summary: 'Unable to log Unhandled Error'
              , detail: `An error occurred trying to log an Angular Error to the server.\n\n ${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`, sticky: false
            });
          }
        });
    }
  }
}
