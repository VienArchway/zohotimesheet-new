# zohotimesheet-new

zoho state: 67452389162534

1. build client

> dist

2. publish api

```
dotnet publish -c Release
```

3. Copy prod

- Edit `program.css` in src/api

```c#
// Set up build docker
app.Urls.Add("http://0.0.0.0:5000");
```

- Edit `vite.config.js` in src/client

```javascript
server: {
    proxy: {
      // '/api': 'http://localhost:5000' // config build local dev
      '/api': 'http://0.0.0.0:5000' // config build docker
    }
}
```

- Run dotnet publish in src/api
  - copy file in api/bin/Release/Publish
- Run npm run build in src/client
  - copy folder `dist` in src/client/dist

- Test prod/

```
dotnet api.dll
```

4. Build docker images 

> name: aci-zohotimesheet
> tag: v1

```
docker build -t aci-zohotimesheet:v1 .
```

```
docker images
docker image rm aci-zohotimesheet:v1
```

5. Run docker

```
docker run -d -p 5000:5000 aci-zohotimesheet:v1
```

6. Deploy

https://docs.microsoft.com/en-us/azure/container-instances/
