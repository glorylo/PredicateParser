#Introduction

I wanted to write simple expressions without the full blown solution of Dynamic LINQ.  In my digging, I found some of the other libraries had more extensive capabilities more like Javascript's eval().   

I wanted something simpler and a basic language to declare predicates on some object.  Hence, this solution.  It is a basic predicate parser that takes in string and evaluates it onto a source object.  

This work is originally based from PredicateParser by Andreas Gieriet. See this Article:  http://www.codeproject.com/Articles/355513/Invent-your-own-Dynamic-LINQ-parser

# Usage

Example of using the parser:

```cs
var john = new Person()  { Name = "John", Age = 35 };
    var expression = @"Age > 30";
    var predicate = PredicateParser<Person>.Parse(expression).Compile();
    var result = predicate(john);
    Console.WriteLine("result = " + result);

// returns "result = true"  
```


#TODOS / Nice-to-Haves
- support escaping double quotes
- Add null types
- Support enum types
- Make the parser customizable at run-time to add new predicates
- Support multi arguments for predicates via function calls
- Smarter type conversion for math operations (float * float) => double for example


Glory