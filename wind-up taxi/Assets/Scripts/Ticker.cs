using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    [SerializeField] private TickerItem tickerItemPrefab;
    private TickerItem currentItem;

    [SerializeField] private float itemDuration = 5f;

    private List<string> fillerItems;

    private float width;
    private float pixelsPerSecond;

    private int currentItemIndex = 0;

    private void Start()
    {
        fillerItems = new List<string>();
        fillerItems.Add("Mad Cabbie on The Loose Again... Pullback Springs in Shambles.   ");
        
        if(Leaderboard.Instance.HasEntries())
        {
            for(int i = 0; i < 3; i++)
            {
                if(Leaderboard.Instance.GetEntries()[i] != null)
                {
                    Leaderboard.LeaderboardEntry entry = Leaderboard.Instance.GetEntries()[i];
                    fillerItems.Add("#" + (i + 1).ToString() + "  " + entry.name + ": " + entry.score + "   ");
                }
            }
        }

        width = GetComponent<RectTransform>().rect.width;
        pixelsPerSecond = width / itemDuration;
        AddNextTickerItem();
    }

    private void Update()
    {
        if(currentItem.GetXPosition() <= -currentItem.GetWidth())
        {
            AddNextTickerItem();
        }
    }

    private void AddNextTickerItem()
    {
        currentItem = Instantiate(tickerItemPrefab, transform);
        currentItem.Initialise(width, pixelsPerSecond, fillerItems[currentItemIndex]);

        if(currentItemIndex + 1 >= fillerItems.Count)
        {
            currentItemIndex = 0;
        }
        else
        {
            currentItemIndex++;
        }
    }
}
