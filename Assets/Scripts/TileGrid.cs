using System.Collections.Generic;

using System;
using UnityEngine;
//using Zenject;
using System.Linq;

namespace pathfinding
{
    public class Node
    {
        public Vector3 worldPosition;
        public bool walkable;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public Node parent;

        public int fCost { get { return gCost + hCost; } }

        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }

    public class TileGrid : MonoBehaviour
    {
        //[Inject]
        //private CharacterMovement.Factory _factory;

        [SerializeField]
        private GameObject _playerPrefab;
        private CharacterMovement _player;
        private List<Tile> _tiles;
        private int _gridWidth;
        private int _gridHeight;
        private Node[,] _grid;

        public Tile lastClickedTile = null;

        public Action<Tile> TileChanged;

        public void OnTileChanged() => TileChanged?.Invoke(lastClickedTile);

        public Action PlayerSpawned;

        public void OnPlayerSpawned() => PlayerSpawned?.Invoke();

        public CharacterMovement Player => _player;

        public void CreateGrid(int newWidth, int newHeight, List<Tile> tiles)
        {
            _gridWidth = newWidth;
            _gridHeight = newHeight;
            _tiles = tiles;
            _grid = new Node[_gridWidth, _gridHeight];

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    Tile tile = tiles[x + y * _gridWidth];
                    bool walkable = tile.IsWalkable;
                    tile.TileClicked += HandleTileClicked;
                    tile.TileChanged += HandleTileChanged;
                    Vector3 worldPoint = tile.transform.position;
                    _grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        private void HandleTileClicked(Tile clickedTile)
        {
            if(lastClickedTile == null || _player == null)
            {
                lastClickedTile = clickedTile;
                var position = lastClickedTile.transform.position;
                Vector3 spawnLocation = new Vector3(position.x,position.y + 0.5f,position.z);
                // _player = _factory?.Create();
                // _player.transform.position = new Vector3(spawnLocation.x, spawnLocation.y + 0.5f, spawnLocation.z);
                _player = Instantiate(_playerPrefab, spawnLocation, Quaternion.identity).GetComponent<CharacterMovement>();
                _player.transform.SetParent(transform);
                OnPlayerSpawned();

            }
            else
            {
                if(clickedTile.IsWalkable)
                lastClickedTile = clickedTile;
                OnTileChanged();
            }
        }

        private void HandleTileChanged(Tile clickedTile)
        {
            if (lastClickedTile != clickedTile)
            NodeFromWorldPoint(clickedTile.transform.position).walkable = clickedTile.IsWalkable;
            OnTileChanged();
        }


        public void HighlightTiles(List<Node> nodes, bool shouldHighlight)
        {
           foreach(Node node in nodes)
           {
                _tiles.First(x => x.transform.position == node.worldPosition).HighlightTile(shouldHighlight);
           }
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x/10);
            int y = Mathf.RoundToInt(worldPosition.z /10);
            return _grid[x, y];
        }

        public void RemoveChildren()
        {
            foreach(var tile in _tiles)
            {
                tile.TileClicked -= HandleTileClicked;
                tile.TileChanged -= HandleTileChanged;
            }
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            _tiles.Clear();
            _player = null;
            lastClickedTile = null;

        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            int[,] directions = new int[,]
            {
                { 0, 1 },
                { 0, -1 }, 
                { 1, 0 }, 
                { -1, 0 } 
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int checkX = node.gridX + directions[i, 0];
                int checkY = node.gridY + directions[i, 1];

                if (checkX >= 0 && checkX < _gridWidth && checkY >= 0 && checkY < _gridHeight)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }

            return neighbours;
        }

    }
}

