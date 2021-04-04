using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragscript : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    [SerializeField] public RectTransform dragTransform;
    [SerializeField] public Canvas canvas;
    public GameObject MouseCursor;

    

    public void OnDrag(PointerEventData eventData)
    {
        DraggingController.isDragging = true;
        
        MouseCursor.SetActive(false);
        
        dragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggingController.isDragging = false;
        Debug.Log("Stopped Dragging");
        MouseCursor.transform.position = dragTransform.position - MouseCursor.transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        
        if (gameObject.tag != ("Icon"))
        {
            dragTransform.SetAsLastSibling();
        }

    }
    public void Update()
    {
        if (DraggingController.isDragging == false)
        {
            MouseCursor.SetActive(true) ;
        }
        else if (DraggingController.isDragging == true)
        {
            MouseCursor.SetActive(false);
        }

        
        
        
    }
}
