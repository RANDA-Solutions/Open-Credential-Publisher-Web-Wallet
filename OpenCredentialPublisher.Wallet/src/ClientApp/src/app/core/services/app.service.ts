import { EventEmitter, Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '@environment/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { LoginService } from '@root/app/auth/auth.service';
import { CredentialSendStatus } from '@shared/models/credentialSendStatus';
import { WalletConnectionStatus } from '@shared/models/walletConnectionStatus';
import { AuthenticatedResult } from 'angular-auth-oidc-client/lib/auth-state/auth-result';
import { of, Subscription, throwError } from 'rxjs';
import { filter, tap } from 'rxjs/operators';

@UntilDestroy()
@Injectable()
export class AppService {

  public get loggedIn(): boolean { return this._authResult?.isAuthenticated ?? false; };
  public disconnectedPage = false;
  public currentUrl = '';
  public previousUrl = '';

  public completeInvitationFlow: string = 'complete-invitation';
  public completeInvitationMethod: string = 'ConnectionStatus';
  public generateInvitationFlow: string = 'generate-invitation';
  public generateInvitationMethod: string = 'InvitationStatus';
  public proofFlow: string = 'proof';
  public proofMethod: string = 'ProofRequestStatus';
  public credentialStatusFlow: string = 'credential-status';
  public credentialStatusMethod: string = 'CredentialStatus';

  private _authResult: AuthenticatedResult;
  private _hubs: { [name: string]: HubConnection } = {};
  private debug = false;
  private routerSubscription: Subscription;

  connectionCompleted = new EventEmitter<Boolean>();

  generateInvitationStatusChanged = new EventEmitter<WalletConnectionStatus>();
  completeInvitationStatusChanged = new EventEmitter<WalletConnectionStatus>();

  credentialStatusChanged = new EventEmitter<CredentialSendStatus>();
  proofFinished = new EventEmitter<boolean>();
  proofStatusChanged = new EventEmitter<string>();

  constructor(public loginService: LoginService, private router: Router) {
    this.loginService.isLoggedIn
      .pipe(untilDestroyed(this), tap((val: AuthenticatedResult) => this._authResult = val))
      .subscribe(val => {
        if (val) {
          this._authResult = val;
        }
      });

    this.routerSubscription = this.router.events
    .pipe(filter(event => event instanceof NavigationEnd), untilDestroyed(this))
    .subscribe((event: NavigationEnd) => {
      this.previousUrl = this.currentUrl;
      this.currentUrl = event.url;
      if (this.currentUrl.includes('/Public/Links/Display')){
        this.disconnectedPage = true;
      } else {
        this.disconnectedPage = false;
      }
    });
  }



  // Notification functionality
  // https://damienbod.com/2017/10/16/securing-an-angular-signalr-client-using-jwt-tokens-with-asp-net-core-and-identityserver4/
  public connectSignalR(flow: string, args: any = null) {
    if (this.debug) console.log(`AppService connectSignalR clear statuses`);

    let hubConnection = this.buildHubConnection(flow);

    if (flow == this.generateInvitationFlow) {
      hubConnection.on(this.generateInvitationMethod, (relationshipId, status, done) => {
        if (this.debug) console.log(status);
        var statusLabel = status.replace(/([A-Z])/g, ' $1').trim();
        this.generateInvitationStatusChanged.emit(new WalletConnectionStatus(relationshipId, statusLabel, done));
      });
    }
    else if (flow == this.completeInvitationFlow) {
      if (this.debug) console.log(`AppService SignalR listening to ${this.completeInvitationMethod}`);
      hubConnection.on(this.completeInvitationMethod, (relationshipId, status, done) => {
        if (this.debug) console.log(`AppService SignalR received ${this.completeInvitationMethod} message (relationshipId: ${relationshipId}, status: ${status}, done: ${done})`);
        if (this.debug) console.log(status);
        var statusLabel = status.replace(/([A-Z])/g, ' $1').trim();
        this.completeInvitationStatusChanged.emit(new WalletConnectionStatus(relationshipId, statusLabel, done));
      });
    } else if (flow == this.credentialStatusFlow) {
      if (this.debug) console.log(`AppService SignalR listening to CredentialStatus`);
      hubConnection.on(this.credentialStatusMethod, (relationshipId, pkgId, status, done, error, revoked) => {
        if (this.debug) console.log(`AppService SignalR received CredentialStatus message (relationshipId: ${relationshipId}, status: ${status}, done: ${done})`);
        if (this.debug) console.log(status);
        var statusLabel = status.replace(/([A-Z])/g, ' $1').trim();
        this.credentialStatusChanged.emit(new CredentialSendStatus(relationshipId, pkgId, statusLabel, done, error, revoked));
      });
    }

    // Start the connection.
    this.startSignalR(flow, args);
  }

  private buildHubConnection(flow: string) : HubConnection {
    if (!this._hubs[flow]) {
      if (flow == this.proofFlow) {
        this._hubs[flow] = new HubConnectionBuilder()
          .withUrl(`${environment.hubProofStatusEndpoint}`)
          .build();
      }
      else if (flow == this.generateInvitationFlow) {
        this._hubs[flow] = new HubConnectionBuilder()
          .withUrl(environment.hubConnectionStatusEndpoint, { accessTokenFactory: () => { return of(this.loginService.token).toPromise(); } }  )
          .build();
      }
      else if (flow == this.completeInvitationFlow) {
        this._hubs[flow] = new HubConnectionBuilder()
          .withUrl(environment.hubConnectionStatusEndpoint, { accessTokenFactory: () => { return of(this.loginService.token).toPromise(); } })
          .build();
      }
      else if (flow == this.credentialStatusFlow) {
        this._hubs[flow] = new HubConnectionBuilder()
          .withUrl(environment.hubCredentialsStatusEndpoint, { accessTokenFactory: () => { return of(this.loginService.token).toPromise(); } })
          .build();
      }
    }

    return this._hubs[flow];
  }

  public killSignalR(flow) {
    if (this.debug) console.log("AppService killSignalR");
    if (this._hubs[flow]) {
      if (flow == this.proofFlow) {
        this._hubs[flow].off(this.proofMethod);
      }
      else if (flow == this.generateInvitationFlow) {
        this._hubs[flow].off(this.generateInvitationMethod);
      }
      else if (flow == this.completeInvitationFlow) {
        this._hubs[flow].off(this.completeInvitationMethod);
      }
      else if (flow == this.credentialStatusFlow) {
        this._hubs[flow].off(this.credentialStatusMethod);
      }
      this._hubs[flow].stop();
    }
  }
  private startSignalR(flow: string, args: any = null): any {
    let cnt = 0;
    if (!!this._hubs[flow]) {
      let connection = this._hubs[flow];
      connection
        .start()
        .then(() => {
          if (this.debug) console.log("AppService SignalR Connected.");
          if (flow == this.proofFlow) {
            console.log("proof stuff happening?");
            connection.invoke("JoinGroup", args).then(() => {
              connection.on(this.proofMethod, (status, finished) => {
                if (this.debug) console.log(`${this.proofMethod}: `, status);
                this.proofStatusChanged.emit(status);
                if (finished)
                  this.proofFinished.emit(finished);
              });
            });
          }
          else if (flow == this.generateInvitationFlow) {
            this.generateInvitationStatusChanged.emit(new WalletConnectionStatus(-1, 'Generating Request', false));
          } else if (flow == this.credentialStatusFlow) {
            this.credentialStatusChanged.emit(new CredentialSendStatus(-1, -1, 'Generating Request', false, false, false));
          }
        })
        .catch(err => {
          if (this.debug) console.log(`AppService SignalR start error. ${cnt}`);
          console.error(err);
          if (cnt < 20) {
            this.killSignalR(flow);
            setTimeout(this.startSignalR(flow, args), 5000);
          } else {
            throwError("AppService Cannot start SignalR connection");
          }
        });
    }
  }
}

