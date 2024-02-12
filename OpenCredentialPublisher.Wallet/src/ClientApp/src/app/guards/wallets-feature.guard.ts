import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { environment } from '@environment/environment';

@Injectable({
	providedIn: 'root'
})
export class WalletsFeatureGuard implements CanActivate {
	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    	return environment.showWallets;
	}
}
