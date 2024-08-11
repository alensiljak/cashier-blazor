/*
    Some basic tests that the app builds and works.
*/

import { test, expect } from '@playwright/test';

test('navigation on/off', async ({ page }) => {
    await page.goto('/');

    // Click on the menu button
    await page.getByRole('toolbar').getByRole('button').first().click();

    // confirm there is no Asset Allocation text on the page.
    var aaLink = page.getByRole('link', { name: 'Asset Allocation' });
    // await Expect(aaLink).ToBeHiddenAsync();
    // await expect(aaLink).not.toBeVisible();
    await expect(aaLink).not.toBeInViewport();
    // await Expect(aaLink).ToHaveCountAsync(0);
});

test('has title', async ({ page }) => {
    await page.goto('/');
    
    // Expect a title "to contain" a substring.
    await expect(page).toHaveTitle('Cashier');
});