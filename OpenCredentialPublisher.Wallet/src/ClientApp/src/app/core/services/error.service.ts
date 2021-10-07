import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})

export class ErrorService {
  private debug = false;
  getClientMessage(error: Error): string {
    if (this.debug) console.log(`ErrorService.getClientMessage ${error.message ? error.message : error.toString()}`);
    if (!navigator.onLine) {
      return 'No Internet Connection';
    }
    return error.message ? error.message : error.toString();
  }

  getClientStack(error: Error): string {
    if (this.debug) console.log(`ErrorService.getClientStack ${error.stack ? error.stack : error.toString()}`);
    return error.stack;
  }

  getServerMessage(error: HttpErrorResponse): string {
    if (this.debug) console.log(`ErrorService.getServerMessage ${JSON.stringify(error)}`);
    return error.message;
  }

  getServerStack(error: HttpErrorResponse): string {
    if (this.debug) console.log(`ErrorService.getServerStack ${JSON.stringify(error)}`);
    // handle stack trace
    return 'Server';
  }
}
