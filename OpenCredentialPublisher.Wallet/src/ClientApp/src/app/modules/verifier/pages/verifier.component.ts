import { Component, OnInit } from "@angular/core";

@Component ({
	selector: 'app-verifier-base',
  	templateUrl: './verifier.component.html',
  	styleUrls: ['./verifier.component.scss']
})
export class VerifierComponent implements OnInit {
	public isExpanded = false;

	constructor() {}
	ngOnInit() {

	}

	toggle() {
		this.isExpanded = !this.isExpanded;
	}
}