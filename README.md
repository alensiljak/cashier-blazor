# cashier-blazor
Cashier implemented in Blazor

The live application is available at https://cashier-ii.alensiljak.eu.org

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

## Unit Testing
TODO: bUnit + Playwright

## E2E Testing
E2E tests are conducted using Playwright.

Go to the `tests` directory in the root of the repository and run the tests there.

### .Net integrated (deprecated)
Go to `src/Cashier.Tests.E2E` and run

```sh
set HEADED=1
set BROWSER=firefox

dotnet test
```

or

```sh
dotnet test --settings test.runsettings
```

Create tests with Codegen:
```sh
pwsh bin/Debug/netX/playwright.ps1 codegen localhost:5000
```

# Publish

Run the `publish.cmd` script. It will run the command below.

Build the optimized, Release, version of Cashier Blazor Webassembly app:
```sh
dotnet publish -c Release
```

Run Netlify CLI to deploy.
