import { test, expect } from '@playwright/test';

// Helper to fill in the date fields and click Check
async function checkDate(page, day: string, month: string, year: string) {
  await page.getByTestId('input-day').fill(day);
  await page.getByTestId('input-month').fill(month);
  await page.getByTestId('input-year').fill(year);
  await page.getByTestId('btn-check').click();
  // Wait for result to appear
  await page.getByTestId('result').waitFor({ state: 'visible' });
}

// ── Setup ─────────────────────────────────────────────────────────────────────

test.beforeEach(async ({ page }) => {
  await page.goto('/');
});

// ── Happy Paths ───────────────────────────────────────────────────────────────

test('valid date shows success message', async ({ page }) => {
  await checkDate(page, '15', '6', '2024');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/valid/);
  await expect(result).toContainText('valid date');
});

test('first day of year is valid', async ({ page }) => {
  await checkDate(page, '1', '1', '2024');

  await expect(page.getByTestId('result')).toHaveClass(/valid/);
});

test('last day of year is valid', async ({ page }) => {
  await checkDate(page, '31', '12', '2024');

  await expect(page.getByTestId('result')).toHaveClass(/valid/);
});

// ── Leap Year Edge Cases ──────────────────────────────────────────────────────

test('Feb 29 in a leap year is valid', async ({ page }) => {
  await checkDate(page, '29', '2', '2024');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/valid/);
  await expect(result).toContainText('valid date');
});

test('Feb 29 in a non-leap year is invalid', async ({ page }) => {
  await checkDate(page, '29', '2', '2023');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/invalid/);
  await expect(result).toContainText('NOT a valid date');
});

test('Feb 29 in year 2000 (century leap) is valid', async ({ page }) => {
  await checkDate(page, '29', '2', '2000');

  await expect(page.getByTestId('result')).toHaveClass(/valid/);
});

test('Feb 29 in year 1900 (century non-leap) is invalid', async ({ page }) => {
  await checkDate(page, '29', '2', '1900');

  await expect(page.getByTestId('result')).toHaveClass(/invalid/);
});

// ── Invalid Dates ─────────────────────────────────────────────────────────────

test('April 31 is invalid', async ({ page }) => {
  await checkDate(page, '31', '4', '2024');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/invalid/);
  await expect(result).toContainText('NOT a valid date');
});

test('November 31 is invalid', async ({ page }) => {
  await checkDate(page, '31', '11', '2024');

  await expect(page.getByTestId('result')).toHaveClass(/invalid/);
});

// ── Bad Input ─────────────────────────────────────────────────────────────────

test('non-numeric day shows error', async ({ page }) => {
  await checkDate(page, 'abc', '6', '2024');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/invalid/);
  await expect(result).toContainText('number');
});

test('month out of range shows error', async ({ page }) => {
  await checkDate(page, '15', '13', '2024');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/invalid/);
  await expect(result).toContainText('between 1 and 12');
});

test('year below minimum shows error', async ({ page }) => {
  await checkDate(page, '15', '6', '999');

  const result = page.getByTestId('result');
  await expect(result).toHaveClass(/invalid/);
  await expect(result).toContainText('between 1000 and 3000');
});

// ── UI Behaviour ──────────────────────────────────────────────────────────────

test('Clear button resets all fields and hides result', async ({ page }) => {
  await checkDate(page, '15', '6', '2024');
  await expect(page.getByTestId('result')).toBeVisible();

  await page.getByTestId('btn-clear').click();

  await expect(page.getByTestId('input-day')).toHaveValue('');
  await expect(page.getByTestId('input-month')).toHaveValue('');
  await expect(page.getByTestId('input-year')).toHaveValue('');
  await expect(page.getByTestId('result')).toBeHidden();
});

test('Enter key triggers check', async ({ page }) => {
  await page.getByTestId('input-day').fill('15');
  await page.getByTestId('input-month').fill('6');
  await page.getByTestId('input-year').fill('2024');
  await page.keyboard.press('Enter');

  await expect(page.getByTestId('result')).toBeVisible();
  await expect(page.getByTestId('result')).toHaveClass(/valid/);
});
