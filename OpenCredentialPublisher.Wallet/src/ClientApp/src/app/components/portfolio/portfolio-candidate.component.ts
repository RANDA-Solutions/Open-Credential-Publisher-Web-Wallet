import { Component, NgModuleRef, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { CandidatesModel } from 'src/app/models/candidates.model';
import { CandidateService } from 'src/app/services';

@UntilDestroy()
@Component({
  selector: 'app-portfolio-candidate',
  templateUrl: './portfolio-candidate.component.html',
  styleUrls: ['./portfolio-candidate.component.css']
})
export class PortfolioCandidateComponent implements OnInit {

  model$: Observable<CandidatesModel>|null;

  constructor(private candidateService: CandidateService) {
    this.model$ = this.candidateService.model$;  // subscribe and do stuff?
  }

  ngOnInit(): void {

  }

  ngOnDestroy() {
    
  }

}
