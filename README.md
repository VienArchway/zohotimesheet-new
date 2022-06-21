# zohotimesheet-new

[![Build to prod and deploy to Azure container registry](https://github.com/VienArchway/zohotimesheet-new/actions/workflows/ci.yml/badge.svg?branch=main&event=pull_request)](https://github.com/VienArchway/zohotimesheet-new/actions/workflows/ci.yml)

## Setup

### Frontend

go to /src/client

1. Install package

```js
npm install
```

2. Setting env

- create 2 files `.env.local` and `.env.production` in /src/client
```env
// .env.local
FE_ZOHO_CLIENT_ID=1000.1KQGLWBUATFDBEE1S19FVN59D339GN
FE_ZOHO_REDIRECT_URI=http://localhost:3000/auth/callback
FE_ZOHO_ACCESS_TYPE=offline
FE_ZOHO_CODE_RESPONSE_TYPE=code
FE_ZOHO_TOKEN_PATH=oauth/v2/token
FE_ZOHO_LOGIN_URL=https://accounts.zoho.com/oauth/v2/auth
FE_ZOHO_LOGIN_SCOPE=ZohoSprints.teams.READ+ZohoSprints.teamusers.READ+ZohoSprints.projects.READ+ZohoSprints.sprints.READ+aaaserver.profile.READ

FE_API_URL=http://localhost:5000
```

```env
// .env.production
FE_ZOHO_CLIENT_ID=1000.Z1JHOXVXLJRC1BXDJX7HQJUVGSDKED
FE_ZOHO_REDIRECT_URI=https://zohotimesheetaw.azurewebsites.net/auth/callback
FE_ZOHO_ACCESS_TYPE=offline
FE_ZOHO_CODE_RESPONSE_TYPE=code
FE_ZOHO_TOKEN_PATH=oauth/v2/token
FE_ZOHO_LOGIN_URL=https://accounts.zoho.com/oauth/v2/auth
FE_ZOHO_LOGIN_SCOPE=ZohoSprints.teams.READ+ZohoSprints.teamusers.READ+ZohoSprints.projects.READ+ZohoSprints.sprints.READ+aaaserver.profile.READ

FE_API_URL=http://0.0.0.0:80
```

3. Run app

```js
npm run dev
```

### Backend

1. Build project

```js
dotnet restore
dotnet build
```

2. Setup environment

Edit `userName` to your name in `appsettings.Development.json` and `appsettings.Production.json`

3. Install extension

Vscode: https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account

Read more: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme#environment-variables

## Deploy

1. build client

```js
npm run build
```

2. publish api

```
dotnet publish -c Release
```

3. Prepare build docker

- copy file from api/bin/Release/Publish to /prod
- copy folder `dist` from src/client/dist to /prod
- verify /prod
```
dotnet api.dll
```

4. Build docker images 

> name: aci-zohotimesheet
> tag: v1

```
docker build -t aci-zohotimesheet:v1 .
```

5. Run docker

```
docker run -d -p 5000:5000 aci-zohotimesheet:v1
```