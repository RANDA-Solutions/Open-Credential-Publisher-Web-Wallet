import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { environment } from "@environment/environment";

@Injectable({
    providedIn: 'root'
  })
  export class WalletsResolver implements Resolve<any> {
      private environment = environment;
      constructor() {}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.environment.showWallets;
    }

  }
