var GetCVFactory = function ($http, $q) {
    var funcGetCv = function (lang) {

        var deferredObject = $q.defer();

        $http.get(
            '/api/resume/' + lang
        ).
        success(function (data) {
            deferredObject.resolve({ success: true, resultData: data });
        }).
        error(function () {
            deferredObject.resolve({ success: false });
        });

        return deferredObject.promise;
    };


     return {
         GetCv: funcGetCv
     }
}

GetCVFactory.$inject = ['$http', '$q'];