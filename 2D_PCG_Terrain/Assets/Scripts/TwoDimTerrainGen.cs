using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TwoDimTerrainGen : MonoBehaviour
{
    [Header("Generation vars")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float smoothness;
    [SerializeField] private float seed;

    [Header("Gameobject Vars")]
    [SerializeField] private TileBase groundTile;
    [SerializeField] private Tilemap groundTileMap;
    private int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }

    private void Generate()
    {
        //Clear all pre-existing ground tiles
        groundTileMap.ClearAllTiles();

        //
        map = GenerateArray(width, height, true);

        //
        map = GenerateTerrain(map);

        //
        RenderMap(map, groundTileMap, groundTile);
    }

    //***** Input: width, height of 2D integer array, and whether 2D integer array should be filled with 0s or 1s *****
    //***** Output: a 2D integer array of either 0s or 1s based on the value of empty *****
    private int[,] GenerateArray(int width, int height, bool empty)
    {
        //create 2D integer array of width = width and height = height
        int[,] map = new int[width, height];

        //iterate through width and height of map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //if empty is true, set map[x,y] to 0
                //else if empty is false, set map[x,y] to 1
                map[x, y] = (empty) ? 0 : 1;
            }
        }

        //return map
        return map;
    }

    //***** Input: *****
    //***** Output: *****
    private int[,] GenerateTerrain(int[,] map)
    {
        int perlinHeight;

        //get and store width and height of map
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinHeight += height / 2;
            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = 1;
            }
        }

        //
        return map;
    }

    private void RenderMap(int[,] map, Tilemap groundTileMap, TileBase groundTileBase)
    {
        //get and store width and height of map
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                //if value of map at [x,y] is 1,...
                if (map[x,y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x,y,0), groundTileBase);
                }
            }
        }
    }
}
