import { Component } from "@angular/core";
import { AppService } from "@core/services/app.service";

@Component({
	selector: 'app-plain-layout',
	styleUrls: ['./plain-layout.component.scss'],
	templateUrl: './plain-layout.component.html'
})
export class PlainLayoutComponent {
	constructor(public appService: AppService) {
		
	}
}