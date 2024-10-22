
using UnityEngine;

public class TileConfig : ScriptableObject
{
  public Mesh mesh;

  public void SetupTile(Tile tile)
  {
    tile.mesh = mesh;
  }
}