# zohotimesheet-new

1. build client

> dist

2. publish api

```
dotnet publish -c Release
```

3. Copy prod

copy dist folder and file in api/bin/Release into /prod

4. Build docker images 

```
docker build -t aci-zohotimesheet .
```

check: docker images

5. Run docker
6. Deploy

https://docs.microsoft.com/en-us/azure/container-instances/
