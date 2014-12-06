ExtensionMethods
================

Some useful extension methods:

###Partition###
Applied to a collection, this method takes a predicate method which it uses to split the contents of the collection into two collections. The first collection will contain all elements that returned true, the second, all the elements that returned false.

Implementations included targeting:
* IEnumerable 
* Array
* IList

###Slice###
Applied to a collection, this method returns a segment of the collection it was called on. What is different about Slice is the fact that it accepts negative values for the end-at index, allowing for the endpoint of the returned segement to be defined by it's distance from the end of the collection.

Implementations included targeting:
* Array
* IList


###Chunk###
Applied to an an enumerable collection, this method splits it into a series of lists of the specified length, and returns and enumerable containing those lists.

Implementations included targeting:
* IEnumerable

