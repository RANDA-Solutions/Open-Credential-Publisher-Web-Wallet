export class RegisterAccountVM {
  userName: string;
  displayName: string;
  email: string;
  password: string;
  confirmPassword: string;
  returnUrl: string;
  constructor() {
    this.userName = '';
    this.displayName = '';
    this.email = '';
    this.password = '';
    this.confirmPassword = '';
    this.returnUrl = '';
  }
}
