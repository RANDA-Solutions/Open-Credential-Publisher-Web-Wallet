// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  environment: 'LOCAL',
  recaptchaKey: '6Ldj1J4UAAAAAPeksFWPJLyFdgoLaKEb-3AjcjEm',
  idleTimer: 1140, // in seconds
  logoutTimer: 30, // in seconds
  updateTimer: 30, // in seconds
  timeoutMinutesCheck: 1, // in minutes
  version: '{BUILD_VERSION}'
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
