# DateTimeChecker API — Topic 2: API Testing Demo

## Overview
A minimal ASP.NET Core API wrapping the same `DateValidator` logic from Topic 1.
Exposes one endpoint so it can be tested with Postman.

## Project Structure
```
DateTimeCheckerAPI/
├── DateTimeCheckerAPI.csproj
├── DateValidator.cs                          ← same class from Topic 1
├── Program.cs                                ← minimal API, two endpoints
├── DateTimeCheckerAPI.postman_collection.json
├── start-api.bat
└── README.md
```

## Running the API

```bat
start-api.bat
```

Or manually:
```bash
dotnet run --urls "http://localhost:5000"
```

Then open **http://localhost:5000/swagger** to see and manually call the API.

---

## Endpoints

### POST /validate-date
Validates a day/month/year combination.

**Request:**
```json
{ "day": "29", "month": "2", "year": "2024" }
```

**Response 200 — valid date:**
```json
{ "isValid": true, "message": "29/02/2024 is a valid date." }
```

**Response 400 — invalid date:**
```json
{ "isValid": false, "message": "29/02/2023 is NOT a valid date." }
```

### GET /health
```json
{ "status": "ok" }
```

---

## Postman Demo

### Import the collection
1. Open Postman
2. Click **Import**
3. Select `DateTimeCheckerAPI.postman_collection.json`

### Run manually
Pick any request, hit **Send**, inspect the response body and status code.

### Run the full collection (the demo moment)
1. Click the collection name in the sidebar
2. Click **Run** → **Run DateTimeChecker API**
3. Watch each request go green or red with pass/fail counts

### Collection structure
| Folder | Requests |
|---|---|
| Health Check | 1 |
| Happy Paths | 3 (valid dates, boundary years) |
| Leap Year Edge Cases | 4 (2024 ✅, 2023 ❌, 2000 ✅, 1900 ❌) |
| Invalid Dates | 2 (April 31, November 31) |
| Bad Input | 4 (non-numeric, out of range, empty) |
| **Total** | **14** |

---

## Demo Script

```
1. start-api.bat                   → API running on localhost:5000
2. Open Postman → import collection
3. Send a few requests manually    → show request/response JSON
4. Run the full collection         → show 14/14 green
5. Open swagger UI                 → bonus: show auto-generated docs
```
