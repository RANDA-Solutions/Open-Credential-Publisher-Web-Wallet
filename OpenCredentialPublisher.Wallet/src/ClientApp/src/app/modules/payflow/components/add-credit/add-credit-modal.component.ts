import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { BehaviorSubject, Observable } from "rxjs";
import { FormComponent } from 'src/app/modules/directives';
import { Result } from "src/app/modules/directives/models/result";
import { PayflowService } from "../../services/payflow.service";

@Component({
  selector: 'app-add-credit-modal',
  templateUrl: './add-credit-modal.component.html',
  styleUrls: ['./add-credit-modal.component.css']
})
export class AddCreditModalComponent extends FormComponent implements OnInit {
    public model: any;
    private _done: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public done: Observable<boolean> = this._done.asObservable();

    constructor(private formBuilder: FormBuilder, private payflowService: PayflowService, public modal: NgbActiveModal) { 
        super() 
    }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            credits: [1, [Validators.required]]
          });
    }

    submit() {
        this.submitForm(() => {
          return this.payflowService.start(this.form.value);
        }).subscribe((result: Result<any>) => {
            if (result.success) {
                this._done.next(true);
            }
        }
        );
    }
    
    dismiss() {
        this.modal.dismiss();
    }

}