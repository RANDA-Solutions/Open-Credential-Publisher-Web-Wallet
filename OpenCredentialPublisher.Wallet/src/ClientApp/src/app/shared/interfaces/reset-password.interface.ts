export interface ResetPasswordModel {
	code: string;
	email: string;
	password: string;
	confirmPassword: string;
}