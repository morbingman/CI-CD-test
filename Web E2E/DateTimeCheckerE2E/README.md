# DateTimeChecker E2E — Topic 3: Web E2E Testing with Playwright

## Prerequisites
- Node.js installed (https://nodejs.org)
- The API running (`start-api.bat` from Topic 2)

## Project Structure
```
DateTimeCheckerE2E/
├── web/
│   └── index.html              ← Web UI that calls the API
├── tests/
│   └── datechecker.spec.ts     ← 14 Playwright test cases
├── playwright.config.ts        ← Config: headed, slowMo 300ms, serve web UI
├── package.json
├── run-e2e.bat                 ← One-click: install + run
└── README.md
```

## Running the Tests

### First time
```bat
run-e2e.bat
```
This installs npm packages, downloads Chromium, and runs all tests.

### After first time
```bat
npx playwright test --reporter=list
```

### Open the HTML report
```bat
npx playwright show-report
```

---

## Test Coverage

| Group | Tests |
|---|---|
| Happy Paths | 3 |
| Leap Year Edge Cases | 4 |
| Invalid Dates | 2 |
| Bad Input | 3 |
| UI Behaviour | 2 |
| **Total** | **14** |

---

## Demo Script

```
1. Make sure start-api.bat is running
2. run-e2e.bat                    ← browser opens, tests run visually
3. Watch Chromium fill in fields and assert results live
4. All 14 pass → green in terminal
5. npx playwright show-report     ← open HTML report
6. Break IsLeapYear in API → rerun → leap year tests go red
7. Fix → rerun → all green
```

## Key Config Choices
- `headless: false` — browser is visible during demo
- `slowMo: 300` — slows actions so audience can follow
- `webServer` — Playwright serves the web UI automatically, no manual step needed
