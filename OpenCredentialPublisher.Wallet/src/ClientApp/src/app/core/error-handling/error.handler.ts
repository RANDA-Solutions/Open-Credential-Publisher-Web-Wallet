import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { ErrorService } from '@core/services/error.service';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { MessageService } from 'primeng/api';
import { LogService } from './logerror.service';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler extends ErrorHandler {
  private errorId: string;
  private debug = false;

  constructor(private logService: LogService, public messageService: MessageService, public errorService: ErrorService, private zone: NgZone) {
    super();
  }

  handleError(error: Error | HttpErrorResponse) {
    super.handleError(error);
    /* eslint-disable no-console */
    if (error instanceof HttpErrorResponse) {
      if (this.debug) console.log(`GlobalErrorHandler.handleError HttpErrorResponse: ${JSON.stringify(error)}`);
      this.logService.logNgError(`${this.errorService.getServerMessage(error)} from Server`).subscribe(data => {
        if (this.debug) console.log(`GlobalErrorHandler.handleError logNgError returned:${JSON.stringify(data)}`);
        if (data.statusCode == 200) {
          this.zone.run(() => {
            this.messageService.add({
              key: 'main', severity: 'error', summary: 'Unhandled Error'
              , detail: `ErrorId: ${(<ApiOkResponse>data).result}\n\n ${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`, sticky: false
            });
          }, this);
        }
      });
    } else {
      if (this.debug) console.log(`GlobalErrorHandler.handleError Error: ${JSON.stringify(error)}`);
      this.logService.logNgError(`${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`)
        .subscribe(data => {
          if (this.debug) console.log(`GlobalErrorHandler.handleError logNgError returned:${JSON.stringify(data)}`);
          if (data.statusCode == 200) {
            this.zone.run(() => {
              this.messageService.add({
                key: 'main', severity: 'error', summary: 'Unhandled Error'
                , detail: `ErrorId: ${(<ApiOkResponse>data).result}\n\n ${this.errorService.getClientMessage(error)} at ${this.errorService.getClientStack(error)}`, sticky: false
              });
            }, this);
          }
        });
    }
  }
}
