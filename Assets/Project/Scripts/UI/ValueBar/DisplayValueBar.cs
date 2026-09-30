using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DisplayValueBar : IInitializable, ITickable
{
    public Text TextCount;
    protected readonly Image _imageDisplayValue;
    protected readonly GameObject _parentForBar;
    protected readonly ValueBar _valueBar;

    protected bool _isDisplayText;
    protected bool _isValueBarVisible;

    protected string _cachedTextValue;
    protected float _lastFillAmount;

    protected float _updateTimer;

    public DisplayValueBar(Image imageDisplayValue, ValueBar valueBar)
    {
        _imageDisplayValue = imageDisplayValue;
        _valueBar = valueBar;
        _isDisplayText = _valueBar.IsDisplayText;
        _isValueBarVisible = _valueBar.IsValueBarVisible;
    }

    public DisplayValueBar(Image imageDisplayValue, GameObject parentForBar, ValueBar valueBar)
    {
        _imageDisplayValue = imageDisplayValue;
        _parentForBar = parentForBar;
        _valueBar = valueBar;

        _isDisplayText = _valueBar.IsDisplayText;
        _isValueBarVisible = _valueBar.IsValueBarVisible;
    }

    public void Initialize()
    {
        if (_valueBar.DisplayPoint != null)
            _valueBar.transform.position = _valueBar.DisplayPoint.position;

        if (!_isDisplayText && TextCount != null)
        {
            TextCount.gameObject.SetActive(false);
        }

        if (!_isValueBarVisible && _parentForBar != null)
        {
            _parentForBar.gameObject.SetActive(false);
        }

        ChangePosition();

        if (!_isDisplayText && TextCount != null)
        {
            var raycaster = TextCount.GetComponent<GraphicRaycaster>();
            if (raycaster != null) raycaster.enabled = false;
        }
    }

    public void Tick()
    {
        ChangePosition();
    }

    public virtual void Display(float currentValue, float maxValue, float minUpdateInterval = 0f, bool enableThrottling = false)
    {
        if (!_isDisplayText && !_isValueBarVisible) return;
        if (_imageDisplayValue == null) return;

        if (enableThrottling)
        {
            float timeSinceLastUpdate = Time.time - _updateTimer;
            if (timeSinceLastUpdate < minUpdateInterval) return;
            _updateTimer = Time.time;
        }

        var normalizedValue = currentValue / maxValue;

        if (normalizedValue != _lastFillAmount && _isValueBarVisible)
        {
            _imageDisplayValue.fillAmount = normalizedValue;
            _lastFillAmount = normalizedValue;
        }

        var textValue = currentValue.ToString();
        if (_cachedTextValue != textValue && _isDisplayText)
        {
            TextCount.text = textValue;
            _cachedTextValue = textValue;
        }
    }

    private void ChangePosition()
    {
        if (_valueBar == null || _valueBar.DisplayPoint == null) return;

        var targetPos = _valueBar.DisplayPoint.position;

        if (_valueBar.transform.position != targetPos)
        {
            _valueBar.transform.position = targetPos;
        }
    }
}
