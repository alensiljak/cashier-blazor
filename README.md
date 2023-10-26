# cashier-blazor
Cashier implemented in Blazor

# Development
Clone the repository. 

Prepare the JS client libraries. In `npm` directory, run
```sh
npm i
npm run build
```

Open a terminal window in `src/Cashier`. Execute 
```sh
dotnet watch
```
to utilize the hot reload for Blazor WebAssembly.

Other options are:
```sh
dotnet run
```
or
```sh
dotnet watch run
```

# Testing
TODO: bUnit + Playwright

# Publish

The JavaScript libraries need to be bundled first. In `npm` directory, run
```sh
npm run build
```

Build the optimized, Release, version of Cashier Blazor Webassembly app:
```sh
dotnet publish -c Release
```

The (temporary) production application is at

https://cashier-blazor.netlify.app
