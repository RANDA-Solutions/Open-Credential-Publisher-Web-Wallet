import { DOCUMENT } from '@angular/common';
import { Component, Inject, ViewChild } from '@angular/core';
import { PackagesComponent } from '../components/credential-packages/credential-packages.component';

@Component({
  selector: 'app-credential-list-component',
  templateUrl: './credential-list.component.html',
  styleUrls: ['./credential-list.component.scss']
})
export class CredentialListComponent {

  @ViewChild(PackagesComponent) 
  private packagesComponent: PackagesComponent;

  constructor(@Inject(DOCUMENT) private document: any){}
  topFunction():void {
    const element = document.getElementById ('theTop');
    element.scrollIntoView();
  }

  fileUploaded(event) {
    this.packagesComponent.refresh();
  }
}
