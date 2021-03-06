Compared to other languages like C# and Java, the value of the keyword *this* in javascript is everything but straight-forward. The key concept is that the value is not pre-determined by the position which the keyword is placed but when the related code is executed. Here's a little comparison to let me explain a bit better:

**1) in Javascript you can write *this* everywhere, in c# not.**

In c# in order to use the keyword *this* you need to know exactly when you write that you are dealing with an instance of an object. If you do otherwise, the code won't compile. This is because the value of *this* is stricted to the fact that refers to the instance of the object that calls the method or the property. Having an instance to begin with is a prerequisite to even using the keyword.

In Javascript instead, *this* is a value assigned at runtime every time a function is executed. Without any function to create scope, the default value of *this* represents the global object.

**2) Look at this code in C#**

    public class Example
    {
        class Foo
        {
            private string _value = \"a\";
            public string GetValue()
            {
                return this._value;
            }
        }

        public void ExampleMainFunction()
        {
            Foo foo = new Foo();
            Func<string> funcRef = foo.GetValue;
            string result1 = funcRef();
            string result2 = foo.GetValue();
        }
    }

We expect both *result1* and *result2* to return exactly the same (\"a\"), because *funcRef* has the handle of the function called from the instance *foo* created priorly.

Look instead at this code in Javascript:

    var foo = {
        value : 'a',
        getResult: function() {
            return this.value;
        }
    };

    function exampleMainFunction() {
        var funcRef = foo.getResult;
        var result1 = funcRef();
        var result2 = foo.getResult();
        console.log(result1);
        console.log(result2);
    }

Using the same logic, you would assume than even in this case the result is the same. But in this case *result1* is *undefined*. That's because when we assign the function *foo.getResult* to *funcRef* we are not telling

> take the function *getResult* executed from the instance *foo*.

but instead

> take the function *getResult*, Full Stop. No need to know from which instance.

When we actually invoke just *funcRef* we got actually no instance set up, so *this* becomes the global object, which in this case have no property called *value* to return.

When we call instead *foo.getResult()* is instead clear that we are calling the function from *foo*, so *this* is assigned to the instance *foo*.

To obtain the desired result, we can do the following trick...

**3) you can assign the value of *this* manually!**

Yep. You can literally do that, by executing the function *call* from any function. For example, if we want to bend the code above to have the expected behaviour (so return *a* even with *result1*) we can change the code to the following:

    var result1 = funcRef.call(foo);

*Call* is a function that accepts as first parameter the value we want to assign to *this*, and then the rest of the parameters (in our example, none). We are actually telling the system to \"Execute the function *funcRef* from the instance *foo*\"
