export class ChangePasswordVM {
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
  constructor() {
    this.oldPassword = '';
    this.newPassword = '';
    this.confirmPassword = '';
  }
};
