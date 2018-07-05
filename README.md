ExtensionMethods
================

Some useful extension methods:

### Partition ###
Applied to a collection, this method takes a predicate method which it uses to partition the contents of the collection into two separate collections. The first collection will contain all elements that returned true as per the predicate, the second, all the elements that returned false.

Implementations included targeting:
* IEnumerable 
* Array
* IList

A Quick Example:

```C#
private void PartitionOnValue()
{
	int[] numbers = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
	var partitionedNums = numbers.Partition<int>((x) => x > 6);
	Console.WriteLine(string.Format("Number of elements > 6 is {0}", partitionedNums[0].Length));
	Console.WriteLine(string.Format("Number of elements <= 6 is {0}", partitionedNums[1].Length));
}

//	Returns:
//	Number of elements > 6 is 4
//	Number of elements <= 6 is 6
```

Note that you can also use split on more complex objects, for example to separate persons by gender:

```C#
private void PartitionOnGender(Ilist<Person> persons)
{
	...
	IEnumerable<Person>[] partitionedResults = persons.Partition<Person>(x => x.Gender == 'M');
	...
}
```

### Slice ###
Applied to a collection, this method returns a segment of the collection it was called on. You can specify the segment to be extracted not only by positve index values, but also negative index values, allowing segment range end points to be defined by their distance from the end of the collection. 

Note that when defining the ranges, Slice is inclusive for the starting index, and exclusive for the ending index

Implementations included targeting:
* Array
* IList

Example:

```C#
private void SliceNumbers()
{
	int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
	var segment1 = numbers.Slice<int>(2, 5);
	var segment2 = numbers.Slice<int>(2, -1);
	Console.WriteLine(segment1.Length); //contains { 3, 4, 5}
	Console.WriteLine(segment2.Length); //contains { 3, 4, 5, 6, 7, 8, 9 }
}
```

### Chunk ###
Applied to an an enumerable collection, this method splits it into a series of lists of the specified length, and returns and enumerable containing those lists.

Implementations included targeting:
* IEnumerable

Example:

```C#
private void ChunkArray()
{
	int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
	var chunkedData = numbers.Chunk<int>(2);
	Console.WriteLine(string.Format("The list of numbers was chunked into {0} pieces", chunkedData.Count()));
}

// Returns 
// The list of numbers was chunked into 5 pieces
```

### Init ###
Given a collection, Init will return another collection of the same type consisting of all elements contained in
the original collection except the very last element. The order of elements is preserved in the resulting collection

Example:

```C#
IList<int> list = new List<int>();

list.Add(1);
list.Add(2);
list.Add(3);
list.Add(4);

var newList = list.Init<int>();

// Returns
// { 1, 2, 3 }
```
Implementations included targeting:
* Array
* IList
* IEnumerable

### Tail ###
Given a collection, Tail will return another collection of the same type consisting of all the elements contained
the original collection, except the very first element. The order of elements is preserver in the resulting collection

Example:

```C#
IList<int> list = new List<int>();

list.Add(1);
list.Add(2);
list.Add(3);
list.Add(4);

var newList = list.Tail<int>();

// Returns
// { 2, 3, 4 }
```

Implementations included targeting:
* Array
* IList
* IEnumerable

### GetJaccardIndex ###
Get the Jaccard Similarity Coefficient (Jaccard Index) of an enumeration as it relates to another enumeration

Implementations included targeting:
* IEnumerable

Example:

```C#
private static void GetJaccardIndexValues()
{
	IList<Product> products = GetProducts();

	//Set comparison criteria used to generate each product's jaccard coefficient against
	List<Func<Product, bool>> critieria = new List<Func<Product, bool>>()
	{
		(x) => x.Name.Contains("Executive"),
		(x) => x.InStock == true,
		(x) => x.Category == "Office Supplies"
	};

	//Calculate and print out products and their associated value
	foreach (var product in products)
	{
		Console.WriteLine(
			string.Format("{0}:\n\t Jaccard Value: {1}",
			product.Name, product.GetJaccardIndex<Product>(critieria))
		);
	}
}

private static IList<Product> GetProducts()
{
	IList<Product> products = new List<Product>();

	products.Add(new Product() { Name = "Ballpoint Pen", Category = "Office Supplies", InStock = true, Price = 2.00 });
	products.Add(new Product() { Name = "Executive Chair", Category = "Furniture", InStock = false, Price = 149.95 });
	products.Add(new Product() { Name = "Notepad, White", Category = "Stationery", InStock = false, Price = 5.00 });
	products.Add(new Product() { Name = "Notepad, Yellow", Category = "Stationery", InStock = true, Price = 5.00 });
	products.Add(new Product() { Name = "Roll of Stamps", Category = "Mail Supplies", InStock = true, Price = 18.50 });
	products.Add(new Product() { Name = "Executive Desk", Category = "Furniture", InStock = true, Price = 1000.00 });
	products.Add(new Product() { Name = "Executive Ballpoint Pen", Category = "Office Supplies", InStock = true, Price = 20.00 });
	products.Add(new Product() { Name = "Executive DeskSet", Category = "Office Supplies", InStock = false, Price = 89.95 });
	products.Add(new Product() { Name = "Lead Pencils", Category = "Office Supplies", InStock = true, Price = 1.50 });
	products.Add(new Product() { Name = "Padded Mail Envelopes", Category = "Mail Supplies", InStock = false, Price = 24.99 });

	return products;
}

// Returns:
/*
Ballpoint Pen:
         Jaccard Value: 0.666666666666667
Executive Chair:
         Jaccard Value: 0.333333333333333
Notepad, White:
         Jaccard Value: 0
Notepad, Yellow:
         Jaccard Value: 0.333333333333333
Roll of Stamps:
         Jaccard Value: 0.333333333333333
Executive Desk:
         Jaccard Value: 0.666666666666667
Executive Ballpoint Pen:
         Jaccard Value: 1
Executive DeskSet:
         Jaccard Value: 0.666666666666667
Lead Pencils:
         Jaccard Value: 0.666666666666667
Padded Mail Envelopes:
         Jaccard Value: 0
*/
```

### JaccardIndexSort ###
Applied to a collection of  enumerable collections, this method returns the contents of the collection sorted by their Jaccard Similarity Coefficient in the order of 1.0 down to 0.0 (i.e., more similarity to less similarity) as it relates to another single collection.

Implementations included targeting:
* IEnumerable


### ToInfinite ###
Applied to an IEnumerable, this method returns a second IEnumberable that can be iterated
over indefinitely

Example:

```C#
private static void RunAsLongAsNeeded()
{
	IList<char> chars = new List<char>();
	chars.Add('H');
	chars.Add('e');
	chars.Add('l');
	chars.Add('l');
	chars.Add('o');
	chars.Add('W');
	chars.Add('o');
	chars.Add('r');
	chars.Add('l');
	chars.Add('d');
	chars.Add('!');

	int maxValue = (new Random()).Next(15, 100);

	var test = Enumerable.Range(0, maxValue)
						 .Zip(chars.ToInfinite(), (x,y) => (string.Format("{0}: {1}",x,y)));

	foreach (var item in test)
	{
		Console.WriteLine(item);
	}
}

// Returns
// 0: H
// 1: e
// 2: l
// 3: l
// 4: o
// 5: W
// 6: o
// 7: r
// 8: l
// 9: d
// 10: !
// 11: H
// 12: e
// 13: l
// 14: l
// 15: o
// 16: W
// 17: o
// 18: r
// ...
```

Implementations included targeting:
* IEnumerable