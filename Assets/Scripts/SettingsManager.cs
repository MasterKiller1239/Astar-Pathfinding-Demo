using pathfinding;
using UnityEngine;
using UnityEngine.UI;
//using Zenject;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private SliderItem _widthSlider;
    [SerializeField]
    private SliderItem _heightSlider;
    [SerializeField]
    private Button _refreshButton;
    [SerializeField]
    private SliderItem _playerSpeedSlider;
    [SerializeField]
    private SliderItem _walkableRatioSlider;
    //[Inject]
    [SerializeField]
    private MapGenerator _generator;
    //[Inject]
    [SerializeField]
    private TileGrid _grid;

    public void Start()
    {
        _refreshButton.onClick.AddListener(Regenerate);
        _playerSpeedSlider.OnValueChanged += HandleSpeedSliderValueChanged;
        _grid.PlayerSpawned += SetPlayerSpeed;
    }

    private void SetPlayerSpeed()
    {
        HandleSpeedSliderValueChanged(_playerSpeedSlider.CurrentValue);
    }
    private void Regenerate()
    {
        _generator.RegenerateMap((int)_widthSlider.CurrentValue, (int)_heightSlider.CurrentValue, _walkableRatioSlider.CurrentValue);
    }

    private void OnDestroy()
    {
        _grid.PlayerSpawned -= SetPlayerSpeed;
        if (_playerSpeedSlider != null)
        {
            _playerSpeedSlider.OnValueChanged -= HandleSpeedSliderValueChanged;
        }
    }

    private void HandleSpeedSliderValueChanged(float newValue)
    {
        _grid.Player?.ChangeSpeed(newValue);
    }
}
