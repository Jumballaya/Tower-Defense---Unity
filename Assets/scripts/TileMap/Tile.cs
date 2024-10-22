
// Holds tile information
using UnityEngine;

public class Tile
{
  public Tile(TileConfig config)
  {
    this.config = config;
    config.SetupTile(this);
  }
  public TileConfig config;
  public Mesh mesh;

  public int elevation = 0;
  public NodeOrientation orientation = NodeOrientation.ZeroDegrees;
}