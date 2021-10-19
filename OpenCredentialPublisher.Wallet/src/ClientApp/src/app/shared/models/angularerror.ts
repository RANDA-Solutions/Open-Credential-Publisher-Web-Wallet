export class AngularError {
  source: string;
  message: string;

  constructor(src: string, msg: string) {
    this.source = src;
    this.message = msg;
  }
}
