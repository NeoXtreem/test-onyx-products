# Steps to Run Tests
1. Run `dotnet user-jwts create --scope "products" --role "auditor"` in the 'Onyx.Products' project folder.
1. Run `dotnet user-secrets set "Products:ApiKey" "[token]"` in the 'Onyx.Products.Tests' project folder where `[token]` is the token provided in the previous step.