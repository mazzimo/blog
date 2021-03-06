Using the *new* keyword in Javascript is really confusing at first sight: after all Javascript doesn't have a concept of classes, so why having a *new* keyword?

In c# you can use the *new* keyword ONLY for some specific methods called *constructors*: those are functions that have the following restrictions:

1) Got the same name as the Class that contains the function
2) Cannot be static
3) Cannot return any value

but what's the real **reason** to use a constructor? basically is to initialize an object with some specific properties and methods.

In c# those members are described inside the definition of a class. When you call a constructor of a specific class you are doing just that: create an object with the same exact members defined in the class definition. 

In Javascript we cannot define classes, but we can take advantage of the fact that is a **weakly typed** language. This allows us to describe those members inside a function.

    function Example() {
        this.intMember = 1;
        this.stringMember = 'abc';
        this.funcMember = function(addThisToIntMember) {
            this.intMember = this.intMember + addThisToIntMember;
        }
    }

to make a very loose comparison, in Javascript a function can act **as both a Class and a Constructor at the same time**.

if you call a function with the *new* keyword, the following steps will be executed:
1) a object is created;
2) all the members described in the *prototype* property of the called function are copied to the new object (I'll go deeper into prototypes in a future post);
3) the function is executed and the created object associated with the keyword *this* ([check out my previous post](/javascript-what-is-this) to have more clarification about the *this* keyword)
4) returns the object;

translated in verbose code the *new* operator behaves like the following code:

    //this line
    var a = new Example();
    

    //has similar behaviour as this
    var b = Object.create(Example.prototype);
    Example.call(b);

**BEAR IN MIND** that I've used the word **SIMILAR**, not **EQUAL**. in fact the code above is meant to only grasp the concept but there's more than that. If you replace the *new* operator with the *Object.create* function you won't be able to use things like the *instanceof* operator for example:

    var a = new Example();
    var b = Object.create(Example.prototype);
    Example.call(b);

    console.log(a instanceof Example) //true;
    console.log(b instanceof Example) //false;
