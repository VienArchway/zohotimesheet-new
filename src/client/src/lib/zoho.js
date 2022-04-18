const ZOHO_SETTINGS = {
    client_id: import.meta.env.FE_ZOHO_CLIENT_ID,
    client_secret: import.meta.env.FE_ZOHO_CLIENT_SECRET,
    redirect_uri: import.meta.env.FE_ZOHO_REDIRECT_URI,
    grant_type: import.meta.env.FE_ZOHO_CODE_GRANT_TYPE,
    login_url: import.meta.env.FE_ZOHO_LOGIN_URL,
    scope: import.meta.env.FE_ZOHO_LOGIN_SCOPE,
    response_type: import.meta.env.FE_ZOHO_CODE_RESPONSE_TYPE,
    token_path: import.meta.env.FE_ZOHO_TOKEN_PATH
}

export default ZOHO_SETTINGS