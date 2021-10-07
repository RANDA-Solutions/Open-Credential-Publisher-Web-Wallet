import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ClrService } from '@core/services/clr.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { AssertionResultsVM } from '@shared/models/clrSimplified/assertionsResultsVM';
import { ResultDescriptionVM } from '@shared/models/clrSimplified/resultDescriptionVM';
import { ResultVM } from '@shared/models/clrSimplified/resultVM';
import { take } from 'rxjs/operators';

@Component({
  selector: '[app-results]',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {
  @Input() clrId: number
  @Input() assertionId: string;
  @Output() doResultsExist = new EventEmitter<boolean>();
  results = new Array<ResultVM>();
  resultDescriptions = new Array<ResultDescriptionVM>();
  showSpinner = false;
  showIt = false;
  private debug = false;

  constructor(private clrService: ClrService, public utils: UtilsService) { }

  ngOnChanges() {
    if (this.debug) console.log('ResultsComponent ngOnChanges');
    if (this.assertionId != null && this.clrId != null) {
      if (this.debug) console.log('ResultsComponent ngOnChanges getting results & descriptions');
      this.getData();
    }

  }
  ngOnInit(): void {
    if (this.debug) console.log('ResultsComponent ngOnInit');
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('ResultsComponent getData');
    this.clrService.getAssertionResultsVM(this.clrId, encodeURIComponent(this.assertionId))
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.results = ((<ApiOkResponse>data).result as AssertionResultsVM).results;
          this.doResultsExist.emit(this.results.length > 0 );
          this.resultDescriptions = ((<ApiOkResponse>data).result as AssertionResultsVM).resultDescriptions;
          if (this.debug) console.log(`ResultsComponent resultscount: ${this.results.length}`);
        } else {
          this.results = new Array<ResultVM>();
          this.resultDescriptions = new Array<ResultDescriptionVM>();
        }
        this.showSpinner = false;
      });
  }
}
