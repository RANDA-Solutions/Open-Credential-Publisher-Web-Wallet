import { Component, Input, OnInit } from '@angular/core';
import { environment } from '@environment/environment';
import { ResultDescriptionVM } from '@shared/models/clrSimplified/resultDescriptionVM';
import { ResultVM } from '@shared/models/clrSimplified/resultVM';
import { RubricCriterionLevelVM } from '@shared/models/clrSimplified/rubricCriterionLevelVM';
import { ResultData } from '@shared/models/resultData';

@Component({
  selector: '[app-result]',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent implements OnInit {
  @Input() result: ResultVM;
  @Input() resultDescriptions: Array<ResultDescriptionVM>;
  resultData: ResultData;
  showSpinner = false;
  showIt = false;

  private debug = false;

  constructor() { }
  ngOnChanges() {
    if (this.debug) console.log('ResultComponent ngOnChanges');
    if (this.result != null && this.resultDescriptions != null) {
      this.showSpinner = true;
      if (this.debug) console.log('ResultComponent ngOnChanges got results & descriptions');
      this.resultData = this.getResultData(this.result);
      this.showSpinner = false;
      this.showIt = true;
    }
  }

  ngOnInit(): void {
    if (this.debug) console.log('ResultComponent ngOnInit');
    if (this.result != null && this.resultDescriptions != null) {
      if (this.debug) console.log('ResultComponent ngOnInit got results & descriptions');
    }
  }

  getResultData(result: ResultVM): ResultData {
    var resultDesc = null as ResultDescriptionVM;
    var achievedLevel = null as RubricCriterionLevelVM;
    var requiredLevel = null as RubricCriterionLevelVM;

    let found = this.resultDescriptions.findIndex(e => {
      return e.id === result.resultDescription;
    });

    if (found > -1){
      resultDesc = this.resultDescriptions[found];
      if (this.debug) console.log(`resultDesc: ${JSON.stringify(resultDesc)}`);
      if (resultDesc.rubricCriterionLevels != null) {
        found = resultDesc.rubricCriterionLevels.findIndex(e => {
          return e.id === result.achievedLevel;
        });
        if (found > -1) {
          achievedLevel = resultDesc.rubricCriterionLevels[found];
        }
        found = resultDesc.rubricCriterionLevels.findIndex(e => {
          return e.id === resultDesc.requiredLevel;
        });
        if (found > -1) {
          requiredLevel = resultDesc.rubricCriterionLevels[found];
        }
      } else {
        if (this.debug) console.log('ResultComponent.rubricCriterionLevels is null');
      }
    } else {
      if (this.debug) console.log('ResultComponent.getResultData resultDesc not found');
    }

    return new ResultData(resultDesc, requiredLevel, achievedLevel);
  }
}
