using UnityEngine;
using UnityEngine.UI;

public class TickerItem : MonoBehaviour
{
    private float tickerWidth;
    private float pixelsPerSecond;

    RectTransform rt;

    public float GetXPosition()
    {
        return rt.anchoredPosition.x;
    }

    public float GetWidth()
    {
        return rt.rect.width;
    }

    public void Initialise(float tickerWidth, float pixelsPerSecond, string message)
    {
        this.tickerWidth = tickerWidth;
        this.pixelsPerSecond = pixelsPerSecond;
        rt = GetComponent<RectTransform>();
        GetComponent<Text>().text = message;
    }

    private void Update()
    {
        rt.position += Vector3.left * pixelsPerSecond * Time.deltaTime;

        if(GetXPosition() <= 0 - tickerWidth - GetWidth())
        {
            Destroy(gameObject);
        }
    }
}
