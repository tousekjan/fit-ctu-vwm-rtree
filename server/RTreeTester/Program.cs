using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vwm.RTree;
using Vwm.RTree.Api;

namespace RTreeTester
{
  class Program
  {
    static void Main(string[] args)
    {
      /*
      var tree = new Tree(null);
      var pointHolder = new PointHolder();
      var random = new Random();
      var stopwatch = new Stopwatch();

      stopwatch.Start();
      for (var i = 0; i < 100_000; i++)
      {
        var point = new Point(random.Next() % 1_000_000, random.Next() % 1_000_000);
        tree.Insert(point);
        pointHolder.Insert(point);
      }
      stopwatch.Stop();

      Console.WriteLine("Init structures took: " + stopwatch.Elapsed);

      
      var middle = new Point(0, 0);
      var range = 10_000;

      var stopwatchTree = new Stopwatch();
      stopwatchTree.Start();
      MathUtils.CallCounter = 0;
      var resTree = tree.RangeQuery(middle, range);
      var treeSet = new HashSet<Point>(resTree);
      stopwatchTree.Stop();
      Console.WriteLine("RangeQuery R-tree took: " + stopwatchTree.Elapsed);
      Console.WriteLine("Distance call count: " + MathUtils.CallCounter);

      MathUtils.CallCounter = 0;
      var stopWatchLinear = new Stopwatch();
      stopWatchLinear.Start();
      var resLinear = pointHolder.RangeQuery(middle, range);
      var linearSet = new HashSet<Point>(resLinear);
      stopWatchLinear.Stop();
      Console.WriteLine("RangeQuery linear took: " + stopWatchLinear.Elapsed);
      Console.WriteLine("Distance call count: " + MathUtils.CallCounter);

      Debug.Assert(treeSet.SetEquals(linearSet), "RangeQuery returned wrong results");
      */

      var tree = new Tree(null);

      tree.Insert(new Point(1, 2));
      tree.Insert(new Point(1, 3));
      tree.Insert(new Point(1, 3));
      tree.Insert(new Point(1, 4));
      tree.Insert(new Point(5, 6));
      tree.Insert(new Point(7, 8));
      tree.Insert(new Point(9, 1));
      tree.Insert(new Point(5, 1));
      tree.Insert(new Point(2, 1));
      tree.Insert(new Point(3, 1));
      tree.Insert(new Point(4, 1));


      if (tree.Contains(new Point(4, 2)))
        Console.WriteLine("contains true");

      Console.ReadKey();
    }
  }
}
