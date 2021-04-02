using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragscript : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] public RectTransform dragTransform;
    [SerializeField] public Canvas canvas;
    public GameObject folderPicture, folderBin;
    
    public void OnDrag(PointerEventData eventData)
    {
        DraggingController.isDragging = true;
        
        dragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DraggingController.isDragging = false;
        if (gameObject.tag != ("Icon"))
        {
            dragTransform.SetAsLastSibling();
        }

    }
}
