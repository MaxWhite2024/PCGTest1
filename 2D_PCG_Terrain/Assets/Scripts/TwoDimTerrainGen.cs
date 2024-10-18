using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TwoDimTerrainGen : MonoBehaviour
{
    [Header("Generation vars")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField, Range(0, 100)] private int horizonHeight;
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
        if(Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    private void Generate()
    {
        //Clear all pre-existing ground tiles
        groundTileMap.ClearAllTiles();

        //Create an array to manipulate later
        map = GenerateArray(width, height, true);

        //Generate horizon from created array
        map = GenerateHorizon(map);

        //Generate Terrain by shifting horizon line from created array with horizon
        map = ShiftTerrainHorizontal(map);

        //map = GenerateTerrain(map);

        //Render a tilemap using the array
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

    private int[,] GenerateHorizon(int[,] map)
    {
        //get and store width and height of map
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                //if current y is less then or equal to horizonHeight, set map[x,y] to 1
                //else if y is greater than horizonHeight, set map[x,y] to 0
                map[x, y] = (y <= horizonHeight) ? 1 : 0;
            }
        }

        //return map
        return map;
    }

    private int[,] ShiftTerrainHorizontal(int[,] map)
    {
        //get and store width and height of map
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for(int x = 0; x < width; x++)
        {

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

    private int[] ShiftHorizontal(int[] column, int amount)
    {
        int height = column.Length;

        //int[] newColumn = column;

        //amount is positive,...
        //if(amount > 0)
        //{
        //    //iterate downwards through newColumn and 
        //    for (int y = height; y > 0; y--)
        //    {
        //        if()
        //        column[y] = 
        //    }
        //}
        ////else if amount is negative,...
        //else if(amount < 0)
        //{
        //    for (int y = 0; y < height; y++)
        //    {

        //    }
        //}

        //return shifted column
        return column;
    }

    //helper functions courtesy of Alex Podles on https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp
    public int[] GetColumn(int[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
    }

    public int[] GetRow(int[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }
}
