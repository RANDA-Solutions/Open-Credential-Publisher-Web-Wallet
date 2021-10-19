export class ChangePasswordVM {
  oldPassword: string;
  password: string;
  confirmPassword: string;
  constructor() {
    this.oldPassword = '';
    this.password = '';
    this.confirmPassword = '';
  }
};
