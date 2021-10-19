import { Component, Input, OnInit } from '@angular/core';
import { CryptographicKeyDType } from '@shared/models/clrLibrary/cryptographicKeyDType';

@Component({
  selector: 'app-public-key',
  templateUrl: './public-key.component.html',
  styleUrls: ['./public-key.component.scss']
})
export class PublicKeyComponent implements OnInit {
  @Input() key: CryptographicKeyDType;
  constructor() { }

  ngOnInit(): void {
  }

}
