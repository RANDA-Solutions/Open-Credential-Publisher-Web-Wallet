import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { environment } from "@environment/environment";
import { Observable } from "rxjs";
import { CodeService } from "./code.service";

@Injectable({
	providedIn: 'root'
  })
  export class CodeResolver implements Resolve<string> {
	  constructor(private codeService: CodeService, private router: Router) {}

	  resolve(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	  ): Observable<any>|Promise<any>|any {
      if (environment.debug) {
        console.log(route.params);
      }
		  let code = route.params.code;
      return this.codeService.claimCode(code).subscribe(response => {
        if (response.invalid) {
          this.router.navigate(['/access/code/invalid']);
        }
        else if (response.expired) {
          this.router.navigate(['/access/code/expired']);
        }

      });
	  }
  }


