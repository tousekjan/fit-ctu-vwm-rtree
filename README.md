# BI-VWM R-Tree

This project was implemented as semestral work for class Searching Web and Multimedia Databases  at Czech Technical University, [Faculty of Information Technology](https://fit.cvut.cz/en).


Authors: [@tousekjan](https://github.com/tousekjan/), [@prixladi](https://github.com/prixladi/)

# R-Tree

This document contains the project documentation for the semester project
for the BI-VWM course. It is an implementation of R-tree and web-based graphical
interface that allows to perform insert operations over the tree,
query and save operations.

## Project description
The goal of this work was to create a custom implementation of R-Tree. R-tree is
hyerarchical data structure, based on B+-tree. It is used for
indexing d-dimensional objects that are represented by their
Minimum Bounding Rectangle, the
MBR. [[1]](#1)

## Inputs

Inputs are made up of randomly generated data or data entered
by the user. However, thanks to the API created, it is possible to easily import into
any data into the tree, as long as it is points in a d-dimensional
space.Project description

## Output

The output is then the results of searches using different types of queries (Range
query, Nearest neighbour query...).

## Solution

The objects the tree works with are points in d-dimensional space.
These points are stored in the leaves of the tree. Each node of the tree is defined
by its MBR, which contains all nodes - if the node is internal, or points if it is a leaf.

## Implemented methods

We have implemented the following methods for the R-Tree.

### Insert

Allows you to insert a new point into the tree. We used the Insert algorithm,
described in the book R-trees: theory and applications. [[1]](#1)
we modified it for our purposes because we're working with points. When inserting
a new point into the tree, it depends on the node splitting algorithm used, if the
node fills up. We have implemented three different algorithms -
linear, quadratic and exponential. Comparison of these algorithms and
their advantages and disadvantages are described in more detail in section Experiments.

### Contains

Returns a bool value indicating whether the tree contains the specified point.

### RangeQuery

For a given point and range, returns all points that are within the given
range.

### NNQuery

Nearest Neighbor query. Returns the point that is closest to the specified
point.

### KNNQuery

Returns the $k$ closest points to the given point.

## Implementation

### Architecture

We decided to create a user interface for the R-tree in the form of
web application. This interface communicates with the API that the server
works with the R-tree.

![Diagram](https://user-images.githubusercontent.com/26005077/130950983-261169eb-1308-4eaf-81a7-8b561ea7246f.png)

### Used technologies

We wrote the backend and API in C\# using the .NET Core framework.
The web application is created in JavaScript. We have decided to use these technologies
because we have a good experience with them and because they provided us
all the functionality we needed. As a development environment we used
Visual Studio 2017 Community and Visual Studio Code.

### Used libraries

### Backend

Newtonsoft.Json
– Json framework for .NET [[3]](#3)

NETStandard.Library
– meta library for .NET core [[4]](#4)

### API

Microsoft.AspNetCore.All
– default API for ASP.NET Core applications [[5]](#5)

Microsoft.NETCore.App
– default API for ASP.NET Core applications [[6]](#6)

Microsoft.Extensions.PlatformAbstractions
– abstraction for .NET Framework, .NET Core and Mono API [[7]](#7)

Swashbuckle.AspNetCore
– Swagger tool to generate API documentation [[8]](#8)

### Frontend {#frontend .unnumbered}

React
– JavaScript knihovna pro tvorbu uživatelských rozhraní [[9]](#9)

Ant Design
– knihovna pro grafický design webových aplikací [[10]](#10)

Konva
– knihovna pro kreslení ve 2d [[11]](#11)

### Requirements
----------------------

To run the project the following needs to be installed

.NET Core SDK
– .NET platforma [[12]](#12)

Node.js
– JavaScript runtime [[13]](#13)

Node package manager
– správce JavaScript balíčků [[14]](#14)

### Run on Windows platform

### Backend

To build the backend part you need to move from the root directory
project to the API folder.

    cd ./server/Vwm.RTree.Api

And then run the powershell script BuildAndRun.ps1, which will build and run
API project in the Release configuration. The API is then available by default on
at <http://localhost:8080/>

    param(
    [switch] $noBuild)

    if(!$noBuild)
    {
        dotnet build --configuration Release
    }
    dotnet exec .\bin\Release\netcoreapp2.2\Vwm.RTree.Api.dll

### Frontend

To build the frontend, move to the front folder and run commands `npm install` and `npm start`. The web application is running by default at <http://localhost:3000/>

    cd ./front
    npm install
    npm start

## Example output

We have created a web application for inputting inputs and displaying outputs,
that allows you to use all the methods implemented on the R-tree. Below
we will describe the tabs that this application contains and add screenshots with
examples of inputs and outputs.

### Web Application

#### Add Point

Allows you to add points to the R-tree.
![AddPoint](https://user-images.githubusercontent.com/26005077/130951118-74df619d-9c1c-4af9-98db-8a725c721133.PNG)


### Queries

In the query interface, the user has two options to perform a query.

**Perform Query**
- performs a query over an R-tree, returns the results and displays the duration
    of the query

**Perform query with benchmark**
- execute the query over both the R-tree and a linear list and display both results and
    the duration of both queries for possible comparison

### Range Query

Allows you to specify the point and range in which the results should be located.
Returns and displays the searched results.
![RangeQuery](https://user-images.githubusercontent.com/26005077/130951141-3e49d3d1-e194-411e-b1c0-1466794707ee.PNG)


### NN Query (Nearest Neighbors)

Allows you to enter a point and find the closest point to it.
![NNQuery](https://user-images.githubusercontent.com/26005077/130951171-05808ea1-fdd6-4967-a8d4-3d142e1e50dd.PNG)


### KNN Query (K-Nearest Neighbors)

Allows you to specify a point and find the $k$ points that are closest to it.
![KNNQuery](https://user-images.githubusercontent.com/26005077/130951195-5303dbb6-9ffd-4edd-aac6-d2ecf1c991f6.PNG)


### Contains

Scans the R-Tree to see if it contains the specified point.

### Data

On the data tab, you can view the tree structure, including the points that
the tree contains. Here we can also find the Replace button
with fresh, which displays a form to create a new tree. In this
form, we can specify tree parameters such as the number of dimensions,
the maximum number of nodes in a node, the maximum number of points in a leaf
node. We can also randomly generate a specified number of nodes.

![Data](https://user-images.githubusercontent.com/26005077/130951323-ada714f3-f46d-46c5-8c09-3c0f5007d894.PNG)
![Replace](https://user-images.githubusercontent.com/26005077/130951364-8d375c3c-d931-467e-9e5a-f9701eb20d56.PNG)

With the Take a snapshot button we can save the current state of the tree to disk and with
the saved snapshot and work with it on the Snapshots tab.

### Visualize 

On the visualize tab is a canvas that graphically displays the tree points and
MBR nodes if the tree is dimension 2. This tab shows how the
points are stored in the nodes and how other nodes are sequentially extracted and
rectangles. In this visualization, you can observe the differences between the
methods used to divide the full nodes. In the example shown in the figure
Quadratic split is used.

![Visualize](https://user-images.githubusercontent.com/26005077/130951438-084db5aa-5dbf-4358-a408-f59e5351bb6e.PNG)

### Snapshots

The Snapshots tab is the interface for working with saved tree snapshots.
A snapshot of a tree can be loaded, deleted, or downloaded in JSON
format.

![Snapshot](https://user-images.githubusercontent.com/26005077/130951483-7e5bdf82-01db-479e-a73b-2d2647f8b87d.PNG)

## Experiments

When creating an R-tree, we can set several parameters.

- Number of dimensions

- Max number of children

- Max number of points in leaf

- Split policy

It is possible to change these parameters and monitor the speed of creation of the
tree or the speed of queries over the tree depending on these
parameters. We can also compare the query performance over the tree with
the speed of queries executed over the same data, but stored in a linear data structure.

### Speed of inserting points

In this test, we generated 100,000 random points, which we
inserted into the tree and measured the speed of this operation as a function of
the parameters Max number of children and Max number of points in leaf, which
are both set to the same value, given on the $x$-axis. Parameter
Number of dimensions was set to 3. We ran the test ten times and
averaged the results for higher precision.

In the graph we can see that while the Linear and Quadratic algorithms
increasing the number of Max children still run for almost the same amount of time,
Exponential algorithm slows down significantly as expected.

![Insert-1](https://user-images.githubusercontent.com/26005077/130951716-a72e5920-ba6d-4187-9474-74326385162c.png)


### Speed of queries

In the query speed test we set Number of dimensions to 2 and Max
number of children to 15. Then we randomly generated 2 000 000 points,
which were inserted into the tree. On this tree, we executed 1,000
random queries. Range query - selected random point and random range, KNN
query - selected a random point and searched for 100 nearest neighbors.

Each query was executed 1000 times and the times were summed for each
executions. We measured data structure creation time, run time
of the range query and the run time of the KNN query.

**Linear search**
- Creating a data structure for a linear search is very
    fast, while queries are slow compared to R-tree, and the
    especially KNN query

**R-tree - Linear split**
- it takes longer to create a tree, however we can depend on the speed of the queries.
    a significant improvement can be observed

**R-tree - Quadratic split**
- it takes the longest time to create the tree, but on the other hand the tree achieves
    the best results when comparing query speed

![Query-1](https://user-images.githubusercontent.com/26005077/130953211-add9414a-4659-4fdd-ba3d-615a2e15f40a.png)


## Discussion

The experiments turned out as expected. Inserting new points into the tree with
using an exponential algorithm to split nodes proved to be very
slow. We therefore expect that the algorithm will be most useful
quadratic.

Both Linear split and Quadratic split algorithms achieve very good results
compared to linear search. The biggest improvement is seen in
execution of KNN queries, which is roughly 3 times faster with Quadratic split,
than Linear split and even 25 times faster than linear search.

Possible improvements - we could also add a delete operation to the R-tree
element. Related to this operation is the operation to reduce the number of tree levels,
when the number of elements in a node falls below a certain threshold.

## Conclusion

The goal was to create an R-Tree, and we succeeded. Thanks to the created API
the R-tree can easily be used for some real data. So it should be
relatively easy to use this implementation in a real project,
that works with points in space. For both range queries and KNN queries
the tree is several times faster than a linear search and generally
both the tree and the web application return results correctly and quickly.

## References
<a id="1">[1]</a> 
MANOLOPOULOS, Yannis. 
R-trees: theory and applications.
London:
Springer, c2006. Advanced information and knowledge processing. ISBN
18-523-3977-2.

<a id="2">[2]</a>
Nearest Neighbour Queries [online]. [cit. 2019-05-07]. Available from:
<https://infolab.usc.edu/csci599/Fall2009/slides/Nearest%20Neighbor%20Queries%20Slides.pdf>

<a id="3">[3]</a>
Json.NET - Newtonsoft [online]. [cit. 2019-05-07]. Available from:
<https://www.newtonsoft.com/json>

<a id="4">[4]</a>
*NETStandard.Library* [online]. [cit. 2019-05-07]. Available from:
<https://www.nuget.org/packages/NETStandard.Library/>

<a id="5">[5]</a>
*Microsoft.AspNetCore.All* [online]. [cit. 2019-05-07]. Available from:
<https://www.nuget.org/packages/Microsoft.AspNetCore.All/>

<a id="6">[6]</a>
*Microsoft.NETCore.App* [online]. [cit. 2019-05-07]. Available from:
<https://www.nuget.org/packages/Microsoft.NETCore.App>

<a id="7">[7]</a>
*Microsoft.Extensions.PlatformAbstractions* [online]. [cit. 2019-05-07].
Available from:
<https://www.nuget.org/packages/Microsoft.Extensions.PlatformAbstractions/>

<a id="8">[8]</a>
*Swashbuckle.AspNetCore* [online]. [cit. 2019-05-07]. Available from:
<https://www.nuget.org/packages/Swashbuckle.AspNetCore/5.0.0-rc2>

<a id="9">[9]</a>
*React* [online]. [cit. 2019-05-07]. Available from: <https://reactjs.org/>

<a id="10">[10]</a>
*Ant Design* [online]. [cit. 2019-05-07]. Available from:
<https://ant.design/>

<a id="11">[11]</a>
*Konva* [online]. [cit. 2019-05-07]. Available from: <https://konvajs.org/>

<a id="12">[12]</a>
*Download .NET* [online]. [cit. 2019-05-08]. Available from:
<https://dotnet.microsoft.com/download>

<a id="13">[13]</a>
*Node.js* [online]. [cit. 2019-05-08]. Available from:
<https://nodejs.org/en/>

<a id="14">[14]</a>
*Node package manager* [online]. [cit. 2019-05-08]. Available from:
<https://www.npmjs.com/>

