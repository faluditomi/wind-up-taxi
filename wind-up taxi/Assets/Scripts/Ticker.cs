using UnityEngine;

public class Ticker : MonoBehaviour
{
    [SerializeField] private TickerItem tickerItemPrefab;
    private TickerItem currentItem;

    [SerializeField] private float itemDuration = 5f;

    public string[] fillerItems;

    private float width;
    private float pixelPerSecond;

    private int currentItemIndex = 0;

    private void Awake()
    {
        width = GetComponent<RectTransform>().rect.width;
        pixelPerSecond = width / itemDuration;
        
    }

    // private void Update()
    // {
    //     if(currentItem.GetXPosition() <= currentItem.GetWidth())
    //     {
    //         AddTickerItem
    //     }    
    // }

    // private void AddNextTickerItem()
    // {
    //     currentItem = Instantiate(tickerItemPrefab, transform);
    //     currentItem.Initialise(width, pixelPerSecond, fillerItems[currentItemIndex]);
    //     if(currentItemIndex.)
    //     currentItemIndex++;

    // }
}
