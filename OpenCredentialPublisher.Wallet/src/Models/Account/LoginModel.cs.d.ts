declare module server {
	interface inputModel {
		email: string;
		password: string;
		rememberMe: boolean;
		returnUrl: string;
	}
	interface loginModel extends postModel {
		result?: any;
		externalLogins: any[];
		returnUrl: string;
	}
	interface postModel extends genericModel {
	}
	const enum loginResultEnum {
		success,
		twoFactorAuthentication,
		lockout,
		error,
	}
}
