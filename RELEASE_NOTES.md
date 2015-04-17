
## Release Notes

### 0.2.0
- fix unintended bugs
- indexer expression that returns a boolean now works
- able to use indexer on dynamic objects. however if member names includes spaces, it will not work.  Works for the following:  "Price > 30.00" or "[Price] > 30.00" for dynamic types.  

### 0.1.0
 
#### Features
- Access property fields via dynamic objects 
- Added string built in predicates
- Added indexer support w/ square brackets for IDictionary<string,object>
- Added support for booleans:  true and false

#### Caveats
- does not support double quotes
- does not support real numbers with % operator
- does not support an indexer expression that returns a boolean from dynamic objects.  For example, "[IsUpgraded]".  Workaround is to use "[IsUpgraded] == true"