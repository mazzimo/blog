Another weird thing to getting used to in javascript if you come from a c# background is **variable declaration**.

the scope of a variable in javascript is in fact just related to the function where is declared and **NOT** to the block. this means that if you declare a variable inside a if block like this:

    function example() {
        if(true) {
            var aaa = 5;
        }
        console.log(aaa);
    }

the result on the console will be *5*. this is due to a mechanism in Javascript called **hoisting** that makes Javascript treat all *var* declarations as if they were indicated at the top of the function. in fact the code above has the same behaviour as the following:

     function example() {        
        var aaa;

        if(true) {
            aaa = 5;
        }
        console.log(aaa);
    }

this is another good reason to declare every local variable at the top of the function to improve code readability.

another thing to mention about variable declaration is the *undefined* value. this is the value that a declared object has by default. that is different from *null* because it indicates that the variable never had an indication of whenever has a value in it or not. To put it straight,

*null* means

> I don't have a value

*undefined* means

> I don't know yet if I have a value or not.

this clearly differs from what we are familiar with in c#. in fact, when we declared a variable of a certain type we know there's already assigned a default value:

    //we know i is going to be 0, as 0 is the default value of int.
    int i;

    //o is going to be null as this is the default value of Object
    Object o;

additionally, when in c# we declare a variable using the *var* keyword, the compiler cannot know from which class need to get the default value. so it won't even compile if we don't assign a value on the declaration.

    //this doesn't compile in c#, giving 'Implicitly-typed variables must be initialized' error
    var aaa;

that's clearly because in c# there's no concept of *undefined value* .
