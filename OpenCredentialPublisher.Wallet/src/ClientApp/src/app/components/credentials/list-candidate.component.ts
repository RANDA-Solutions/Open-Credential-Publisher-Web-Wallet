import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Observable } from 'rxjs';
import { CandidatesModel } from 'src/app/models/candidates.model';
import { FormComponent } from 'src/app/modules/directives';
import { CandidateService } from '../../services';

@UntilDestroy()
@Component({
  selector: 'app-list-candidate',
  templateUrl: './list-candidate.component.html',
  styleUrls: ['./list-candidate.component.css']
})
export class ListCandidateComponent extends FormComponent implements OnInit, OnDestroy {

  model$: Observable<CandidatesModel>|null;

  constructor(private candidateService: CandidateService, private formBuilder: FormBuilder) {
    super();
    this.model$ = this.candidateService.model$;
  }

  ngOnInit(): void {

    this.form = this.formBuilder.group({
      candidateId: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8)]]
    });

    this.candidateService.get().pipe(untilDestroyed(this)).subscribe();
  }

  submit() {
    this.submitForm(() => {
      return this.candidateService.add(this.form.get('candidateId').value);
    }).subscribe();
  }

  ngOnDestroy() {
    
  }

}
