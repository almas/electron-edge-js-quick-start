# electron-edge-js-quick-start

1. Install dependencies `npm install` or `yarn`
2. Build electron-edge-js via `npm run rebuild:debug` or `yarn rebuild:debug` // or `:release`
3. Build `src\QuickStart.sln` using Visual Studio 2019 or JetBrains Rider or run `dotnet build src/QuickStart.sln`
4. Ensure `NODE_ENV` is set to `development` when using a debug build (use `$env:NODE_ENV = 'development'` in powershell)
5. To run the app using .NET 5 use `npm start` or `npm run start:core`
