next chapter of our journey to Javascript is about **inheritance**.

anyone knows that inheritance in javascript is based on *prototypes*, but how this can be translated for people with knowledge of classical inheritance?

classical inheritance allows us to reuse some code on multiple classes. this means that each object of a particular class has all the properties declared in itself plus the members of its superclass.

    class SuperClass
    {
        public int Number { get;set;}
        public int ReturnTwice()
        {
            return this.Number * 2;
        }
    }

    class SubClass : SuperClass
    {
        public int ReturnSquare()
        {
            return this.Number * this.Number;
        }
    }

as we already know, in Javascript to recreate a behaviour similar to classes we are using functions.

each function has a specific member called *prototype*. the value of the prototype can be any object. this object is cloned inside a private member (called *\\___proto___*) that is part of any object in Javascript. when you are looking for a member of an object, first it checks if it's part of the object itself, then check if is part of the *\\___proto___* object.

*\\___proto___* itself is an object, so it contains another *\\___proto___* object to check if a member it's not found. this chain ends when *\\___proto___* is null.

    function SuperClass() {
        this.numb = 3;
        this.returnTwice = function() {
            return this.numb * 2;
        }
    }

    function SubClass() {
        this.returnSquare = function() {
            return this.numb * this.numb;
        }
    }
    SubClass.prototype = new SuperClass();

    var one = new SubClass();

    //returnSquare is part of the object, returns 9
    one.returnSquare();

    //returnTwice and numb are not part of the object, but part of the object contained in __proto__
    one.returnTwice();
    one.numb;

    //toString() is part of the __proto__'s __proto__ member, which is Object.prototype (the main base class). returns [object Object]
    one.toString();

    //aaaaaaa does not exists in anyone of the chained __proto__ members. returns undefined
    one.aaaaaaa;

a main difference is that **superclasses are immutable on classical inheritance, but variable on prototypical inheritance**. nothing stops us to replace the prototype of a function with another one. in this case, only the new instances of the object will have the new methods and members. old ones will continue to have the older subclasses.

    function SuperClassOne() {
        this.numb = 3;
        this.returnTwice = function() {
            return this.numb * 2;
        }
    }

    function SuperClassTwo() {
        this.numb = 3;
        this.returnTimes = function(times) {
            return this.numb * times;
        }
    }

    function SubClass() {
        this.returnSquare = function() {
            return this.numb * this.numb;
        }
    }
    SubClass.prototype = new SuperClassOne();
    var one = new SubClass();
    SubClass.prototype = new SuperClassTwo();
    var two = new SubClass();

    //both return 9
    one.returnSquare();
    two.returnSquare();

    //returns 6
    one.returnTwice();  

    //error: returnTimes is not a function
    one.returnTimes(5);

    //error: returnTwice is not a function
    two.returnTwice();  

    //returns 15
    two.returnTimes(5); 

