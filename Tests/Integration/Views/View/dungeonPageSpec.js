﻿'use strict'

var CommonTestFunctions = require('./../commonTestFunctions.js');

describe('Dungeon Page', function () {
    var commonTestFunctions = new CommonTestFunctions();

    beforeEach(function () {
        browser.get(browser.baseUrl + '/Dungeon');
    });

    afterEach(function () {
        browser.manage().logs().get('browser').then(commonTestFunctions.assertNoErrors);
    });

    it('should have a header', function () {
        expect(element(by.css('h1')).getText()).toBe('DungeonGen');
    });
});