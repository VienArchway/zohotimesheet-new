# Zoho config

## Dev
create `.env.local`

```env
FE_ZOHO_CLIENT_ID=1000.1KQGLWBUATFDBEE1S19FVN59D339GN
FE_ZOHO_REDIRECT_URI=http://localhost:3000/auth/callback
FE_ZOHO_ACCESS_TYPE=offline
FE_ZOHO_CODE_RESPONSE_TYPE=code
FE_ZOHO_TOKEN_PATH=oauth/v2/token
FE_ZOHO_LOGIN_URL=https://accounts.zoho.com/oauth/v2/auth
FE_ZOHO_LOGIN_SCOPE=ZohoSprints.teams.READ+ZohoSprints.teamusers.READ+ZohoSprints.projects.READ+ZohoSprints.sprints.READ
```

## Prod

create `.env.production.local`

```env
FE_ZOHO_CLIENT_ID=1000.1KQGLWBUATFDBEE1S19FVN59D339GN
FE_ZOHO_REDIRECT_URI=http://localhost:3000/auth/callback
FE_ZOHO_ACCESS_TYPE=offline
FE_ZOHO_CODE_RESPONSE_TYPE=code
FE_ZOHO_TOKEN_PATH=oauth/v2/token
FE_ZOHO_LOGIN_URL=https://accounts.zoho.com/oauth/v2/auth
FE_ZOHO_LOGIN_SCOPE=ZohoSprints.teams.READ+ZohoSprints.teamusers.READ+ZohoSprints.projects.READ+ZohoSprints.sprints.READ
```

### Pages