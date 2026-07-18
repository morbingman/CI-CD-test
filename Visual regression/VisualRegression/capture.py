"""
capture.py
Takes a screenshot of the DateTimeChecker window and saves it.
Usage:
    python capture.py baseline    → saves screenshots/baseline.png
    python capture.py current     → saves screenshots/current.png
"""

import sys
import os
import time
import ctypes
import ctypes.wintypes

try:
    import pygetwindow as gw
    from PIL import ImageGrab
except ImportError:
    print("Missing packages. Run: pip install pygetwindow pillow")
    sys.exit(1)

# DPI aware so all coords are physical pixels
ctypes.windll.shcore.SetProcessDpiAwareness(2)

DWMWA_EXTENDED_FRAME_BOUNDS = 9

class RECT(ctypes.Structure):
    _fields_ = [("left", ctypes.c_long), ("top", ctypes.c_long),
                ("right", ctypes.c_long), ("bottom", ctypes.c_long)]

def get_visible_window_rect(hwnd):
    """
    Use DwmGetWindowAttribute to get the VISIBLE bounds of the window,
    excluding the invisible drop-shadow border that GetWindowRect includes.
    """
    rect = RECT()
    result = ctypes.windll.dwmapi.DwmGetWindowAttribute(
        hwnd,
        DWMWA_EXTENDED_FRAME_BOUNDS,
        ctypes.byref(rect),
        ctypes.sizeof(rect)
    )
    if result == 0:  # S_OK
        return rect.left, rect.top, rect.right, rect.bottom
    else:
        # Fallback to GetWindowRect
        print("DWM fallback to GetWindowRect")
        r = ctypes.wintypes.RECT()
        ctypes.windll.user32.GetWindowRect(hwnd, ctypes.byref(r))
        return r.left, r.top, r.right, r.bottom

def capture(name: str):
    script_dir = os.path.dirname(os.path.abspath(__file__))
    screenshots_dir = os.path.join(script_dir, "screenshots")
    os.makedirs(screenshots_dir, exist_ok=True)

    windows = gw.getWindowsWithTitle("Date Time Checker")
    if not windows:
        print("ERROR: DateTimeChecker window not found.")
        print("Make sure the app is running before capturing.")
        sys.exit(1)

    win = windows[0]
    win.activate()
    time.sleep(0.5)

    left, top, right, bottom = get_visible_window_rect(win._hWnd)
    print(f"Visible window rect: left={left}, top={top}, right={right}, bottom={bottom}")
    print(f"Window size: {right - left}x{bottom - top}px")

    screenshot = ImageGrab.grab(bbox=(left, top, right, bottom), all_screens=True)

    path = os.path.join(screenshots_dir, f"{name}.png")
    screenshot.save(path)
    print(f"Saved: {path} ({screenshot.width}x{screenshot.height}px)")

if __name__ == "__main__":
    if len(sys.argv) < 2 or sys.argv[1] not in ("baseline", "current"):
        print("Usage: python capture.py baseline|current")
        sys.exit(1)
    capture(sys.argv[1])
