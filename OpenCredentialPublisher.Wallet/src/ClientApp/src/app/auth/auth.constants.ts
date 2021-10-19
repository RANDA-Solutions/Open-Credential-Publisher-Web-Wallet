export const applicationName = 'teacherwalletclient';

export const returnUrlType = 'returnUrl';

export const queryParameterNames = {
	returnUrl: returnUrlType,
	message: 'message'
};

export const logoutActions = {
	logoutCallback: 'logout-callback',
	logout: 'logout',
	loggedOut: 'logged-out'
};

export const loginActions = {
	login: 'login',
	loginCallback: 'login-callback',
	loginFailed: 'login-failed',
	profile: 'profile',
	register: 'register'
};

interface ApplicationPathsType {
	readonly defaultLoginRedirectPath: string;
	readonly apiAuthorizationClientConfigurationUrl: string;
	readonly login: string;
	readonly loginFailed: string;
	readonly loginCallback: string;
	readonly register: string;
	readonly profile: string;
	readonly logOut: string;
	readonly loggedOut: string;
	readonly logOutCallback: string;
	readonly loginPathComponents: string [];
	readonly loginFailedPathComponents: string [];
	readonly loginCallbackPathComponents: string [];
	readonly registerPathComponents: string [];
	readonly profilePathComponents: string [];
	readonly logOutPathComponents: string [];
	readonly loggedOutPathComponents: string [];
	readonly logOutCallbackPathComponents: string [];
	readonly identityRegisterPath: string;
	readonly identityManagePath: string;
}

let applicationPaths: ApplicationPathsType = {
	defaultLoginRedirectPath: '/',
	apiAuthorizationClientConfigurationUrl: `_configuration/${applicationName}`,
	login: `authentication/${loginActions.login}`,
	loginFailed: `authentication/${loginActions.loginFailed}`,
	loginCallback: `authentication/${loginActions.loginCallback}`,
	register: `authentication/${loginActions.register}`,
	profile: `authentication/${loginActions.profile}`,
	logOut: `authentication/${logoutActions.logout}`,
	loggedOut: `authentication/${logoutActions.loggedOut}`,
	logOutCallback: `authentication/${logoutActions.logoutCallback}`,
	loginPathComponents: [],
	loginFailedPathComponents: [],
	loginCallbackPathComponents: [],
	registerPathComponents: [],
	profilePathComponents: [],
	logOutPathComponents: [],
	loggedOutPathComponents: [],
	logOutCallbackPathComponents: [],
	identityRegisterPath: 'Identity/Account/Register',
	identityManagePath: 'Identity/Account/Manage'
};

applicationPaths = {
	...applicationPaths,
	loginPathComponents: applicationPaths.login.split('/'),
	loginFailedPathComponents: applicationPaths.loginFailed.split('/'),
	registerPathComponents: applicationPaths.register.split('/'),
	profilePathComponents: applicationPaths.profile.split('/'),
	logOutPathComponents: applicationPaths.logOut.split('/'),
	loggedOutPathComponents: applicationPaths.loggedOut.split('/'),
	logOutCallbackPathComponents: applicationPaths.logOutCallback.split('/')
};

export const ApplicationPaths: ApplicationPathsType = applicationPaths;
