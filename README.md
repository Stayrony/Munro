# Munro

Provides information about munros and munro tops within Scotland.

Functions include:

- Filtering of search by hill category (i.e. Munro, Munro Top or either). If this information is
not provided by the user it should default to either. This should use the “post 1997”
column and if it is blank the hill should be always excluded from results.
- The ability to sort the results by height in meters and alphabetically by name. For both
options it should be possibly to specify if this should be done in ascending or descending
order.
- The ability to limit the total number of results returned, e.g. only show the top 10
- The ability to specify a minimum height in meters
- The ability to specify a maximum height in meters
- Queries may include any combination of the above features and none are mandatory.
- Suitable error handling for invalid queries (e.g. when the max height is less than the
minimum height)

## Usage

### The Structure of a CSV File
A CSV file has a fairly simple structure. It’s a list of data separated by commas.
You’d upload a file containing text like this:

```
Running No,DoBIH Number,Streetmap,Geograph,Hill-bagging,Name,SMC Section,RHB Section,_Section,Height (m),Height (ft),Map 1:50,Map 1:25,Grid Ref,GridRefXY,xcoord,ycoord,1891,1921,1933,1953,1969,1974,1981,1984,1990,1997,Post 1997,Comments
```

```
Upload File API:

POST /api/Files/UploadFile? HTTP/1.1
Host: localhost:5001
Content-type: multipart/form-data; 

Munros Query API:

POST /api/Munros? HTTP/1.1
Host: localhost:5001
Content-Type: application/json

Body example:

{
	"NameSortDirectionType" : 1,
	"HeightSortDirectionType" : 2,
	"HeightMaxMetres": 1000,
	"HeightMinMetres" : 900,
	"HillCategories" : [1, 2],
	"Limit" : 10
}

where:

 - NameSortDirectionType - sort the result by name in ascending or descending order. Can be:
        Ascending = 1,
        Descending = 2
        
 - HillCategories - filtering of search by hill category. Can be:
        TOP = 1,
        MUN = 2
        
 - HeightSortDirectionType - sort the results by height in meters in ascending or descending order. Can be:
        Ascending = 1,
        Descending = 2
        
 - HeightMaxMetres - specify a maximun height in meters
 
 - HeightMinMetres - specify a minimum height in meters
 
 - Limit - limit the total number of results returned.
```

### Possible result codes:

```
        Ok = 1,
        InternalServerError = 2,
        ValidationError = 3,
        ObjectMissing = 4,
        UnsupportedFileExtension = 5
```

### Examples of requests:

```
	//Get all TOP Munros and show the top 10:
	{
		"HillCategories" : [1],
		"Limit" : 10
	}
	
	//Get Munros between 900 and 1000 metres and sort by name in descending order
	{
		"NameSortDirectionType" : 2,
		"HeightSortDirectionType" : 2,
		"HeightMaxMetres": 1000,
		"HeightMinMetres" : 900,
	}
	
	//Get Munros between 950 and 995 metres and sort by name in descending order then by height in meters in ascending order
	{
		"NameSortDirectionType" : 2,
		"HeightSortDirectionType" : 1,
		"HeightMaxMetres": 995,
		"HeightMinMetres" : 950,
	}
```
