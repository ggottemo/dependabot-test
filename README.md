# Dependabot Test Solution

This is a sample .NET 9 solution designed to test Dependabot on GitHub. It includes intentionally outdated package versions to trigger Dependabot pull requests.

## Project Structure

- **DependabotTest.Api** - ASP.NET Core Web API with Swagger, Serilog, AutoMapper, and Newtonsoft.Json
- **DependabotTest.Library** - Class library with FluentValidation, AutoMapper, and Newtonsoft.Json
- **DependabotTest.BlazorWasm** - Blazor WebAssembly frontend application

## Central Package Management (CPM)

This solution uses Central Package Management, which allows all package versions to be managed in a single `Directory.Packages.props` file at the solution root.

### Outdated Packages (Intentionally)

The following packages are using older versions to trigger Dependabot updates:

| Package | Current Version | Latest Version |
|---------|----------------|----------------|
| Newtonsoft.Json | 12.0.3 | 13.x |
| Serilog | 2.10.0 | 4.x |
| Serilog.AspNetCore | 4.1.0 | 9.x |
| Serilog.Sinks.Console | 4.0.0 | 6.x |
| Serilog.Sinks.File | 5.0.0 | 6.x |
| AutoMapper | 10.1.1 | 13.x |
| AutoMapper.Extensions.Microsoft.DependencyInjection | 8.1.1 | 12.x |
| FluentValidation | 10.3.0 | 11.x |
| FluentValidation.DependencyInjectionExtensions | 10.3.0 | 11.x |
| Swashbuckle.AspNetCore | 6.2.3 | 6.9.x |

## How to Test Dependabot

1. **Push to GitHub**: Create a new repository on GitHub and push this solution:
   ```bash
   git init
   git add .
   git commit -m "Initial commit with outdated packages"
   git branch -M main
   git remote add origin <your-repo-url>
   git push -u origin main
   ```

2. **Enable Dependabot**: Dependabot is enabled by default on public repositories. For private repositories:
   - Go to repository Settings
   - Navigate to Security & analysis
   - Enable "Dependabot alerts" and "Dependabot security updates"

3. **Configure Dependabot (Optional)**: Create a `.github/dependabot.yml` file to customize Dependabot behavior:
   ```yaml
   version: 2
   updates:
     - package-ecosystem: "nuget"
       directory: "/"
       schedule:
         interval: "daily"
       open-pull-requests-limit: 10
   ```

4. **Wait for Dependabot**: Dependabot will automatically:
   - Scan your repository for outdated dependencies
   - Create pull requests to update each package
   - You should see multiple PRs (one per package or grouped by security updates)

## Running the Projects

### Web API
```bash
cd src/DependabotTest.Api
dotnet run
```

The API will be available at `https://localhost:<port>/api/products`

### Blazor WASM
```bash
cd src/DependabotTest.BlazorWasm
dotnet run
```

The Blazor app will be available at `https://localhost:<port>`

### Build the Entire Solution
```bash
dotnet build
```

### Restore Packages
```bash
dotnet restore
```

## Expected Dependabot Behavior

Once pushed to GitHub, you should see:

1. **Multiple Pull Requests**: Dependabot will create separate PRs for each outdated package (or grouped PRs if configured)
2. **Security Alerts**: If any packages have known vulnerabilities, you'll see security alerts
3. **Changelog Information**: Each PR will include changelog links and release notes
4. **Compatibility Score**: Dependabot shows compatibility scores based on how other repositories handled the same upgrade

## Features Demonstrated

- **CPM**: All package versions managed centrally in `Directory.Packages.props`
- **FluentValidation**: Product validation in the Library project
- **AutoMapper**: Mapping between Product and ProductDto
- **Serilog**: Structured logging in the Web API
- **Newtonsoft.Json**: JSON serialization in both API and Blazor projects
- **Swagger/OpenAPI**: API documentation

## Notes

- This solution is for **testing purposes only**
- Package versions are intentionally outdated
- Once Dependabot creates PRs, you can merge them to update to the latest versions
- Some package updates may require code changes due to breaking changes

## Cleaning Up

After testing Dependabot, you can update all packages to their latest versions by modifying `Directory.Packages.props` and running:

```bash
dotnet restore
dotnet build
```
