# DateTimeChecker — Unit Testing Demo (NUnit)

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) installed on Windows

---

## Project Structure

```
DateTimeChecker/
├── .config/
│   └── dotnet-tools.json             ← pins nunit3-console as a local tool
├── DateTimeChecker/
│   ├── DateValidator.cs              ← Extracted logic (no WinForms dependency)
│   └── DateTimeChecker.csproj
├── DateTimeChecker.Tests/
│   ├── LeapYearTests.cs              ← 18 tests for IsLeapYear
│   ├── DateValidationTests.cs        ← 19 tests for IsValidDate & GetMaxDaysInMonth
│   ├── FieldValidatorTests.cs        ← 18 tests for ValidateDay/Month/Year
│   ├── CheckDateTests.cs             ← 19 tests for the full CheckDate pipeline
│   └── DateTimeChecker.Tests.csproj
├── DateTimeChecker.sln
└── run-tests.bat                     ← ONE-CLICK: restore → build → run with color
```

---

## Running the Tests

### Option A — One click (recommended for demo)
Just double-click **`run-tests.bat`** or from a terminal at the solution root:

```bat
run-tests.bat
```

This restores packages, builds, then runs **nunit3-console** directly with `--labels=All`,
which prints every test name and result as it runs:

```
=> DateTimeChecker.Tests.LeapYearTests.IsLeapYear_DivisibleBy400_ReturnsTrue
=> DateTimeChecker.Tests.LeapYearTests.IsLeapYear_2400_DivisibleBy400_ReturnsTrue
...

Test Run Summary
  Overall result: Passed
  Test Count: 92, Passed: 92, Failed: 0, Inconclusive: 0, Skipped: 0
```

Failed tests print in red with the failure message inline.

### Option B — Manual (if you want to tweak flags)

```bash
# First time only: restore the nunit3-console tool
dotnet tool restore

# Build
dotnet build DateTimeChecker.Tests\DateTimeChecker.Tests.csproj -c Debug

# Run — key flag is --labels=All for per-test output
dotnet tool run nunit3-console DateTimeChecker.Tests\bin\Debug\net48\DateTimeChecker.Tests.dll --labels=All --noresult
```

---

## Test Coverage Summary

| Test Class | # Tests | What it covers |
|---|---|---|
| `LeapYearTests` | 18 | All 4 leap year rules + TestCase batch |
| `DateValidationTests` | 19 | Month lengths, Feb edge cases, century years |
| `FieldValidatorTests` | 18 | String parsing, boundaries, empty/whitespace/negative |
| `CheckDateTests` | 19 | Full pipeline from raw string input to result |
| **Total** | **74** | |

---

## Demo Script

```bash
# 1. Show all passing
run-tests.bat

# 2. Break something — open DateValidator.cs, change IsLeapYear to:
#       return false;
#    Save, then re-run:
run-tests.bat
#    → leap year tests go red with failure messages inline

# 3. Fix it, re-run → all 74 back to green
```
