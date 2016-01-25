/// <reference path="jasmine.js" />
/// <reference path="ExampleClass.js" />

describe("will add 5 to number", function () {
    var res = mathLib.add5(10)
    it("should be 10 + 5 = 15", function () {
        expect(res).toBe(15);
    });
});