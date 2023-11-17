:: Blazor

:: Build
pushd src\Cashier

dotnet publish -c Release

popd

:: Deploy
netlify deploy --prod --dir=src\Cashier\bin\Release\net8.0\publish\wwwroot --message="Deploying to Prod"
