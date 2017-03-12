It might happen during the development of your web application to have the need to load and save a file using only client side code.

a typical example might be to load a JSON file, parse it into an object, then save it back.

## Load a JSON file in javascript

Loading file from Javascript is relatively easy. 
All we need to do is an *input* field of type *file* (the ones used for uploading a file) and then use the *FileReader* object to retrieve the content and parse it into an object.

	<input type="file" id="fileText" />

Bear in mind that the *readAsText* method is asyncronous: this means that we need to set a callback with the code that needs to run after the file has finished to load.

	//this variable will receive the value read from the file;
	var variableToSet;
	
	var callBack = function(resObject) {
		variableToSet = resObject;
	};
	
	var loadFile = function(returnCallback) {
		return function(ev) {
			var elem = ev.target;
			var file = elem.files[0];
			var start = 0;
			var stop = (file.size - 1);

			var reader = new FileReader();

			reader.onloadend = function (evt) {
				if (evt.target.readyState === FileReader.DONE) {
					callBack(JSON.parse(evt.target.result));
				}
			};

			var blob = file.slice(start, stop + 1);
			reader.readAsText(blob);

		};
	};

	//attach the event on the "onchange" method
	document.getElementById("fileText").onchange = loadFile(callBack);

Here's a working [jsFiddle](https://jsfiddle.net/f0rnk2fn/) to work with.

## Save a JSON object to disk in Javascript

Saving a file is a bit trickyer, because for security reasons we cannot raise a download programmatically from the page. what we need to do is divided in 2 steps:

1) create a *Blob* file using the *JSON.stringify* method

2) create a temporary url from this Blob using URL *URL.createObjectURL* function

3) create a fake link assigning the temporary url and click it programmatically


	var saveFile = function(objToSave,fileName) {
		var data = new Blob([JSON.stringify(objToSave)], { type: 'text/plain' });
		var tempFileName = window.URL.createObjectURL(data);
	
		//create the fake link and click it
		var a = document.createElement('a');
		a.href = tempFileName;
		a.download = fileName;
		a.style.display = 'none';
		document.body.appendChild(a);
		a.click();
		delete a;
	}

Don't worry, I got a [jsFiddle](https://jsfiddle.net/fxuLp0z6/) for this one too ;)