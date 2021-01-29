import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { CandidateModel } from 'src/app/models/candidate.model';
import { CandidatesModel } from 'src/app/models/candidates.model';
import { StatusEnum } from 'src/app/models/statusenum.model';
import { VerificationModel } from 'src/app/models/verification.model';
import { FormComponent } from 'src/app/modules/directives';
import { AddCreditModalComponent, CompleteModalComponent, ProcessingModalComponent } from 'src/app/modules/payflow/components';
import { CandidateService, VerificationService } from 'src/app/services';
import { PortfolioDeleteModalComponent } from './portfolio-delete-modal.component';
import { PortfolioVerificationModalComponent } from './portfolio-verification-modal.component';

@Component({
  selector: 'app-candidate-profile',
  templateUrl: './candidate-profile.component.html',
  styleUrls: ['./candidate-profile.component.css']
})
export class CandidateProfileComponent extends FormComponent implements OnInit, OnDestroy {
  private candidatesModel = new CandidatesModel()
  
  candidate: CandidateModel = null;
  fullName: string;
  dob: string;
  candidateId: string;
  candidateStatus: StatusEnum;
  totalTests: string;
  totalCredits: string;
  showAddPortfolioBtn: boolean = true;

  constructor(private candidateService: CandidateService, private formBuilder: FormBuilder, private modalService: NgbModal, private verificationService: VerificationService) {
    super();
  }


  ngOnInit(): void {

    this.form = this.formBuilder.group({
      candidateId: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8)]]
    });

    this.candidateService.model$.subscribe(m => {
      if(m){
        this.candidatesModel = m;
        this.candidate = this.candidatesModel.candidates.find(x => x.candidateId !== null);  // do it better
        
        if (this.candidate) {
          this.fullName = this.getFullName();
          this.dob = this.candidate.dob;
          this.candidateId = this.candidate.candidateId;
          this.candidateStatus = this.candidate.status;
          this.totalTests = this.candidateStatus === 13 ? this.candidate.totalTests.toString() : '-';
          this.totalCredits = this.candidate.totalCredits > 0 ? this.candidate.totalCredits.toString() : '-';

          if (this.candidate.status !== 11) {
            this.showAddPortfolioBtn = false;
          }
        }
       
        }
    });

  }

  getFullName() {
      if (this.candidate){
      const value = `${this.candidate.firstName} ${this.candidate.lastName}`;
      return value ? value : '-';
  
    }
    return '-';
  }

  submit() {
    this.submitForm(() => {
      var result = this.candidateService.add(this.form.value);
      if(result){
        this.showAddPortfolioBtn = false;

      }
      return result;
    }).subscribe();
  }

  verifyCandidate(candidateId: string) {
    this.verificationService.get(candidateId).subscribe((result: VerificationModel) => {
      if(!result.error) {
        const modalRef = this.modalService.open(PortfolioVerificationModalComponent, {size: 'lg', centered: true});
        modalRef.componentInstance.model = result;
      }
    });
    
  }

  delete (candidateId: string) {    
    const modalRef = this.modalService.open(PortfolioDeleteModalComponent, {size: 'lg', centered:true});
        modalRef.componentInstance.candidateId = candidateId;
  }

  addCredit() {
    const modalRef = this.modalService.open(AddCreditModalComponent, {size: 'lg', centered: true});
    modalRef.componentInstance.done.subscribe((result: boolean) => {
      console.log("In add credit", result);
      if (result) {
        modalRef.close();
        this.showProcessing();
        
      }
    });
  }

  showProcessing() {
    const modalRef = this.modalService.open(ProcessingModalComponent, {size: 'lg', centered: true});
    modalRef.componentInstance.done.subscribe((result: boolean) => {
      console.log("In showProcessing", result);
      if (result) {
        modalRef.close();
        this.showComplete();
      }
    });
    modalRef.componentInstance.start();
  }

  showComplete() {
    const modalRef = this.modalService.open(CompleteModalComponent, {size: 'lg', centered: true});
    modalRef.componentInstance.done.subscribe((result: boolean) => {
      console.log("In showComplete", result);
      if (result) {
        modalRef.close();
        this.candidateService.get().subscribe();
      }
    });
  }


  ngOnDestroy() {
    
  }
}
