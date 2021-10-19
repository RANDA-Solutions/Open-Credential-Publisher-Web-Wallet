import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SearchService } from "@modules/search/services/search.service";
import { UntilDestroy } from "@ngneat/until-destroy";
import { SearchResponse } from "@shared/models/searchResponse";
import { Observable, of, OperatorFunction } from "rxjs";
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from "rxjs/operators";

@UntilDestroy()
@Component({
	selector: 'app-search-request-component',
	templateUrl: './search-request.component.html',
	styleUrls: ['./search-request.component.scss']
})
export class SearchRequestComponent implements OnInit, OnDestroy {
	constructor(private searchService: SearchService, private router: Router, private route: ActivatedRoute) {}
	model: any;

	modelErrors = new Array<string>();
	submitted: boolean = false;
	loading: boolean = false;
	message: string = "Loading Form...";

	searchTerm: string;
	query: FormControl = new FormControl();
	credentialFormGroup: FormGroup = new FormGroup({
		selection: new FormControl()
	});
	selected: any;
	result: SearchResponse = null;
	searching = false;
  	searchFailed = false;

	ngOnInit() {
	}

	searchCredentials() {
		this.searchService.search(this.searchTerm).subscribe(response => {
			this.model = response.result;
		})
	}

	search: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term =>
        this.searchService.list(term).pipe(
          tap(() => this.searchFailed = false),
          catchError(() => {
            this.searchFailed = true;
            return of([]);
          }))
      ),
      tap(() => this.searching = false)
    )

	ngOnDestroy() {
		
	}
}