import { EventEmitter, Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { User, UserManager } from "oidc-client";
import { BehaviorSubject, Observable } from "rxjs";
import { AuthSettings } from "./auth.settings";

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	private _user: User | null;
	private _userManager: UserManager;

	private _silientRenewStarted: boolean = false;
	private _loggedInBehavior: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	public isLoggedIn$: Observable<boolean> = this._loggedInBehavior.asObservable();

	
	public accessTokenExpiring: EventEmitter<any> = new EventEmitter<any>();
	public accessTokenExpired: EventEmitter<any> = new EventEmitter<any>();
	public silentRenewError: EventEmitter<any> = new EventEmitter<any>();
	public userLoaded: EventEmitter<User> = new EventEmitter<User>();
	public userSignedIn: EventEmitter<boolean> = new EventEmitter<boolean>();
	public userSessionChanged: EventEmitter<any> = new EventEmitter<any>();
	public userUnloaded: EventEmitter<any> = new EventEmitter<any>();

	constructor(private authSettings: AuthSettings) {
		this._userManager = new UserManager(authSettings);
		

		this._userManager.events.addAccessTokenExpired((ev) => {
			if (environment.debug) console.log("Access token expired");
			this.accessTokenExpired.emit(ev);
		});

		this._userManager.events.addAccessTokenExpiring((ev) => {
			if (environment.debug) console.log("Access token is expiring");
			this.accessTokenExpiring.emit(ev);
		});

		this._userManager.events.addUserLoaded((user) => {
			if (environment.debug) console.log("User is loaded");
			this.userLoaded.emit(user);
			this._loggedInBehavior.next(user?.expired == false);
		});

		this._userManager.events.addUserUnloaded(() => {
			if (environment.debug) console.log("User is unloaded");
			this.userUnloaded.emit();
			this._loggedInBehavior.next(false);
		});

		this._userManager.events.addUserSignedOut(() => {
			if (environment.debug) console.log("User is signed out");
			this.userSignedIn.emit(false);
		});

		this._userManager.events.addUserSignedIn(() => {
			if (environment.debug) console.log("User is signed in");
			this.userSignedIn.emit(true);
		});
		
		this._userManager.events.addUserSessionChanged(() => {
			if (environment.debug) console.log("User session changed");
			this.userSessionChanged.emit();
		});

		this._userManager.events.addSilentRenewError((ev) => {
			this.silentRenewError.emit(ev);
		});

		this.getUser();
		if (environment.debug)
		{
			console.log("Auth Service Constructor");
		}
	}

	isLoggedIn() : Observable<boolean> {
		if (environment.debug) console.log("User is logged in: ", this._user);
		return new Observable((obs) => {
			if (this._user) {
				if (this._user.expired) {
					if (environment.debug)
						 console.log("User expired, refreshing login");
					this.getUser().then(() => obs.next(!this._user.expired));
				}
				else {
					obs.next(!this._user.expired);
				}
			}
			else {
				obs.next(false);
			}
		})
		
	}

	getAccessToken() {
		return this._user ? this._user.access_token : '';
	}

	getClaims() {
		return this._user?.profile;
	}

	checkLogin(): Promise<boolean> {
		return this._userManager.signinSilent().then((user) => {
			this._user = user;
			if (!this._silientRenewStarted) {
				this._userManager.startSilentRenew();
				this._silientRenewStarted = true;
			}
			return true;
		}, (reason) => {
			if (environment.debug) console.log("Not logged in: ", reason);
			if (this._silientRenewStarted) {
				this._userManager.stopSilentRenew();
				this._silientRenewStarted = false;
			}
			localStorage.clear();
			return false;
		});
	}

	clearStaleStorage() {
		this._userManager.clearStaleState();
	}

	logout(): Promise<void> {
		if (this._silientRenewStarted) {
			this._userManager.stopSilentRenew();
			this._silientRenewStarted = false;
		}
		return this._userManager.revokeAccessToken().then(() => {
			return this._userManager.removeUser().then(() => {
				this._user = null;
			});
		});
	}

	private getUser() {
		return this._userManager.getUser().then((user) => {
			this._user = user;
		})
	}


}