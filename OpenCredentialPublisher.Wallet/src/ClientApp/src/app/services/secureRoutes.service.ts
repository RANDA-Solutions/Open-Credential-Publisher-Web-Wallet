import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";

@Injectable({
	providedIn: "root"
})
export class SecureRoutesService {
	private insecureRoutes: string[] = [
		'/access/',
		'/public/',
		'/verifier/',
		'/search/',
		'/unauthorized/',
		'/sources-callback',
		'/sources-error'
	];

	isSecure(route: string) : boolean {
		let routes = environment.secureRoutes;
		return routes.every((val) => {
			return route.toLowerCase().includes(val);
		});
	}

	isInsecure(route: string) : boolean {
		return this.insecureRoutes.every((val) => {
			return route.toLowerCase().includes(val);
		});
	}
}