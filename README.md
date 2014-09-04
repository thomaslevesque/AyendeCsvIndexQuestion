Index based search in a CSV file
================================

My attempt at solving [Ayende's interview question on how to implement an index to quickly search a large CSV file](http://ayende.com/blog/167937/question-6-is-a-trap-a-very-useful-one).

Usage
-----

- Building the index

```
BuildIndex.exe <dataFileName> <fieldIndex>
```
`dataFileName` is the name of the CSV file. `fieldIndex` is the zero-based index of the column to build the index for. 

Example for the sample CSV file included with the source (the email column is at index 3):

```
BuildIndex.exe contacts.csv 3
```

- Querying with the index

```
Query.exe <dataFileName> <fieldIndex> [<searchTerm>]
```
`dataFileName` is the name of the CSV file. `fieldIndex` is the zero-based index of the column to search. `searchTerm` is the term to search (case-insensitive); if not specified, the program will run in interactive mode.

Example for the sample CSV file included with the source (the email column is at index 3):

```
Query.exe contacts.csv 3 jburns@katz.net
```

Output:
```
1184,Judy,Burns,jburns@katz.net,Paraguay,8-(581)467-8354
35942,James,Burns,jburns@katz.net,Thailand,0-(950)809-5042
89811,Jack,Burns,jburns@katz.net,Egypt,0-(003)901-3728
```

Notes
-----

This codes uses the hashcode of the indexed column value to determine a "bucket" (the number of buckets is fixed to 1024*1024 in this case); this bucket is directly mapped to a location in the buckets file (.idx.bkt), at which we read the offset of the first node in the actual index file (.idx). Since several values will fall into the same bucket, each node has a reference to the next node in the bucket (so nodes in the same bucket form a singly linked list).

This implementation is simple, but it has limitations:
- No fast way to update the index if a row is modified or deleted, because the nodes don't have a fixed size; addition of new rows at the end of the CSV file could be supported, but isn't implemented.
- The fact that the index nodes are organized in linked lists makes it very inefficient when many rows have the same value, causing a lot of seeking in the index file.
