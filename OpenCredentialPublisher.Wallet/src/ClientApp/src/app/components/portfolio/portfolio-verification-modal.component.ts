import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormComponent } from 'src/app/modules/directives';
import { VerificationService } from 'src/app/services';

@Component({
  selector: 'app-portfolio-verification-modal',
  templateUrl: './portfolio-verification-modal.component.html',
  styleUrls: ['./portfolio-verification-modal.component.css']
})
export class PortfolioVerificationModalComponent extends FormComponent implements OnInit {
  public model: any;
  public disableVerifyBtn: boolean = false;  
  public isProcessing: boolean = false;

  constructor(private formBuilder: FormBuilder, private verificationService: VerificationService, public modal: NgbActiveModal) { 
    super() 
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      ssn: [''],
      dob: ['', [Validators.required]],
      nameSelectionId: ['', [Validators.required]],
      addressSelectionId: ['', [Validators.required]],
      verificationId:['']
    });

    this.form.get('verificationId').setValue(this.model.verificationId);
  }

  submit() {
    this.submitForm(() => {
      this.disableVerifyBtn = true;
      this.isProcessing = true;
      return this.verificationService.post(this.form.value);
     }).subscribe(
       //b => {
    //   if (b.success) {
    //     this.modal.dismiss();
    //   }
    //}
    );
  }

  

  dismiss() {
    this.modal.dismiss();
  }
}
