import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { environment } from "@environment/environment";
import { SourcesService } from "@modules/sources/sources.service";
import { ApiBadRequestResponse } from "@shared/models/apiBadRequestResponse";
import { ApiOkResponse } from "@shared/models/apiOkResponse";
import { SourceCallback } from "@shared/models/sourceCallback";
import { SourcesCallbackResponse } from "@shared/models/sourcesCallbackResponse";
import { map } from "rxjs/operators";

@Injectable({
	providedIn: 'root'
})
export class CallbackResolverService implements Resolve<SourcesCallbackResponse> {
	private debug = false;
	constructor(private sourcesService: SourcesService) {}
	resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		let callback: SourceCallback = {
			code: route.queryParams.code,
			scope: route.queryParams.scope,
			state: route.queryParams.state,
			error: route.queryParams.error
		};
		if (this.debug) {
			console.log("Callback: ", callback);
		}
		return this.sourcesService.postCallback(callback).pipe(map(response => {
			let sourcesCallbackResponse = new SourcesCallbackResponse();
			if (response.statusCode == 200) {
				sourcesCallbackResponse.error = false;
				sourcesCallbackResponse.sourceId = (<ApiOkResponse>response).result as string;
			}
			else {
				sourcesCallbackResponse.error = true;
				sourcesCallbackResponse.errorMessages = (<ApiBadRequestResponse>response).errors;
			}
			return sourcesCallbackResponse;
		}));
	}
}
