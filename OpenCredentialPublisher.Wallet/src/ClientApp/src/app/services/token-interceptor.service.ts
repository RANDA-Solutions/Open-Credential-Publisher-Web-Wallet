import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { Observable } from "rxjs";
import { AuthService } from "../auth/auth-client.service";
import { SecureRoutesService } from "./secureRoutes.service";

@Injectable({
	providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {
	constructor(private authService: AuthService, private secureRoutesService: SecureRoutesService) {}
  
	intercept(
	  req: HttpRequest<any>,
	  next: HttpHandler
	): Observable<HttpEvent<any>> {
		if (this.secureRoutesService.isSecure(req.url)) {
			if (environment.debug) {
				console.log("TokenInterceptor: ", req.url);
			}
			let token = this.authService.getAccessToken();
			if (token) {
				req = req.clone({
					setHeaders: {
					Authorization: `Bearer ${token}`
					}
				});
			}
		}
		return next.handle(req);
	}
  }