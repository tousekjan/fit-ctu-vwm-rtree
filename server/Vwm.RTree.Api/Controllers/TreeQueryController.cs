using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vwm.RTree.Api.Models.Query;
using Vwm.RTree.Linear;

namespace Vwm.RTree.Api.Controllers
{
  [Route("api/v1/rtree/query")]
  public class TreeQueryController : Controller
  {
    private const int _MaxPointReturned = 200;

    private readonly ILogger<TreeQueryController> fLogger;
    private readonly Tree fTree;
    private readonly PointHolder fPointHolder;

    public TreeQueryController(
      ILogger<TreeQueryController> logger,
      Tree tree,
      PointHolder pointHolder)
    {
      fLogger = logger;
      fTree = tree;
      fPointHolder = pointHolder;
    }

    /// <summary>
    /// Returns whether the tree contains given point
    /// </summary>
    /// <returns></returns>
    [HttpGet("contains")]
    [ProducesResponseType(typeof(ContainsQueryResultModel), HttpStatusCodes._Ok)]
    public ContainsQueryResultModel GetContains(ContainsQueryModel model)
    {
      var point = new Point(model.Coords);

      var stopwatch = Stopwatch.StartNew();
      bool contains = fTree.Contains(point);
      stopwatch.Stop();

      long elapsed = stopwatch.ElapsedMilliseconds;

      return new ContainsQueryResultModel
      {
        Contains = contains,
        RequestPoint = point,
        TreeMilliseconds = elapsed
      };
    }

    /// <summary>
    /// Runs knn query and returns result
    /// </summary>
    /// <returns></returns>
    [HttpGet("knn")]
    [ProducesResponseType(HttpStatusCodes._Ok)]
    public KNNQueryResultModel GetKnn(KnnQueryModel model)
    {
      var middlePoint = new Point(model.Coords);

      var treeWatch = Stopwatch.StartNew();
      var treeResult = fTree.KNNQuery(middlePoint, model.Count).ToArray();
      treeWatch.Stop();

      long treeElapsed = treeWatch.ElapsedMilliseconds;

      return new KNNQueryResultModel(middlePoint, model.Count)
      {
        TreeResult = treeResult.Length > _MaxPointReturned ? null : treeResult,
        TreeMilliseconds = (ulong)treeElapsed,
        TreeResultShrinked = treeResult.Length > _MaxPointReturned,
        TreeResultCount = treeResult.Length
      };
    }

    /// <summary>
    /// Runs range query with benchmark and returns result
    /// </summary>
    /// <returns></returns>
    [HttpGet("knn/benchmark")]
    [ProducesResponseType(typeof(BenchmarkRangeQueryResultModel), HttpStatusCodes._Ok)]
    public BenchmarkKNNQueryResultModel GetKnnBenchmark(KnnQueryModel model)
    {
      var middlePoint = new Point(model.Coords);

      var linearWatch = Stopwatch.StartNew();
      var linearResult = fPointHolder.KnnQuery(middlePoint, model.Count).ToArray();
      linearWatch.Stop();

      var treeWatch = Stopwatch.StartNew();
      var treeResult = fTree.KNNQuery(middlePoint, model.Count).ToArray();
      treeWatch.Stop();

      long linearElapsed = linearWatch.ElapsedMilliseconds;
      long treeElapsed = treeWatch.ElapsedMilliseconds;

      var linearSet = new HashSet<Point>(linearResult);
      var treeSet = new HashSet<Point>(treeResult);
      bool resultsIdentical = treeSet.SetEquals(linearSet);

      return new BenchmarkKNNQueryResultModel(middlePoint, model.Count)
      {
        LinearResult = linearResult.Length > _MaxPointReturned ? null : linearResult,
        LinearMilliseconds = (ulong)linearElapsed,
        LinearResultCount = linearResult.Length,
        LinearResultShrinked = linearResult.Length > _MaxPointReturned,

        TreeResult = treeResult.Length > _MaxPointReturned ? null : treeResult,
        TreeMilliseconds = (ulong)treeElapsed,
        TreeResultShrinked = treeResult.Length > _MaxPointReturned,
        TreeResultCount = treeResult.Length,

        ResultsIdentical = resultsIdentical
      };
    }

