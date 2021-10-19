import { Component, Input, OnInit } from '@angular/core';
import { environment } from '@environment/environment';
import { Proof } from '@shared/models/clrLibrary/proof';

@Component({
  selector: 'app-proof',
  templateUrl: './proof.component.html',
  styleUrls: ['./proof.component.scss']
})
export class ProofComponent implements OnInit {
  @Input() pkgId: number;
  @Input() proof: Proof;

  showSpinner = false;
  private debug = false;

  constructor() { }

  ngOnInit(): void {
    if (this.debug) console.log('ProofComponent ngOnInit');
  }

}
