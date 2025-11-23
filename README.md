# Backend.Tests

Unit test project for validating the functionality of the ASP.NET Core API in `Backend/`.  
This test suite ensures correctness of authentication logic, including registration, login, and protected endpoints.

## 🧪 Technologies Used

- ✅ [xUnit](https://xunit.net/) — Unit testing framework
- ✅ [FluentAssertions](https://fluentassertions.com/) — Expressive assertions
- ✅ [.NET 8](https://dotnet.microsoft.com/en-us/) — Target framework
- ✅ [Microsoft.AspNetCore.Mvc](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc) — For controller testing

## 📁 Project Structure

Backend.Tests/ 
├── AuthControllerTests.cs # Unit tests for AuthController 
├── Backend.Tests.csproj # Test project file


## 🧪 What’s Tested

- ✅ `Register()` — Valid and duplicate username scenarios
- ✅ `Login()` — Valid credentials, wrong password, and nonexistent user
- ✅ `Me()` — Authorized user context simulation

## 🚀 How to Run Tests

### 1. Restore dependencies

```bash
dotnet restore

Build the test project
dotnet build Backend.Tests

Run all tests
dotnet test Backend.Tests

Run with detailed output
dotnet test --logger "console;verbosity=detailed"

🧠 Notes
The test suite uses in-memory storage (static List<User>) for simplicity.

No mocking framework is required since the controller logic is self-contained.

JWT generation is verified by checking the presence of a token string.

📄 License
MIT © 2025 Wuttachai Chumyen
