///<reference path="../jasmine/jasmine.js" />
///<reference path="../Angular/angular.min.js"/>
///<reference path="../Angular/angular-mocks.js"/>
///<reference path="../Controllers/CvController.js"/>

//mocking the dependencies
var langid = "en";
var GetCVFactory = function ($q) {
    var funcGetCv = function (lang) {
        var deferredObject = $q.defer();
        var data = {
            WorkingExperience: [
		        {
		            CompanyName : 'Work Exp 1',
		            CompanyLogo : 'test.png',
		            CompanyWebsite : 'http://www.google.com/',
		            RoleName : 'Web Developer/Software Engineer',
		            DateStart : "2014-09-01T00:00:00.000Z",
		            DateEnd : null,
		            Description : '',
		            Skills : [
				        { 
				            Id: 'oop',
				            Desc: 'Object Oriented Programming'
				        }
		            ]
		        },
                {
                    CompanyName: 'Work Exp 2',
                    CompanyLogo: 'test.png',
                    CompanyWebsite: 'http://www.google.com/',
                    RoleName: 'Web Developer/Software Engineer',
                    DateStart: "2014-09-01T00:00:00.000Z",
                    DateEnd: null,
                    Description: '',
                    Skills: [
				        {
				            Id: 'designpatterns',
				            Desc: 'Design Patterns'
				        }
                    ]
                }
            ],
            SideProjects : [
		    {
		        Name : 'Side Project 1',
		        Logo : 'test.jpg',
		        PeriodDescription : 'From 2015',
		        Website : 'http://www.test.com',
		        ProjectType : 'Website',
		        Description : '',
		        Skills: [{
		            Id: 'csharp',
		            Desc: 'c#'
		        },
				{
				    Id: 'dotnet',
				    Desc: '.Net'
				}]
		    }],
            Skills: [
			{
				Id: 'csharp',
				Desc: 'c#',
				Percentage: 95
			},
			{
				Id: 'dotnet',
				Desc: '.Net',
				Percentage: 95
			},
            {
                Id: 'oop',
                Desc: 'Object Oriented Programming',
                Percentage: 95
            },
            {
                Id: 'designpatterns',
                Desc: 'Design Patterns',
                Percentage: 90
            }]
        };
        deferredObject.resolve({ success: true, resultData: data });
        return deferredObject.promise;
    };

    return {
        GetCv: funcGetCv
    }
};
GetCVFactory.$inject = ['$q'];

var AngularApp = angular.module('AngularApp', []);
AngularApp.value('langid', langid);
AngularApp.controller('CvController', CvController);
AngularApp.factory('GetCVFactory', GetCVFactory);

describe("CvController.addFilter", function () {
    
    beforeEach(module('AngularApp'));

    var $controller;

    beforeEach(inject(function (_$controller_) {
        $controller = _$controller_;
    }));

    it("should populate the filter property", function () {
        var ctrl = $controller('CvController');
        expect(ctrl.filter.length).toBe(0);
        ctrl.addFilter({ Id: 'oop' });
        expect(ctrl.filter.length).toBe(1);
    });

    it("should not add the same filter twice", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'oop' });
        expect(ctrl.filter.length).toBe(1);
        ctrl.addFilter({ Id: 'oop' });
        expect(ctrl.filter.length).toBe(1);
        ctrl.addFilter({ Id: 'abc' });
        expect(ctrl.filter.length).toBe(2);
    });

});

describe("CvController.removeFilter", function () {

    beforeEach(module('AngularApp'));

    var $controller;

    beforeEach(inject(function (_$controller_) {
        $controller = _$controller_;
    }));

    it("should not raise the error if filters are empty", function () {
        var ctrl = $controller('CvController');
        expect(ctrl.filter.length).toBe(0);
        ctrl.removeFilter({ Id: 'oop' });
        expect(ctrl.filter.length).toBe(0);
    });

    it("should remove if the object is the same", function () {    
        var ctrl = $controller('CvController');
        var element = { Id: 'oop' };
        var wrongElement = { Id: 'abc' };
        ctrl.filter = [element];
        ctrl.removeFilter(wrongElement);
        expect(ctrl.filter.length).toBe(1);
        ctrl.removeFilter(element);
        expect(ctrl.filter.length).toBe(0);
    });

});

