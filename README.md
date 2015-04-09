# UglyToad.DataTable #

## Introduction ##

This is a class library that adds a converter for mapping a DataTable, like those returned by a stored procedure, to a list of objects of the type requested by the user.

## Quickstart ##

For the simplest possible conversion all you need is to include:

	using UglyToad.DataTable

In your code file. The conversion can then be performed as follows:

	DataTableConverter.Convert<T>(dataTable);

Where ```Convert<T>``` is a static method on the DataTable converter and ```T``` is the type of your target class such as:

	class Person
	{
		public string Name { get; set; }
		public DateTime BirthDate { get; set; }
	}

You can provide additional mappings to column names using the ```ColumnMapping``` attribute included in:

	using UglyToad.DataTable.Types

This attribute takes the name of the column to match the property to (it is case insensitive):

	[ColumnMapping("Date-of-birth")]
	public DateTime BirthDate { get; set; }

The mapper can also map to encapsulated classes with no public constructor and can map to properties with private setters.

## Settings ##

Any argument to the ```DataTableConverter``` can be overloaded to include the ```DataTableParserSettings``` which can set a variety of behaviours.

For example the ```Resolver``` setting selects how the converter should process the conversion. The ```Resolver.Delegate``` is faster than ```Resolver.Default``` but less well tested. The ```Resolver.Parallel``` is faster still but does not respect order of the input table.

## Defaults ##

The converter tries to assume some defaults when performing a mapping. For example if a cell contains a ```DbNull``` for a non-nullable value type and the settings contains:

	AllowDbNullForNonNullableTypes = true

Then the default for that value type will be used.

Where the conversion is not generally supported, such as ```int``` to ```bool``` then the converter assumes a value of 1 to be true and all other values to be false.

Where a conversion cannot take place a ```NotImplementedException``` is thrown rather than an ```InvalidCastException```. This is because the intent is to extend the behaviour of the converter to avoid as far as possible invalid casts taking place.