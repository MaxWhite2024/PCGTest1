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
    [SerializeField] private int noiseAmplitude = 1;
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

        //Generate horizon from created map
        map = GenerateHorizon(map);

        //Generate Terrain by shifting horizon line from created array with horizon
        map = ShiftTerrainHorizontal(map);

        //Generate Terrain by directly applying perlin noise
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
        int perlinHeight;

        //get and store width and height of map
        int width = map.GetLength(0);
        Debug.Log(width);
        int height = map.GetLength(1);

        //iterate through each column,...
        for(int x = 0; x < width; x++)
        {
            Debug.Log(x);
            //calculate perlinHeight
            //perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * noiseAmplitude);

            //get column
            int[] column = GetColumn(map, x);

            //shift column by perlin height
            column = ShiftColumn(column, perlinHeight);

            //set column
            map = SetColumn(map, x, column);
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
            Debug.Log(Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2));
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            //perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed));
            perlinHeight += height / 2;
            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = 1;
            }
        }

        //return map with perlin noise generated terrain
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

    private int[] ShiftColumn(int[] column, int amount)
    {
        int height = column.Length;

        //int[] newColumn = column;

        //if amount is positive,...
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                //iterate downwards through newColumn and shift each element upwards
                for (int y = height - 1; y > 0; y--)
                {
                    //if y != 0, set column[y] = column[y - 1]
                    //else y == 0, so set column[y] = 1 (solid block)
                    column[y] = (y != 0) ? column[y] = column[y - 1] : 1;
                }
            }
        }
        //else if amount is negative,...
        else if (amount < 0)
        {
            for (int i = 0; i < amount; i++)
            {
                //iterate upwards through newColumn and shift each element downwards
                for (int y = 0; y < height; y++)
                {
                    //if y != height - 1, set column[y] = column[y + 1]
                    //else y == height - 1, so set column[y] = 0 (air)
                    column[y] = (y != height - 1) ? column[y] = column[y + 1] : 0;
                }
            }
        }

        //return shifted column
        return column;
    }

    private int[] ShiftRow(int[] row, int amount)
    {
        return row;
    }

    //GetColumn and GetRow courtesy of Alex Podles on https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp
    public int[] GetColumn(int[,] matrix, int columnNumber)
    {
        Debug.Log("A");
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

    //SetColumn and SetRow
    public int[,] SetColumn(int[,] matrix, int columnNumber, int[] column)
    {
        int height = matrix.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            matrix[columnNumber, y] = column[y];
        }

        return matrix;
    }

    public int[,] SetRow(int[,] matrix, int rowNumber, int[] row)
    {
        int width = matrix.GetLength(0);

        for (int x = 0; x < height; x++)
        {
            matrix[rowNumber, x] = row[x];
        }

        return matrix;
    }
}
