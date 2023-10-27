:: JavaScript
::pushd src\npm
::npm run build
::call copy_libs.cmd
::popd

:: Blazor
pushd src\Cashier

dotnet publish -c Release

popd