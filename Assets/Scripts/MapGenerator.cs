using UnityEngine;
using System.Collections.Generic;
//using Zenject;
namespace pathfinding
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> tilePrefabs;
        [SerializeField]
        private int mapWidth = 10;
        [SerializeField]
        private int mapHeight = 10;
        [SerializeField]
        private float tileSpacing = 1.1f;
        // [Inject]
        [SerializeField]
        private TileGrid grid;
        [SerializeField,Range(0, 1)]
        private float walkableRatio = 0.7f;

        void Start()
        {
            GenerateMap();
        }

        public void RegenerateMap(int width, int height, float ratio)
        {
            mapWidth = width;
            mapHeight = height;
            walkableRatio = ratio;
            grid.RemoveChildren();

            GenerateMap();
        }

        void GenerateMap()
        {
            List<Tile> generatedTiles = new List<Tile>();
            int totalTiles = mapWidth * mapHeight;
            int walkableTilesCount = Mathf.RoundToInt(totalTiles * walkableRatio);
            int unwalkableTilesCount = totalTiles - walkableTilesCount;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
                    Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);
                    GameObject tileInstance = Instantiate(tilePrefab, position, Quaternion.identity);
                    tileInstance.transform.SetParent(grid.transform);
                    Tile tileScript = tileInstance.GetComponent<Tile>();
                    if (tileScript != null)
                    {
                        bool isWalkable = false;
                        if (walkableTilesCount > 0 && (unwalkableTilesCount == 0 || Random.value < walkableRatio))
                        {
                            isWalkable = true;
                            walkableTilesCount--;
                        }
                        else
                        {
                            unwalkableTilesCount--;
                        }

                        tileScript.SetWalkable(isWalkable);
                        generatedTiles.Add(tileScript);
                    }
                }
            };
            grid.CreateGrid(mapWidth, mapHeight, generatedTiles);
        }
    }
}
