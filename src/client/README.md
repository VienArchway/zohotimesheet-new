# Zoho config

## Dev
create `.env.local`

```js
VITE_ZOHO_CLIENT_ID=1000.1KQGLWBUATFDBEE1S19FVN59D339GN
VITE_ZOHO_CLIENT_SECRET=161c801bdade439069fe07d6c552807e106710d480
VITE_ZOHO_REDIRECT_URI=http://localhost:3000/auth/callback
VITE_ZOHO_ACCESS_TYPE=offline
VITE_ZOHO_CODE_RESPONSE_TYPE=code
VITE_ZOHO_CODE_GRANT_TYPE=authorization_code

VITE_ZOHO_REDIRECT_LOGIN=https://accounts.zoho.com/oauth/v2/auth?scope=ZohoSprints.teams.READ+ZohoSprints.teamusers.READ+ZohoSprints.projects.READ&client_id=1000.1KQGLWBUATFDBEE1S19FVN59D339GN&response_type=code&redirect_uri=http://localhost:3000/auth/callback&access_type=offline
```

## Prod

create `.env.production.local`

```js
VITE_ZOHO_CLIENT_ID=1000.1KQGLWBUATFDBEE1S19FVN59D339GN
VITE_ZOHO_CLIENT_SECRET=161c801bdade439069fe07d6c552807e106710d480
VITE_ZOHO_REDIRECT_URI=http://<deploy-domain>:<port>/auth/callback
VITE_ZOHO_ACCESS_TYPE=offline
VITE_ZOHO_CODE_RESPONSE_TYPE=code
VITE_ZOHO_CODE_GRANT_TYPE=authorization_code

VITE_ZOHO_REDIRECT_LOGIN=https://accounts.zoho.com/oauth/v2/auth?scope=ZohoSprints.teams.READ+ZohoSprints.teamusers.READ+ZohoSprints.projects.READ&client_id=1000.1KQGLWBUATFDBEE1S19FVN59D339GN&response_type=code&redirect_uri=http://localhost:3000/auth/callback&access_type=offline
```