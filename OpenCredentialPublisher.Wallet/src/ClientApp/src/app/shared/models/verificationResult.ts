export class VerificationResult {
  error: string;
  id: string;
  message: string;
  infoBubble: boolean;
  bubbleText: string;
  revocationsMessage: string;
  constructor(){
    this.error='';
    this.id='';
    this.message = '';
    this.infoBubble = false;
    this.bubbleText = '';
    this.revocationsMessage = '';
  }
}
