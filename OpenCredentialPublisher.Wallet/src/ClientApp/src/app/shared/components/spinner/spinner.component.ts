import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { environment } from '@environment/environment';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnChanges, OnInit {
  @Input() message = '';
  private debug = false;

  constructor() { }
  ngOnInit() {
    if (this.debug) console.log('SpinnerComponent ngOnInit');
  }

  ngOnChanges() {

  }
}
