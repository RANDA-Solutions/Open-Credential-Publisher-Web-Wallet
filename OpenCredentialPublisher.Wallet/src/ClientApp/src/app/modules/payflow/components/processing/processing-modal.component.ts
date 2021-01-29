import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { BehaviorSubject, Observable } from "rxjs";
import { FormComponent } from 'src/app/modules/directives';

@Component({
  selector: 'app-processing-modal',
  templateUrl: './processing-modal.component.html',
  styleUrls: ['./processing-modal.component.css']
})
export class ProcessingModalComponent extends FormComponent implements OnInit {
    public model: any;
    
    private _done: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public done: Observable<boolean> = this._done.asObservable();

    constructor(public modal: NgbActiveModal) { 
        super() 
    }

    ngOnInit(): void {
        
    }

    start() {
      var promise = new Promise(resolve => setTimeout(resolve, 3000));
      promise.then(() => {
        this._done.next(true);
      });
    }
}