import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
	providedIn: 'root'
  })
  export class ResetPasswordResolver implements Resolve<string> {
	  constructor(private router: Router) {}

	  resolve(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	  ): Observable<any>|Promise<any>|any {
		return route.paramMap.get('code');
	  }
  }