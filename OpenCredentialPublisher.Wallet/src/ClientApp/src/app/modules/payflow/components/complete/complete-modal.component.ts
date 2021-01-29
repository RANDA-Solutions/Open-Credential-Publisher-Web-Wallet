import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { BehaviorSubject, Observable } from "rxjs";
import { FormComponent } from 'src/app/modules/directives';
import { Result } from "src/app/modules/directives/models/result";
import { PayflowService } from "../../services/payflow.service";

@Component({
  selector: 'app-complete-modal',
  templateUrl: './complete-modal.component.html',
  styleUrls: ['./complete-modal.component.css']
})
export class CompleteModalComponent extends FormComponent implements OnInit {
    public model: any;
    public requestId: string;
    public credits: number;

    private _done: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public done: Observable<boolean> = this._done.asObservable();

    constructor(private payflowService: PayflowService, public modal: NgbActiveModal) { 
        super();
        this.requestId = this.payflowService.requestId;
        this.credits = this.payflowService.credits;
    }

    ngOnInit(): void {
        
    }

    submit() {
      this.payflowService.complete().subscribe((result: Result<any>) => {
        if (result.success) {
            this._done.next(true);
        }
      });
    }
    
    dismiss() {
        this.modal.dismiss();
    }

}