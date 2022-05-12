export class RegisterAccountVM {
  displayName: string;
  email: string;
  password: string;
  confirmPassword: string;
  returnUrl: string;
  constructor() {
    this.displayName = '';
    this.email = '';
    this.password = '';
    this.confirmPassword = '';
    this.returnUrl = '';
  }
}
