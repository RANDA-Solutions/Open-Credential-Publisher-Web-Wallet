window["oidc-config"] = {
    authority: `${window.location.origin}`,
    client_id: 'ocp-wallet-client',
    loadUserInfo: true,
    lockSkew: 0,
    post_logout_redirect_uri: `${window.location.origin}/access/login`,
    redirect_uri: `${window.location.origin}/callback`,
    response_type: 'code',
    scope: 'openid profile roles offline_access', // 'openid profile offline_access ' + your scopes
    automaticSilentRenew: true,
    silent_redirect_uri: `${window.location.origin}/silent-renew.html`,
    accessTokenExpiringNotificationTime: 30,
    userStore: new WebStorageStateStore({
        store: window.localStorage
    }),
    filterProtocolClaims: true,
};