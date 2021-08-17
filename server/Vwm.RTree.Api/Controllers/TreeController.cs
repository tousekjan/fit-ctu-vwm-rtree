using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Vwm.RTree.Api.Extensions;
using Vwm.RTree.Api.Models.RTree;
using Vwm.RTree.Linear;

namespace Vwm.RTree.Api.Controllers
{
  [Route("api/v1/rtree")]
  public class TreeController : Controller
  {
    private readonly ILogger<TreeController> fLogger;
    private readonly Tree fTree;
    private readonly PointHolder fPointHolder;

    public TreeController(
      ILogger<TreeController> logger,
      Tree tree,
      PointHolder pointHolder)
    {
      fLogger = logger;
      fTree = tree;
      fPointHolder = pointHolder;
    }

    /// <summary>
    /// Gets basic info about the tree
    /// </summary>
    /// <returns></returns>
    [HttpGet("info")]
    [ProducesResponseType(typeof(TreeInfoModel), HttpStatusCodes._Ok)]
    public TreeInfoModel GetInfo()
    {
      return new TreeInfoModel();
    }

    [HttpPut("fresh")]
    [ProducesResponseType(typeof(TreeInfoModel), HttpStatusCodes._Ok)]
    public IActionResult Fresh([FromBody] FreshModel model)
    {
      Constants._Dimensions = model.Dimensions;
      Constants._MaxNumberOfChildren = model.MaxNumberOfChildren;
      Constants._MaxPointCount = model.MaxPointCount;
      Constants._MinNumberOfChildren = model.MaxNumberOfChildren / model.MinNumberOfChildrenRatio;
      Constants._SplitType = model.SplitType;
      Constants._CacheSize = model.CacheSize;
      fTree.Clean();
      fPointHolder.Clean();

      DataGenProccessor.Fill(model, fTree, fPointHolder);

      return NoContent();
    }

    [HttpGet("data")]
    [ProducesResponseType(typeof(IEnumerable<NodeBase>), HttpStatusCodes._Ok)]
    public TreeDataModel GetData()
    {
      var nodes = fTree.Nodes;
      if (nodes.Count() > 100)
        return new TreeDataModel { Shrinked = true, NodeCount = nodes.Count() };

      return new TreeDataModel { Structure = fTree.Nodes, NodeCount = nodes.Count(), RootNodeId = fTree.RootNodeId };
    }

    /// <summary>
    /// Adds point to the tree
    /// </summary>
    /// <returns></returns>
    [HttpPost("points")]
    [ProducesResponseType(HttpStatusCodes._Ok)]
    public IActionResult AddPoint([FromBody]AddPointModel model)
    {
      var point = model.ToPoint();
      fTree.Insert(point);
      fPointHolder.Insert(point);
      return NoContent();
    }
  }
}
