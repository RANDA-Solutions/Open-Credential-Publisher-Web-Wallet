import { Component, OnInit, OnDestroy } from '@angular/core';
import { CodeFlowService } from 'src/app/services';
import { Observable } from 'rxjs';
import { CodeFlowModel } from 'src/app/models/code-flow.model';
import { map } from 'rxjs/operators';
import { untilDestroyed, UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-codeflow',
  templateUrl: './codeflow.component.html',
  styleUrls: ['./codeflow.component.css']
})
export class CodeflowComponent implements OnInit, OnDestroy {
  public model$: Observable<CodeFlowModel>|null;
  constructor(private codeFlowService: CodeFlowService) { 
    this.model$ = this.codeFlowService.codeFlowModel$;
  }

  ngOnInit(): void {
    this.codeFlowService.get().pipe(untilDestroyed(this)).subscribe();
  }

  ngOnDestroy() {}

}
