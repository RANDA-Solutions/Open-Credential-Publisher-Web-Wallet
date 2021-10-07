import { Component, OnInit } from "@angular/core";
import { AppService } from "@core/services/app.service";

@Component ({
	selector: 'app-search-base',
  	templateUrl: './search.component.html',
  	styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
	public isExpanded = false;

	constructor(private appService: AppService) {
	} 
	ngOnInit() {

	}

	toggle() {
		this.isExpanded = !this.isExpanded;
	}
}