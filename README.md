#Introduction

This is a basic predicate parser that takes in string and evaluates it from a source object.

This work is based off of a forked version of PredicateParser originally by Andreas Gieriet. See this Article:  http://www.codeproject.com/Articles/355513/Invent-your-own-Dynamic-LINQ-parser

#Release Notes


#### Features 
- Access property fields via dynamic objects 
- Added string built in predicates
- Added indexer support w/ square brackets for idictionary<string,object>
- Added support for booleans:  true and false

#### Caveats
- does not support real numbers with % operator
- does not support an indexer expression on dynamic types such as [IsUpgraded].  Workaround is to use [IsUpgraded] == true

 

#TODOS / Nice-to-Haves
- Add null types
- Support enum types
- Make the parser customizable at run-time to add new predicates
- Support multi arguments for predicates via function calls
- Smarter type conversion for math operations (float * float) => double for example
