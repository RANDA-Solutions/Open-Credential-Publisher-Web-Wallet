import { Component, NgZone, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthService } from '@root/app/auth/auth.service';

@UntilDestroy()
@Component({
	selector: 'app-logout',
	templateUrl: './logout.component.html'
})

export class LogoutComponent implements OnInit {

  message = 'logging out';
  private _infoMessage?: string;
  constructor(
	private authService: AuthService,
	private router: Router,
	private ngZone: NgZone,
	private activeRoute: ActivatedRoute
) {
	this.activeRoute.queryParams.pipe(untilDestroyed(this)).subscribe(
		(param: any) => {
			this._infoMessage = param['infoMessage'];
			if (environment.debug)
				console.log(this._infoMessage);
		});
}

ngOnInit() {
	var self = this;
	setTimeout((self) => {
		this.ngZone.run(() => {
			this.authService.logout().then(
				() => {
					console.log(this._infoMessage);
					this.router.navigate(["/access/login"], { queryParams: { infoMessage: this._infoMessage }});
				}
			);
		},  self);
	}, 500, [ self ]);
}
}
