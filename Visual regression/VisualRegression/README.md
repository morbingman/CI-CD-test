# Visual Regression — Topic 5: Screenshot Diff on DateTimeChecker UI

## Prerequisites
- Python 3.13 installed
- DateTimeChecker.exe (the WinForms app from the original project)

## Project Structure
```
VisualRegression/
├── capture.py          ← takes a screenshot of the running app window
├── diff.py             ← compares baseline vs current, outputs diff image
├── run-visual.bat      ← interactive one-click runner
├── screenshots/        ← created automatically
│   ├── baseline.png
│   ├── current.png
│   └── diff.png        ← red pixels = changed areas
└── README.md
```

## Dependencies (auto-installed by bat)
```
pip install pillow pygetwindow
```

---

## Demo Script

### Step 1 — Capture baseline
1. Build and run the original `DateTimeChecker.exe`
2. Run `run-visual.bat` → choose **1**
3. `screenshots/baseline.png` is saved

### Step 2 — Make a UI change
Open `MainForm.Designer.cs` and make a visible change, e.g:

**Change button color:**
```csharp
// Find the btnCheck setup block and add:
this.btnCheck.BackColor = System.Drawing.Color.Red;
this.btnCheck.ForeColor = System.Drawing.Color.White;
```

**Or change button text:**
```csharp
this.btnCheck.Text = "Validate";
```

Rebuild and run the modified app.

### Step 3 — Capture current
Run `run-visual.bat` → choose **2**

### Step 4 — Run diff
Run `run-visual.bat` → choose **3**

The diff image opens automatically showing:
- **Dark areas** = unchanged pixels
- **Red pixels** = changed areas

### Step 5 — Revert and confirm
Revert the UI change, rebuild, capture current again, run diff → PASS, no red pixels.

---

## Command line (alternative to bat)
```bash
python capture.py baseline
python capture.py current
python diff.py
```

---

## Demo Narrative
> "Visual regression testing catches unintended UI changes. I take a baseline
> screenshot of the known-good UI. After any code change, I capture again and
> diff — the red pixels show me exactly what changed visually, even if the
> code change was buried deep in a Designer file."
