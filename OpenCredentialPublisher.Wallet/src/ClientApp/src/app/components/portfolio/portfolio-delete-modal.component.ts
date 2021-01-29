import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CandidateModel } from 'src/app/models/candidate.model';
import { Result } from 'src/app/modules/directives/models/result';
import { CandidateService } from 'src/app/services';

@Component({
  selector: 'app-portfolio-delete-modal',
  templateUrl: './portfolio-delete-modal.component.html',
  styleUrls: ['./portfolio-delete-modal.component.css']
})
export class PortfolioDeleteModalComponent implements OnInit {
  public candidateId: string;


  constructor(public modal: NgbActiveModal, private candidateService: CandidateService) { }

  ngOnInit(): void {
  }

  delete() {
    this.candidateService.delete(this.candidateId).subscribe((result: Result<CandidateModel>) => {
      if (result.success){
        this.modal.dismiss();
      }
    });
  }

  dismiss() {
    this.modal.dismiss();
  }
}
