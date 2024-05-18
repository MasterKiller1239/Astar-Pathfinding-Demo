using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderItem : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TMP_Text text;

    public float CurrentValue => _slider.value;

    public delegate void ValueChangedDelegate(float newValue);
    public event ValueChangedDelegate OnValueChanged;

    private void Start()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        UpdateText(_slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateText(value);
        OnValueChanged?.Invoke(value);
    }

    private void UpdateText(float value)
    {
        text.text = (Mathf.Round(value * 100f) / 100f).ToString();
    }

}
