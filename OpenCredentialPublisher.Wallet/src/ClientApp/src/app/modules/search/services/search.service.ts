import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { ApiOkResponse } from "@shared/models/apiResponse";
import { SearchResponse } from "@shared/models/searchResponse";
import { WordList } from "@shared/models/wordList";
import { map } from "rxjs/operators";

@Injectable({
	providedIn: "root"
})
export class SearchService {

	constructor(private http: HttpClient) {}

	list(word: string) {
		const apiUrl = `${environment.apiEndPoint}search/list/${word}`;
		return this.http.get<ApiOkResponse<WordList>>(apiUrl).pipe(map(wl => wl.result.words));
	}

	search(word: string) {
		const apiUrl = `${environment.apiEndPoint}search/${word}`;
		return this.http.get<ApiOkResponse<SearchResponse>>(apiUrl);
	}
}