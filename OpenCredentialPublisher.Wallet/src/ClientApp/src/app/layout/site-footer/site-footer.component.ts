import { Component, OnInit } from "@angular/core";
import { AppService } from "@core/services/app.service";
import { environment } from "@environment/environment";
import { ApiOkResponse } from "@shared/models/apiOkResponse";
import { FooterSettingsVM } from "@shared/models/footerSettingsVM";
import { take } from "rxjs/operators";

@Component({
	selector: 'app-site-footer',
	styleUrls: ['./site-footer.component.scss'],
	templateUrl: './site-footer.component.html'
})
export class SiteFooterComponent implements OnInit {
	footerSettingsVM = new FooterSettingsVM();

	constructor(private appService: AppService) {

	}


	ngOnInit(): void {
		this.appService
			.getFooterSettings()
			.pipe(take(1))
			.subscribe(data => {
				if (data.statusCode == 200) {
					this.footerSettingsVM = (<ApiOkResponse>data).result as FooterSettingsVM;
					if (environment.debug) 
						console.log(`AppComponent gotData ${JSON.stringify(this.footerSettingsVM)}`);
				} else {
					this.footerSettingsVM = new FooterSettingsVM();
					if (environment.debug) 
						console.log(`AppComponent gotData ${data.statusCode}`);
				}
			});
	}

	
}