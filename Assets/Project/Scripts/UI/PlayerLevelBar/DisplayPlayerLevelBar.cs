using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerLevelBar : DisplayValueBar
{
    private readonly Game _game;
    private readonly string _formatDisplayPlayerLevelBar;

    public DisplayPlayerLevelBar(Image imageDisplayValue, ValueBar valueBar, Game game, string formatDisplayPlayerLevelBar)
        : base(imageDisplayValue, valueBar)
    {
        _game = game;
        _formatDisplayPlayerLevelBar = formatDisplayPlayerLevelBar;
    }

    public override void Display(float currentValue, float maxValue, float minUpdateInterval = 0, bool enableThrottling = false)
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

        if (_lastFillAmount == 0 && normalizedValue == 0 && _isValueBarVisible)
        {
            _imageDisplayValue.fillAmount = normalizedValue;
            _lastFillAmount = normalizedValue;
        }
        else
        {
            if (normalizedValue != _lastFillAmount && _isValueBarVisible)
            {
                _imageDisplayValue.fillAmount = normalizedValue;
                _lastFillAmount = normalizedValue;
            }
        }

        var textValue = string.Format(_formatDisplayPlayerLevelBar, currentValue.ToString(), _game.GameData.SelectedPlayer.Value.Data.LevelUpExp);

        if (_cachedTextValue != textValue && _isDisplayText && TextCount != null)
        {
            TextCount.text = textValue;
            _cachedTextValue = textValue;
        }
    }
}

