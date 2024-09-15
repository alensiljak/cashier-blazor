:: Blazor

:: Build
::pushd src\Cashier
:: use .Net 8.0
dotnet publish -c Release -f net8.0

::popd

:: Deploy
npx netlify deploy --prod --dir=bin/Release/net8.0/publish/wwwroot --message="Publishing to Prod"
