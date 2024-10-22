using UnityEngine;

public enum TileType
{
    BlankPrimary,
    BlankSecondary,

    RoadStraight,
    RoadBump,
    RoadCornerRounded,
    RoadCornerSquare,
    RoadCrossing,
    RoadSplit,
    RoadRamp,
    RoadBridge,
    RoadEndRound,
    RoadEndSquare,

    SpawnerEndRoad,
    SpawnerEndSquare,
    SpawnerRoadRound,
    SpawnerRoadSquare,

    TreesOne,
    TreesTwo,
    TreesMany,

}


//
//  @TODO: Think about a TileConfig scriptable object and instead of returning
//         a mesh, this scriptable object could return the TileConfig object
//
[CreateAssetMenu(fileName = "Tile Set", menuName = "Tower Defense/Tile Set")]
public class TileSet : ScriptableObject
{
    public Material TileMaterial;
    [Header("Blank Tiles")]
    public Mesh BlankPrimary;
    public Mesh BlankSecondary;

    [Header("Road Tiles")]
    public Mesh RoadStraight;
    public Mesh RoadBump;
    public Mesh RoadCornerRounded;
    public Mesh RoadCornerSquare;
    public Mesh RoadCrossing;
    public Mesh RoadSplit;
    public Mesh RoadRamp;
    public Mesh RoadBridge;
    public Mesh RoadEndRound;
    public Mesh RoadEndSquare;

    [Header("River Tiles")]

    [Header("Enemy Spawn Points")]
    public Mesh SpawnerEndRoad;
    public Mesh SpawnerEndSquare;
    public Mesh SpawnerRoadRound;
    public Mesh SpawnerRoadSquare;

    [Header("Environment and Foliage")]
    public Mesh TreesOne;
    public Mesh TreesTwo;
    public Mesh TreesMany;

    public Mesh GetTileData(TileType type)
    {
        return type switch
        {
            TileType.BlankPrimary => BlankPrimary,
            TileType.BlankSecondary => BlankSecondary,
            TileType.RoadStraight => RoadStraight,
            TileType.RoadBump => RoadBump,
            TileType.RoadCornerRounded => RoadCornerRounded,
            TileType.RoadCornerSquare => RoadCornerSquare,
            TileType.RoadCrossing => RoadCrossing,
            TileType.RoadSplit => RoadSplit,
            TileType.RoadRamp => RoadRamp,
            TileType.RoadBridge => RoadBridge,
            TileType.RoadEndRound => RoadEndRound,
            TileType.RoadEndSquare => RoadEndSquare,
            TileType.SpawnerEndRoad => SpawnerEndRoad,
            TileType.SpawnerEndSquare => SpawnerEndSquare,
            TileType.SpawnerRoadRound => SpawnerRoadRound,
            TileType.SpawnerRoadSquare => SpawnerRoadSquare,
            TileType.TreesOne => TreesOne,
            TileType.TreesTwo => TreesTwo,
            TileType.TreesMany => TreesMany,
            _ => BlankPrimary,
        };
    }
}
