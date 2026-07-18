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
    // Show browser during demo — change to false for headless CI
    headless: false,
    // Slow down actions so audience can follow along
    launchOptions: {
      slowMo: 300,
    },
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },

  reporter: [
    ['list'],       // per-test colored output in terminal
    ['html', { open: 'never' }],  // HTML report for CI/CD later
  ],
});
