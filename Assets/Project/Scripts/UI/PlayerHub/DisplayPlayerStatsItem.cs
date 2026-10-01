using System.Collections.Generic;

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
            playerStateItems[i].Text.text = 
                string.Format(_displayPlayerStatsTextFormat, developItemFloats[i].Name, developItemFloats[i].StartValue);
        }
    }
}
