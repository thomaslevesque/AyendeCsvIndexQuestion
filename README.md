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
