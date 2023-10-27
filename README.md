# cashier-blazor
Cashier implemented in Blazor

# Development
Clone the repository. 

Prepare the JS client libraries. In `npm` directory, run
```sh
npm i
copy_libs.cmd
```
Then go to `src/Cashier` and execute 
```sh
dotnet watch
```
to utilize the hot reload for Blazor WebAssembly.

Other potential options are:
```sh
dotnet run
```
or
```sh
dotnet watch run
```

## Debugging
To debug the Blazor application, select Edge as the browser in Visual Studio. Run/Debug the Cashier
profile from Visual Studio.

# Testing
TODO: bUnit + Playwright

# Publish

Run the `publish.cmd` script. It will copy the JavaScript libraries and
run the command below.

Build the optimized, Release, version of Cashier Blazor Webassembly app:
```sh
dotnet publish -c Release
```

The (temporary) production application is at

https://cashier-blazor.netlify.app
