# Tests

This directory contains Playwright tests.

https://playwright.dev/

The tests are in `tests` subdirectory.

# Prerequisites
Install Playwright.

```sh
npm i
```

Run the web application separately.

# Generating Tests
Run CodeGen:

```sh
npx playwright codegen [--browser=firefox]
```

# Running Tests

The easiest way is to run from VS Code.

Alternatively, the tests can be run from the console.

```sh
npx playwright test
```

# Report
```sh
npx playwright show-report
```