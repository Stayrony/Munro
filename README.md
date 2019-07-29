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
