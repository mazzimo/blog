var CvController = function (GetCVFactory, langid) {

    var that = this;
    var result = GetCVFactory.GetCv(langid);
    result.then(function (result) {
        if (result.success) {
            that.cv = result.resultData;
        }
    });

    this.filter = [];

    this.addFilter = function (skillFilter) {
        if (!that.filter.some(function (sk) { return sk.Id == skillFilter.Id })) {
            that.filter.push(skillFilter);
        }
    };

    this.hasFilter = function () {
        return that.filter.length > 0;
    }

    this.removeFilter = function(skillFilter) {
        var i = that.filter.indexOf(skillFilter);
        if (i > -1)
            that.filter.splice(i,1);
    }

    this.clearFilter = function() {
        that.filter = [];
    };

    this.isWorkingExperienceInFilter = function (workingExperience) {
        if (that.filter.length < 1)
            return true;

        return workingExperience.Skills.some(function (v) {
            return that.filter.some(function (f) { return f.Id == v.Id });
        });
    };

    this.isPersonalProjectInFilter = function (personalProject) {
        if (that.filter.length < 1)
            return true;

        return personalProject.Skills.some(function (v) {
            return that.filter.some(function(f) { return f.Id == v.Id });
        });
    };

    this.isSkillInFilter = function (skill) {
        if (that.filter.length < 1)
            return true;

        return that.filter.some(function (f) { return f.Id == skill.Id });
    };
    
}

// The $inject property of every controller (and pretty much every other type of object in Angular) needs to be a string array equal to the controllers arguments, only as strings
CvController.$inject = [ 'GetCVFactory', 'langid'];