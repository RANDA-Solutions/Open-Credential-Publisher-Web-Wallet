import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthenticatedResult } from 'angular-auth-oidc-client/lib/auth-state/auth-result';
import { filter, map, mergeMap, tap } from 'rxjs/operators';
import { LoginService } from '../auth/auth.service';
@UntilDestroy()
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
	public isExpanded = false;
	public get isAuthenticated() : boolean { return this._authResult?.isAuthenticated ?? false; };
	public userName: string;
  public showNavMenu: boolean;
  private _authResult: AuthenticatedResult;

	constructor(private loginService: LoginService, private router: Router, private activatedRoute: ActivatedRoute) {
    this.showNavMenu = true;

		loginService.isLoggedIn
			.pipe(untilDestroyed(this), tap(val => this._authResult = val)).subscribe(
				val => {
					this._authResult = val;
				});
    router.events
      .pipe(filter(event => event instanceof NavigationEnd), untilDestroyed(this))
      .subscribe((val) => {
        this.isExpanded = false;
      });
	}

	ngOnInit() {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map(route => {
          while (route.firstChild) {
            route = route.firstChild;
          }
          return route;
        }),
      )
      .pipe(
        filter(route => route.outlet === 'primary'),
        mergeMap(route => route.data),
      )
      .subscribe(event => {
        this.showNavmenu(event.hideNavBar); // show the navmenu?
      });
		this.loginService.userData.subscribe(user => {
      if (!!user) {
        this.userName = user['preferred_username'];
      }
		});
	}
	collapse() {
		this.isExpanded = false;
	}

	toggle() {
		this.isExpanded = !this.isExpanded;
	}
  showNavmenu(event) {
    if (event === false) {
      this.showNavMenu = true;
    } else if (event === true) {
      this.showNavMenu = false;
    } else {
      this.showNavMenu = this.showNavMenu;
    }
  }

  logout() {
    this.loginService.signOut();
  }
}
