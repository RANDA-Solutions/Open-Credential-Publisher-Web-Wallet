import { DOCUMENT } from '@angular/common';
import { Component, Inject, ViewChild } from '@angular/core';

@Component({
  selector: 'app-credential-list-component',
  templateUrl: './credential-list.component.html',
  styleUrls: ['./credential-list.component.scss']
})
export class CredentialListComponent {
  constructor(@Inject(DOCUMENT) private document: any){}
  topFunction():void {
    const element = document.getElementById ('theTop');
    element.scrollIntoView();
  }
}
