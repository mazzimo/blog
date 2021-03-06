Is very common nowadays to see web applications that give the user possibility to upload multiple images at the same time, and checking the progress.
I have recently came across this problem developing my wife's travel blog [In Giro Con Fluppa](http://www.ingiroconfluppa.com/Home/Index/En) so I made some research.
There are already some solutions available around (directives primarly), but none of them had exactly the kind of behaviour I expected.

Something similar to this:

![behaviour](http://i.giphy.com/xT1XGGIb0koqjSBLkk.gif \"behaviour of the upload\")


1) the user clicks a button and adds some images.

2) the user can then choose to upload the selected images or add other images.

3) the user can remove files to upload BEFORE starting the upload

4) the user can see a preview of the images BEFORE the upload.

5) the user can add images AFTER the first upload. if clicks again the upload button only the non-uploaded one will be uploaded.

6) each upload will be asyncronous (1 request per file)

first problem is that in angular 1.x file inputs don't have a *ng-model*. We need to build a directive that allows us to bind a multiple file input to a model, but in a **UNILATERAL WAY**.

That is needed because the *FileList* (the object behind the multiple file input field) is immutable (the list cannot be changed by removing single files). What we need to do instead is to use the file input field only to push files to an array stored in a variable in the scope.

Using an *Array* instead of the *Filelist* will allow us to remove single files.

Once we got the list of files, a click of a button will start to upload single images asynchronously. Progress will also be tracked. To achieve the tracking of the progress we need to use the *XHTMLrequest* object instead of *$http* as *$http* by default don't allow progress tracking.

But now enough talk, let's code :) 

**Disclaimer: this code is OVER-SIMPLIFIED. the actual code is not like this but what I wanted to show is the overall implementation.**

## Step 1 - Add image upload action (Asp.Net MVC)

First things first: we are now creating a really basic action that will upload the file on the filesystem.
         
    public class ImgUploadController : Controller
    {         
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase imageFile)
        {
            var localBaseFolder = \"~/Static/Uploads\";     
            if (imageFile.ContentLength > 0) {
              var name = Path.GetFileName(imageFile.FileName);
              var path = Path.Combine(Server.MapPath(localBaseFolder), name);
              file.SaveAs(path);
            }
            
            return EmptyResult();
        }
    }

## Step 2 - Add fileupload directive (Javascript)

Now we have to create a directive in AngularJS to use on the input type file. This directive will intercept the *onchange* event of the input field
and adds the files selected to the field of the scope indicated by the attrubte *file-model*.

    var FileUploadDirective = function ($parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var model = $parse(attrs.fileModel);
                var modelSetter = model.assign;

                element.bind('change', function () {
                    scope.$apply(function () {
                        var i, totCount, currValue;
                        currValue = model(scope);
                        totCount = element[0].files.length;
                        
                        //if the model is undefined, it sets it as empty array
                        if (currValue === undefined) {
                            currValue = [];
                        }
                        
                        //decorate each file selected with additional information
                        for (i = 0; i < totCount; i++) {
                            element[0].files[i].temporaryUrl = URL.createObjectURL(element[0].files[i]);
                            currValue.push(element[0].files[i]);
                        }
                        modelSetter(scope, currValue);
                    });
                });
            }
        };
    };
    
I've added a new property on each file selected (*temporaryUrl*) that is generated from the method [URL.createObjectURL](http://www.javascripture.com/URL). 
This property will contain a url that can be set as src of an image tag to allow the user to see a preview of the image BEFORE is uploaded.

## Step 3 - Add image upload factory to call action (Javascript)

Now we need to create a factory that will have the responsibility to call the action created above.

What I've implemented here is a version of a *promise pattern*. This factory will return an object with a function *then* that accepts 3 callbacks: 

The first callback will be called when *the upload has been successful*; 

The second callback will be called in case of errors; 

The third callback will be called *on progress*, accepting as first parameter the completion percentage.

    var ImageUploadFactory = function () {

        var imageUploadPromise = function (image) {

            var execute = function (success, error, progress) {

                var uploadUrl = '/ImgUpload';
                var data = new FormData();

                data.append('imageFile', image);

                var request = new XMLHttpRequest();
                request.onreadystatechange = function () {
                    if (request.readyState == 4) {
                        try {
                            var resp = request.response;
                            success(resp);
                        } catch (e) {
                            console.log(e);
                            error(e);
                        }
                    }
                };

                request.upload.onprogress = function (e) {
                    var percentage = Math.round(e.loaded * 100 / e.total);
                    progress(percentage);
                }

                request.open('POST', uploadUrl, true);
                request.send(data);

            }

            return {
                Then: execute
            };   

        }
        return {
            UploadPromise: imageUploadPromise
        };
    }

## Step 4 - Bind all together in the controller (Javascript)

Now all we need to do is to connect the wires into an angularJS controller. The view is the following:

    <label for=\"addFile\">Add Files</label>
    <input id=\"addFile\" style=\"display:none\" type=\"file\" file-model=\"filesToUpload\" multiple />
    <button ng-click=\"execUpload()\">Start Upload</button> 
    
    <table>
        <thead> 
            <tr> 
                <th>Preview</th> 
                <th>Completion</th>
                <th>Name</th>
                <th></th> 
            </tr> 
        </thead>
        <tbody>
            <tr ng-repeat=\"singleFile in filesToUpload track by $index\">
                <td><img ng-src=\"{{singleFile.temporaryUrl}}\" height=\"100\" /></td> 
                <td>
                    {{singleFile.percentageUploaded}}%    
                </td>
                <td>
                    {{singleFile.name}}
                </td>
                <td>
                    <button ng-click=\"remove($index)\">delete</button>
                </td> 
            </tr>
        </tbody>
    </table>

Using the *file-model* directive we are bounding the files selected on the file input to the scope variable *filesToUpload*. 
We are also hiding that input and using a visible label instead: that will be useful for styling purposes.

That's the controller:

    var thisPageController = function ($scope, ImageUploadFactory) {

        $scope.remove = function (idx) {
            $scope.filesToUpload.splice(idx, 1);
        }

        $scope.execUpload = function () {

            var i, totFiles, prom;

            var successHand = function (index) {
                return function(data) {
                    $scope.filesToUpload[index].percentageUploaded = 100;
                    $scope.$apply();                        
                };
            };
            
            var updatePerc = function (index) {
                return function (progress) {
                    $scope.filesToUpload[index].percentageUploaded = progress;
                    $scope.$apply();
                };
            };

            totFiles = $scope.filesToUpload.length;
            for (i = 0; i < totFiles ; i++) {
                if ($scope.filesToUpload[i].percentageUploaded === undefined) {
                    prom = ImageUploadFactory.UploadPromise($scope.filesToUpload[i]);
                    prom.Then(
                        successHand(i),
                        function (error) { /* handle error */ },
                        updatePerc(i));
                }
            }
        }

    }
        
    thisPageController.$inject = ['$scope', 'ImageUploadFactory'];

The controller has 2 methods: *remove* and *execUpload*. The *execUpload* will get any file added to the *filesToUpload* model (populated through the directive) and checks if that file was uploaded or not (using the *percentageUploaded* property if is set or not).
If the percentage is *undefined* we will create the promise and call the *then* function for each of those files with the proper callbacks.

Don't forget eventually to inject the dependencies (in this case the directive *fileModel* and the factory *ImageUploadFactory*) into the Angular app:

    var app = angular.module('app', []);
    
    ...
    
    app.directive('fileModel', ['$parse', FileUploadDirective]);
    app.controller('thisPageController', thisPageController);
    app.factory('ImageUploadFactory', ImageUploadFactory);

    ...

