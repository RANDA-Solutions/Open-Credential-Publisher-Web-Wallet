import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { OidcSecurityService } from "angular-auth-oidc-client";
import { Observable } from "rxjs";

export class TokenInterceptorService implements HttpInterceptor {
	static OidcInterceptorService: any;
	constructor(private oidcSecurityService: OidcSecurityService) {}
  
	intercept(
	  req: HttpRequest<any>,
	  next: HttpHandler
	): Observable<HttpEvent<any>> {
		let token = this.oidcSecurityService.getToken();
		if (token) {
			req = req.clone({
				setHeaders: {
				  Authorization: `Bearer ${token}`
				}
			  });
		}
		return next.handle(req);
	}
  }