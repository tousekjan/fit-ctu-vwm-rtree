using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vwm.RTree.Api.Exceptions;
using Vwm.RTree.Api.Models.Snapshot;
using Vwm.RTree.Linear;

namespace Vwm.RTree.Api.Controllers
{
  [Route("api/v1/rtree/snapshots")]
  public class TreeSnapshotController : Controller
  {
    private readonly ILogger<TreeSnapshotController> fLogger;
    private readonly Tree fTree;
    private readonly PointHolder fPointHolder;

    public TreeSnapshotController(
      ILogger<TreeSnapshotController> logger,
      Tree tree,
      PointHolder pointHolder)
    {
      fLogger = logger;
      fTree = tree;
      fPointHolder = pointHolder;
    }

    [HttpGet]
    public SnapshotPreviewModel[] Get()
    {
      var dir = Directory.CreateDirectory("Snapshots");

      var files = dir.GetFiles("*.json");

      return files.Select(x => new SnapshotPreviewModel { Size = x.Length, Name = x.Name }).ToArray();
    }

    [HttpPost("{fileName}")]
    public async Task<IActionResult> Post(string fileName)
    {
      Directory.CreateDirectory("Snapshots");
      if (System.IO.File.Exists($"Snapshots/{fileName}.json"))
        throw new ConflictException($"Snapshot with fileName '{fileName}' already exist.");

      await System.IO.File.WriteAllTextAsync($"Snapshots/{fileName}.json", JsonConvert.SerializeObject(fTree.GetSnapshot(),
        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }));

      return NoContent();
    }

    [HttpDelete("{fileName}")]
    public IActionResult Delete(string fileName)
    {
      Directory.CreateDirectory("Snapshots");
      if (!System.IO.File.Exists($"Snapshots/{fileName}.json"))
        throw new NotFoundException($"Snapshot with fileName '{fileName}' not found.");

      System.IO.File.Delete($"Snapshots/{fileName}.json");

      return NoContent();
    }

    [HttpPut("{fileName}/apply")]
    public async Task<IActionResult> Apply(string fileName)
    {
      if (!System.IO.File.Exists($"Snapshots/{fileName}.json"))
        throw new NotFoundException($"Snapshot with fileName '{fileName}' not found.");

      var data = await System.IO.File.ReadAllTextAsync($"Snapshots/{fileName}.json");
      var snap = JsonConvert.DeserializeObject<Snapshot>(data);
      Constants._Dimensions = snap.Dimensions;
      Constants._MaxNumberOfChildren = snap.MaxNumberOfChildren;
      Constants._MaxPointCount = snap.MaxPointCount;
      Constants._MinNumberOfChildren = snap.MinNumberOfChildren;
      Constants._CacheSize = snap.CacheSize;
      Constants._SplitType = snap.SplitType;

      fPointHolder.Replace(snap.Nodes.Where(x => x.Points != null).SelectMany(x => x.Points).Select(x => x.ToPoint()).ToArray());
      fTree.FillFromSnapshot(snap);
      return NoContent();
    }

    [HttpGet("{fileName}")]
    [Produces("application/octet-stream")]
    public IActionResult Download(string fileName)
    {
      if (!System.IO.File.Exists($"Snapshots/{fileName}.json"))
        throw new NotFoundException($"Snapshot with fileName '{fileName}' not found.");

      var stream = new FileStream($"Snapshots/{fileName}.json", FileMode.Open, FileAccess.Read);
      return File(stream, "application/octet-stream");
    }
  }
}
