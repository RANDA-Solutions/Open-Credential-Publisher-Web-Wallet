import { EventEmitter, Injectable, OnDestroy } from '@angular/core';
import { environment } from '@environment/environment';
import { DEFAULT_INTERRUPTSOURCES, Idle, StorageInterruptSource, WindowInterruptSource } from '@ng-idle/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthService } from '../auth/auth.service';

@UntilDestroy()
@Injectable({
  providedIn: 'root',
})
export class TimeoutService implements OnDestroy {
  private _watching: boolean = false;
  private _debug: boolean = environment.debug;
  private _onBadRefresh = new EventEmitter<boolean>();

  private INTERRUPT_SOURCES: any[] = [new StorageInterruptSource(), new WindowInterruptSource('keyup scroll touch focus')];
  private id: string;
  constructor(
    private _idle: Idle,
    private authService: AuthService,
  ) {
    this.id = new Date().toDateString();
    if (this._debug)
      console.log("Timeout Service: ", this.id);

    this.authService.userLoaded.pipe(untilDestroyed(this)).subscribe(val => {
      if (this.authService.isLoggedIn && !this._watching)
        this.startWatching();
    });

    this.authService.userUnloaded.pipe(untilDestroyed(this)).subscribe(val => {
      if (this._watching)
        this.stopWatching();
    });

    this.authService.accessTokenExpiring.pipe(untilDestroyed(this)).subscribe(val => {
      if (this._debug)
        console.log("Access token is expiring");
        //this.authService.checkLogin();

    });

    this.authService.accessTokenExpired.pipe(untilDestroyed(this)).subscribe(val => {
      if (this._debug)
        console.log("Access token is expired");
    });
  }


  initialize() {
    //this._idle.setAutoResume(AutoResume.disabled);
	this._idle.setIdle(10); // how many seconds before considered idle
    this._idle.setTimeout(environment.logoutTimer * 60); // multiply by seconds
    this._idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    this._idle.onIdleStart.pipe(untilDestroyed(this)).subscribe(() => {
      if (this._debug) console.log(`${new Date()}: Idle has started`);
    });

	this._idle.onIdleEnd.pipe(untilDestroyed(this)).subscribe((e) => {
		if (this._debug) console.log(`${new Date()}: Idle ended`);
    });

    this._idle.onInterrupt.pipe(untilDestroyed(this)).subscribe((e) => {
		if (this._debug) console.log(`${new Date()}: Idle interrupted`);
		if (this._idle.isIdling())
		{
			this._idle.watch();
		}
    });
  }

  startWatching() {
	if (!this._watching) {
		this._watching = true;
		this._idle.watch();
	}
  }

  stopWatching() {
	  this._watching = false;
	  this._idle.stop();
  }

  ngOnDestroy() {}
}