describe("CvController.isWorkingExperienceInFilter", function () {

    beforeEach(module('AngularApp'));

    var $controller;

    beforeEach(inject(function (_$controller_) {
        $controller = _$controller_;
    }));

    var workingExperienceExample = {
        CompanyName: 'Work Exp 1',
        CompanyLogo: 'test.png',
        CompanyWebsite: 'http://www.google.com/',
        RoleName: 'Web Developer/Software Engineer',
        DateStart: "2014-09-01T00:00:00.000Z",
        DateEnd: null,
        Description: '',
        Skills: [
            {
                Id: 'oop',
                Desc: 'Object Oriented Programming'
            },
            {
            	Id: 'designpatterns',
            	Desc: 'Design Patterns'
            },
            {
                Id: 'abc',
                Desc: 'A B C'
            },
        ]
    };

    it("should return true if no skill in filter", function () {
        var ctrl = $controller('CvController');
        var result = ctrl.isWorkingExperienceInFilter(workingExperienceExample);
        expect(result).toBe(true);
    });

    it("should return true if skill with same id is present in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'oop' })
        var result = ctrl.isWorkingExperienceInFilter(workingExperienceExample);
        expect(result).toBe(true);
    });

    it("should return false if skill with same id is NOT present in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'notinexample' })
        var result = ctrl.isWorkingExperienceInFilter(workingExperienceExample);
        expect(result).toBe(false);

    });

});

describe("CvController.isPersonalProjectInFilter", function () {

    beforeEach(module('AngularApp'));

    var $controller;

    beforeEach(inject(function (_$controller_) {
        $controller = _$controller_;
    }));

    var personalProjectExample = {
        Name: 'Side Project 1',
        Logo: 'test.jpg',
        PeriodDescription: 'From 2015',
        Website: 'http://www.test.com',
        ProjectType: 'Website',
        Description: '',
        Skills: [{
            Id: 'csharp',
            Desc: 'c#'
        },
        {
            Id: 'dotnet',
            Desc: '.Net'
        }]
    };

    it("should return true if no skill in filter", function () {
        var ctrl = $controller('CvController');
        var result = ctrl.isPersonalProjectInFilter(personalProjectExample);
        expect(result).toBe(true);
    });

    it("should return true if skill with same id is present in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'csharp' });
        var result = ctrl.isPersonalProjectInFilter(personalProjectExample);
        expect(result).toBe(true);
    });

    it("should return false if skill with same id is NOT present in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'oop' });
        var result = ctrl.isPersonalProjectInFilter(personalProjectExample);
        expect(result).toBe(false);
    });

});

describe("CvController.isSkillInFilter", function () {

    beforeEach(module('AngularApp'));

    var $controller;

    beforeEach(inject(function (_$controller_) {
        $controller = _$controller_;
    }));

    var skillExample = {
        Id: 'oop',
        Desc: 'Object Oriented Programming'
    };

    it("should return true if no skill in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'oop' });
        var result = ctrl.isSkillInFilter(skillExample);
        expect(result).toBe(true);
    });

    it("should return true if skill with same id is present in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'oop' });
        var result = ctrl.isSkillInFilter(skillExample);
        expect(result).toBe(true);
    });

    it("should return false if skill with same id is NOT present in filter", function () {
        var ctrl = $controller('CvController');
        ctrl.addFilter({ Id: 'aaa' });
        var result = ctrl.isSkillInFilter(skillExample);
        expect(result).toBe(false);
    });

});