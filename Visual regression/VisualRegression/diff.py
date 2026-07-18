"""
diff.py
Compares baseline.png vs current.png and produces a diff image
highlighting changed pixels in red.

Usage:
    python diff.py
"""

import sys
import os
from PIL import Image, ImageChops, ImageEnhance
import math

def diff():
    # Always resolve paths relative to this script's location
    script_dir     = os.path.dirname(os.path.abspath(__file__))
    baseline_path  = os.path.join(script_dir, "screenshots", "baseline.png")
    current_path   = os.path.join(script_dir, "screenshots", "current.png")
    diff_path      = os.path.join(script_dir, "screenshots", "diff.png")

    # Check files exist
    for path in (baseline_path, current_path):
        if not os.path.exists(path):
            print(f"ERROR: {path} not found. Run capture.py first.")
            sys.exit(1)

    baseline = Image.open(baseline_path).convert("RGB")
    current  = Image.open(current_path).convert("RGB")

    # Resize current to match baseline if sizes differ
    if baseline.size != current.size:
        print(f"Warning: sizes differ {baseline.size} vs {current.size}, resizing current.")
        current = current.resize(baseline.size)

    # Pixel-level diff
    diff_image = ImageChops.difference(baseline, current)

    # Count changed pixels (use getdata or iterate directly — avoid deprecated call)
    changed = 0
    total   = baseline.width * baseline.height
    for r, g, b in diff_image.getdata():
        if r > 10 or g > 10 or b > 10:
            changed += 1

    pct = (changed / total) * 100

    # Build output: dim the baseline, overlay red on changed pixels
    dimmed = ImageEnhance.Brightness(baseline).enhance(0.4)
    result = dimmed.copy()

    baseline_pixels = baseline.load()
    current_pixels  = current.load()
    result_pixels   = result.load()

    for y in range(baseline.height):
        for x in range(baseline.width):
            r1, g1, b1 = baseline_pixels[x, y]
            r2, g2, b2 = current_pixels[x, y]
            delta = math.sqrt((r2-r1)**2 + (g2-g1)**2 + (b2-b1)**2)
            if delta > 15:
                result_pixels[x, y] = (220, 50, 50)  # red highlight

    result.save(diff_path)

    # Print summary
    print()
    print("=" * 50)
    print("  Visual Regression Report")
    print("=" * 50)
    if changed == 0:
        print("  PASS  No visual differences detected.")
    else:
        print(f"  FAIL  {changed:,} pixels changed ({pct:.2f}% of screen)")
        print(f"        Diff saved to: {diff_path}")
        print("        Red pixels = changed areas")
    print("=" * 50)
    print()

    # Auto-open the diff image
    if changed > 0 and os.path.exists(diff_path):
        os.startfile(diff_path)

if __name__ == "__main__":
    diff()
