using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}