    /// <summary>
    /// Runs range query and returns result
    /// </summary>
    /// <returns></returns>
    [HttpGet("range")]
    [ProducesResponseType(typeof(RangeQueryResultModel), HttpStatusCodes._Ok)]
    public RangeQueryResultModel GetRange(RangeQueryModel model)
    {
      var middlePoint = new Point(model.Coords);

      var stopwatch = Stopwatch.StartNew();
      var queryResult = fTree.RangeQuery(middlePoint, model.Range).ToArray();
      stopwatch.Stop();

      long treeElapsed = stopwatch.ElapsedMilliseconds;

      return new RangeQueryResultModel(middlePoint, model.Range)
      {
        TreeResult = queryResult.Length > _MaxPointReturned ? null : queryResult,
        TreeMilliseconds = (ulong)treeElapsed,
        TreeResultCount = queryResult.Length,
        TreeResultShrinked = queryResult.Length > _MaxPointReturned
      };
    }

    /// <summary>
    /// Runs range query with benchmark and returns result
    /// </summary>
    /// <returns></returns>
    [HttpGet("range/benchmark")]
    [ProducesResponseType(typeof(BenchmarkRangeQueryResultModel), HttpStatusCodes._Ok)]
    public BenchmarkRangeQueryResultModel GetRangeBenchMark(RangeQueryModel model)
    {
      var middlePoint = new Point(model.Coords);

      var linearWatch = Stopwatch.StartNew();
      var linearResult = fPointHolder.RangeQuery(middlePoint, model.Range).ToArray();
      linearWatch.Stop();

      var treeWatch = Stopwatch.StartNew();
      var treeResult = fTree.RangeQuery(middlePoint, model.Range).ToArray();
      treeWatch.Stop();

      long linearElapsed = linearWatch.ElapsedMilliseconds;
      long treeElapsed = treeWatch.ElapsedMilliseconds;

      var linearSet = new HashSet<Point>(linearResult);
      var treeSet = new HashSet<Point>(treeResult);
      bool resultsIdentical = treeSet.SetEquals(linearSet);

      return new BenchmarkRangeQueryResultModel(middlePoint, model.Range)
      {
        LinearResult = linearResult.Length > _MaxPointReturned ? null : linearResult,
        LinearMilliseconds = (ulong)linearElapsed,
        LinearResultCount = linearResult.Length,
        LinearResultShrinked = linearResult.Length > _MaxPointReturned,
        TreeResult = treeResult.Length > _MaxPointReturned ? null : treeResult,
        TreeMilliseconds = (ulong)treeElapsed,
        TreeResultCount = treeResult.Length,
        TreeResultShrinked = treeResult.Length > _MaxPointReturned,
        ResultsIdentical = resultsIdentical
      };
    }

    /// <summary>
    /// Runs range query and returns result
    /// </summary>
    /// <returns></returns>
    [HttpGet("nn")]
    [ProducesResponseType(typeof(NnQueryResultModel), HttpStatusCodes._Ok)]
    public NnQueryResultModel GetNn(NnQueryModel model)
    {
      var middlePoint = new Point(model.Coords);

      var treeWatch = Stopwatch.StartNew();
      var treeResult = fTree.NNQuery(middlePoint);
      treeWatch.Stop();

      long treeElapsed = treeWatch.ElapsedMilliseconds;

      return new NnQueryResultModel(middlePoint)
      {
        TreeResult = treeResult,
        TreeMilliseconds = (ulong)treeElapsed,
      };
    }

    /// <summary>
    /// Runs nn query with benchmark and returns result
    /// </summary>
    /// <returns></returns>
    [HttpGet("nn/benchmark")]
    [ProducesResponseType(typeof(BenchmarkNnQueryResultModel), HttpStatusCodes._Ok)]
    public BenchmarkNnQueryResultModel GetNnBenchMark(NnQueryModel model)
    {
      var middlePoint = new Point(model.Coords);

      var linearWatch = Stopwatch.StartNew();
      var linearResult = fPointHolder.NnQuery(middlePoint);
      linearWatch.Stop();

      var treeWatch = Stopwatch.StartNew();
      var treeResult = fTree.NNQuery(middlePoint);
      treeWatch.Stop();


      long linearElapsed = linearWatch.ElapsedMilliseconds;
      long treeElapsed = treeWatch.ElapsedMilliseconds;

      return new BenchmarkNnQueryResultModel(middlePoint)
      {
        LinearResult = linearResult,
        LinearMilliseconds = (ulong)linearElapsed,
        TreeResult = treeResult,
        TreeMilliseconds = (ulong)treeElapsed,
        ResultsIdentical = treeResult != null && linearResult != null && treeResult.Equals(linearResult)
      };
    }
  }
}
