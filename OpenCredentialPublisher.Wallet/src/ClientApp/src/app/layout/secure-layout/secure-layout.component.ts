import { Component } from "@angular/core";
import { AppService } from "@core/services/app.service";

@Component({
	selector: 'app-secure-layout',
	styleUrls: ['./secure-layout.component.scss'],
	templateUrl: './secure-layout.component.html'
})
export class SecureLayoutComponent {
	constructor(public appService: AppService) {
		
	}
}