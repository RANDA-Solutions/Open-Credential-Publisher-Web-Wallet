import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { LoginService } from "@root/app/auth/login.service";
import { Observable } from "rxjs";
import { SecureRoutesService } from "./secureRoutes.service";

@Injectable({
	providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {
	constructor(private loginService: LoginService, private secureRoutesService: SecureRoutesService) {}
  
	intercept(
	  req: HttpRequest<any>,
	  next: HttpHandler
	): Observable<HttpEvent<any>> {
		if (environment.debug) {
			console.log("TokenInterceptor: ", req.url);
		}
		if (this.secureRoutesService.isSecure(req.url)) {
			let token = this.loginService.token;
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