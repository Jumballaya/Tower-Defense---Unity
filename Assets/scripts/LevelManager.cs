using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public int type;
    public int height;
    public int rotation;

    public Tile(int _type, int _height, int rot)
    {
        type = _type;
        height = _height;
        rotation = rot % 4;
    }
}

public class LevelManager : MonoBehaviour
{
    public List<GameObject> tiles;

    public Tile[,] map = {
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
        { new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0), new Tile(0,0,0) },
    };

    // Rotate a tile 90 degrees
    // false means rotate backwars, true means forwards
    public void RotateTile(int x, int y, bool rot)
    {
        if (x < 0 || x >= map.GetLength(1))
        {
            return;
        }
        if (y < 0 || y >= map.GetLength(0))
        {
            return;
        }

        var tile = map[y, x];
        tile.rotation += rot ? 1 : -1;
        if (tile.rotation < 0)
        {
            tile.rotation = 0;
        }
        tile.rotation %= 4;
    }

    void Start()
    {

        for (int y = 0; y < map.GetLength(0); y++)
        {
            int row = y - 10;
            for (int x = 0; x < map.GetLength(1); x++)
            {
                int col = x - 10;
                var tile = map[y, x];
                var prefab = tiles[tile.type];
                Vector3 pos = new Vector3(col, tile.height, row);
                Quaternion rot = Quaternion.Euler(0f, 90f * (float)tile.rotation, 0f);
                Instantiate(prefab, pos, rot);
            }
        }
    }
}
