using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap tilemap;

    [Header("Wall Tiles (by type)")]
    [SerializeField] private TileBase wallCenter;
    [SerializeField] private TileBase wallTop;
    [SerializeField] private TileBase wallBottom;
    [SerializeField] private TileBase wallLeft;
    [SerializeField] private TileBase wallRight;
    [SerializeField] private TileBase wallCornerTL;
    [SerializeField] private TileBase wallCornerTR;
    [SerializeField] private TileBase wallCornerBL;
    [SerializeField] private TileBase wallCornerBR;
    [SerializeField] private TileBase wallSolo;

    [Header("Map Settings")]
    [SerializeField] private int width = 30;
    [SerializeField] private int height = 20;
    [SerializeField, Range(0f, 1f)] private float fillPercent = 0.45f;
    [SerializeField] private int smoothingIterations = 4;

    private int[,] map;

    private void Start()
    {
        GenerateMap();
        DrawAutoTiles();
    }

    private void GenerateMap()
    {
        map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = Random.value < fillPercent ? 1 : 0;
            }
        }

        for (int i = 0; i < smoothingIterations; i++)
            SmoothMap();
    }

    private void SmoothMap()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                int wallCount = GetSurroundingWallCount(x, y);
                map[x, y] = wallCount > 4 ? 1 : (wallCount < 4 ? 0 : map[x, y]);
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int count = 0;
        for (int nx = gridX - 1; nx <= gridX + 1; nx++)
        {
            for (int ny = gridY - 1; ny <= gridY + 1; ny++)
            {
                if (nx == gridX && ny == gridY) continue;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    count += map[nx, ny];
                else
                    count++;
            }
        }
        return count;
    }

    private void DrawAutoTiles()
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    int mask = GetNeighborMask(x, y);
                    tilemap.SetTile(new Vector3Int(x, y, 0), GetTileByMask(mask));
                }
            }
        }
    }

    // 4方向の接続状態をビット化 (上=8, 右=4, 下=2, 左=1)
    private int GetNeighborMask(int x, int y)
    {
        int mask = 0;
        if (IsWall(x, y + 1)) mask += 8; // 上
        if (IsWall(x + 1, y)) mask += 4; // 右
        if (IsWall(x, y - 1)) mask += 2; // 下
        if (IsWall(x - 1, y)) mask += 1; // 左
        return mask;
    }

    private bool IsWall(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return true;
        return map[x, y] == 1;
    }

    private TileBase GetTileByMask(int mask)
    {
        switch (mask)
        {
            case 0b1111: return wallCenter;     // 完全に囲まれ
            case 0b0111: return wallLeft;
            case 0b1011: return wallRight;
            case 0b1101: return wallBottom;
            case 0b1110: return wallTop;
            case 0b1100: return wallCornerBL;
            case 0b0110: return wallCornerBR;
            case 0b1001: return wallCornerTL;
            case 0b0011: return wallCornerTR;
            case 0b0000: return wallSolo;
            default: return wallCenter;
        }
    }
}
