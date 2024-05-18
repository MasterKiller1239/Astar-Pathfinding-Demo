using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace pathfinding
{
    public class Tile : MonoBehaviour
    {
        [field: SerializeField]
        public MeshRenderer tileWallRenderer { get; private set; }
        [field: SerializeField]
        public MeshRenderer tileRenderer { get; private set; }
        [field: SerializeField]
        public bool IsWalkable { get; private set; }
        

        private Coroutine _disolveCo;
        private float _dissolveSpeed = 1f;
        private float _dissolveAmount = 1f; 
        
        public Action<Tile> TileClicked;
        public void OnTileClicked() => TileClicked?.Invoke(this);

        public Action<Tile> TileChanged;
        public void OnTileChanged() => TileChanged?.Invoke(this);

        public void SetWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
            OnTileChanged();
            UpdateWalkable();
        }

        private void UpdateWalkable()
        {
            if (tileWallRenderer != null)
            {
                if(_disolveCo != null)
                {
                    StopCoroutine(_disolveCo);
                }
                _disolveCo = StartCoroutine(UpdateTileCo());
            }
        }

        public void HighlightTile(bool shouldHighlight)
        {
            if (tileRenderer != null)
            {
                tileRenderer.material.color = shouldHighlight ? Color.yellow : Color.green;
            }
        }

        private IEnumerator UpdateTileCo()
        {
            do
            {
                if (IsWalkable)
                {
                    _dissolveAmount = Mathf.MoveTowards(_dissolveAmount, 0f, _dissolveSpeed * Time.deltaTime);
                }
                else
                {
                    _dissolveAmount = Mathf.MoveTowards(_dissolveAmount, 1f, _dissolveSpeed * Time.deltaTime);
                }

                Mathf.Clamp01(_dissolveAmount);
                tileWallRenderer.material.SetFloat("_Cutoff", _dissolveAmount);

                if (_dissolveAmount <= .5f)
                {
                    tileWallRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                else
                {
                    tileWallRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                yield return null;
            } while (_dissolveAmount != 0 || _dissolveAmount != 1);
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                OnTileClicked();
            }
        
        }
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(3) && !EventSystem.current.IsPointerOverGameObject())
            {
                SetWalkable(!IsWalkable);
            }
        }
    }
}
