using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerStatsItem
{
    private readonly string _displayPlayerStatsTextFormat;

    public DisplayPlayerStatsItem(string displayPlayerStatsTextFormat)
    {
        _displayPlayerStatsTextFormat = displayPlayerStatsTextFormat;
    }

    public void Display(List<PlayerStateItem> playerStateItems, List<DevelopItemFloat> developItemFloats)
    {
        for (int i = 0; i < developItemFloats.Count; i++)
        {
            if (playerStateItems[i].Text == null)
            {
                Debug.LogError("playerStateItems[i].Text = null");
                continue;
            }

            playerStateItems[i].Text.text = 
                string.Format(_displayPlayerStatsTextFormat, developItemFloats[i].Name, developItemFloats[i].StartValue);
        }
    }
}
