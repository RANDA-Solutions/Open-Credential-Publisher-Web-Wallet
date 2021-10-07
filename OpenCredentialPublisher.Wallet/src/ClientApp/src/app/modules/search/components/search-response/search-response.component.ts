import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from '@environment/environment';
import { AttachmentAttribute, ProofResponse } from '@shared/models/proofResponse';

@Component({
  selector: 'app-search-response',
  templateUrl: './search-response.component.html',
  styleUrls: ['./search-response.component.scss']
})
export class SearchResponseComponent implements OnInit {
  @Input() proof: ProofResponse;
  showSpinner = false;
  private debug = false;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnChanges() {
    if (this.debug) console.log('ProofResponseComponent ngOnChanges');
  }

  ngOnInit() {
    console.log(this.proof);
    if (this.debug) console.log('ProofResponseComponent ngOnInit');
  }

  getVerificationResult() {
    return this.proof.verificationResult.replace(/([A-Z])/g, ' $1').trim();
  }

  isValidated(): boolean {
    return this.proof.verificationResult == "ProofValidated";
  }

  getItemName(name) {
    return name.replace(/([^a-z])([a-z])(?=[a-z]{2})|^([a-z])|^(_)/g, function(_, g1, g2, g3) {
      return (typeof g1 === 'undefined') ? g3.toUpperCase() : g1 + g2.toUpperCase(); } ).replace(/_/g, ' ');
  }

  instanceOfAttachmentType(item) : item is AttachmentAttribute {
    if (typeof item == 'object') {
      console.log(item);
      return 'mime-type' in item;
    }
    return false;
  }

  async getAttachmentLink(item) {
    let attachment = item;
    const base64 = await fetch(`data:${attachment['mime-type']};base64,${attachment.data.base64}`);
    const blob = await base64.blob();
    const fileUrl = URL.createObjectURL(blob);
    window.open(fileUrl, '_blank');
    //return this.sanitizer.bypassSecurityTrustUrl(`data:${attachment['mime-type']};base64,${attachment.data.base64}`);
  }

  getAttachmentName(item) {
    return item.name;
  }

  getAttachmentExtension(item) {
    return item.extension;
  }

  isLink(value) {
    return value.startsWith("http");
  }

  ngOnDestroy(): void {
  }
}
