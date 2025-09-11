using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum TileType { Path, Barrier, SpawnRoom, Power }

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject pathPrefab;
    [SerializeField] GameObject barrierPrefab;
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] GameObject pointPrefab;
    [SerializeField] GameObject powerPrefab;
    [SerializeField] int width = 28;   // Pac-Man maps are usually 28x31
    [SerializeField] int height = 31;
    [SerializeField] string fileName = "Tile/Maps/Level1.csv";
    private TileType[,] grid;
    [HideInInspector] public TileType[,] Grid => grid;

    public static GridManager sharedInstance;

    public List<GameObject> points = new List<GameObject>();

    public static Action<int> pointsActivated;

    [ContextMenu("Generate Level From CSV")]
    public void GenerateLevel()
    {
        if (!Application.isPlaying) // Only in editor
        {
            grid = new TileType[width, height];
            ClearChildren();
            LoadCSV(fileName);
            generateMap();
        }
        if (Application.isPlaying)
        {
            grid = new TileType[width, height];
            ClearChildren();
            LoadCSV(fileName);
            generateMap();
            spawnPoints();
        }
    }
    void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(transform.GetChild(i).gameObject);
            else
                Destroy(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
    }

    void LoadCSV(string filePath)
    {
        // Load the file (relative to Assets/Resources if using Resources.Load)
        string fullPath = Path.Combine(Application.dataPath, filePath);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("CSV file not found at: " + fullPath);
            return;
        }

        string[] lines = File.ReadAllLines(fullPath);

        int height = lines.Length;
        int width = lines[0].Split(',').Length;
        for (int y = 0; y < height; y++)
        {
            string[] values = lines[y].Split(',');

            for (int x = 0; x < width; x++)
            {
                int cell = int.Parse(values[x]);

                switch (cell)
                {
                    case 0:
                        grid[x,height-y-1] = TileType.Path; // Flip Y so row 0 is bottom
                        break;
                    case 1:
                        grid[x,height - y - 1] = TileType.Barrier;
                        break;
                    case 2:
                        grid[x,height - y - 1] = TileType.SpawnRoom;
                        break;
                    case 3:
                        grid[x,height - y - 1] = TileType.Power;
                        break;
                }
            }
        }
    }

    void generateMap()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x, 0, y);

                GameObject prefab = null;

                switch (grid[x,y])
                {
                    case TileType.Path: grid[x,y] = TileType.Path; prefab = pathPrefab; break;
                    case TileType.Barrier: grid[x,y] = TileType.Barrier; prefab = barrierPrefab; break;
                    case TileType.SpawnRoom: grid[x,y] = TileType.SpawnRoom; prefab = spawnPrefab; break;
                    case TileType.Power: grid[x,y] = TileType.Power; prefab = pathPrefab; break; // Power uses path prefab
                }

#if UNITY_EDITOR
                if (prefab != null)
                {
                    GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    go.transform.position = pos;
                    go.transform.SetParent(transform);
                }
#endif
            }
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            GenerateLevel();
        }
    }
    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe to NewGame event
        GameManager.NewGame += OnNewGame;
        GameManager.GameOver += OnGameOver;  // Subscribe to GameOver Event
        // Subscribe to GameOver Event
    }

    void OnNewGame()
    {
        activatePoints();
        //For item in maps
        //Spawn a Point
        //Exclude SpawnLocation
    }
    void OnGameOver()
    {
        deactivatePoints();
    }
    void spawnPoints()
    {
        points.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x,y] == TileType.Path)
                {
                    Vector3 pos = new Vector3(x, 0.1f, y); // Slightly above ground
                    GameObject point = Instantiate(pointPrefab, pos, Quaternion.identity, transform);
                    points.Add(point);
                    point.SetActive(false);
                }
                else if (grid[x,y] == TileType.Power)
                {
                    Vector3 pos = new Vector3(x, 0.1f, y); // Slightly above ground
                    GameObject power = Instantiate(powerPrefab, pos, Quaternion.identity, transform);
                    points.Add(power);
                    power.SetActive(false);
                }
            }

        }
    }
    void activatePoints()
    {
        foreach (var point in points)
        {
            point.SetActive(true);
        }
        pointsActivated?.Invoke(points.Count);
    }

    void deactivatePoints()
    {
        foreach (var point in points)
        {
            point.SetActive(false);
        }
    }
}
