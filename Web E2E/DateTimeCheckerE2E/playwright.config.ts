import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  timeout: 10000,
  retries: 0,

  // Run tests in a single browser for the demo
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],

  // Serve the web UI statically during tests
  webServer: {
    command: 'npx serve web -p 3000 --no-clipboard',
    port: 3000,
    reuseExistingServer: true,
  },

  use: {
    baseURL: 'http://localhost:3000',
    // false locally (demo), true in CI (no display server)
    headless: process.env.CI === 'true',
    // Slow down actions so audience can follow along (skipped in CI)
    launchOptions: {
      slowMo: process.env.CI === 'true' ? 0 : 300,
    },
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },

  reporter: [
    ['list'],
    ['html', { open: 'never', outputFolder: 'playwright-report' }],
  ],
});