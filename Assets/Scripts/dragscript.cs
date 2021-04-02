using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragscript : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] public RectTransform dragTransform;
    [SerializeField] public Canvas canvas;
    public void OnDrag(PointerEventData eventData)
    {
        dragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log("Dragging");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragTransform.SetAsLastSibling();
    }
}
