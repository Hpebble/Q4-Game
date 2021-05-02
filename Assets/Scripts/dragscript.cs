using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;


public class dragscript : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] public RectTransform dragTransform;
  
    [SerializeField] public Canvas canvas;
    public GameObject MouseCursor;
    public float timeLeft;

    public Vector2 mouseOffset;
    bool isDragging;

    public void Start()
    {
    }
        

    public void OnDrag(PointerEventData eventData)
    {
        //dragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Vector2 mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = mousePosRaw + mouseOffset;
        dragTransform.position = new Vector3(mousePos.x, mousePos.y, dragTransform.position.z);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        DraggingController.isDragging = false;
        Debug.Log("Stopped Dragging");
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
        if (DraggingController.isDragging == true)
        {
            if (timeLeft <= 3)
            {
                MouseCursor.SetActive(false);
                timeLeft = 0;
            }
            timeLeft -= Time.deltaTime;
        }
        else if (DraggingController.isDragging == false)
        {
            MouseCursor.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DraggingController.isDragging = true;
        SetMouseToIconOffset(this.gameObject);
    }

    private void SetMouseToIconOffset(GameObject icon)
    {
        Vector3 mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseOffset = (icon.transform.position - mousePosRaw);
    }
}
