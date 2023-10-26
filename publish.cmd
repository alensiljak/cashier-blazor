:: JavaScript
pushd src\npm

npm run build

popd

:: Blazor
pushd src\Cashier

dotnet publish -c Release

popd