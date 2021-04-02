using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationlll : MonoBehaviour
{
    public GameObject windowsButtonOb;
    public bool Pressed;
    public GameObject pictureFile;
    public GameObject RecycleBin;
    public GameObject GameFolder;
    // Start is called before the first frame update
    void Start()
    {
        windowsButtonOb.SetActive(false);
        pictureFile.SetActive(false);
        RecycleBin.SetActive(false);
        GameFolder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void windowsButton()
    {
        
        if (Pressed == true)
        {
            windowsButtonOb.SetActive(true);
            Pressed = false;
        }
        else if (Pressed == false)
        {
            windowsButtonOb.SetActive(false);
            Pressed = true;
        }
    }

    public void pictureFileButton()
    {
        if (DraggingController.isDragging == false)
        {
            pictureFile.SetActive(true);
        } 
        
    }

    public void RecycleBinButton()
    {
        if (DraggingController.isDragging == false) 
        {
            RecycleBin.SetActive(true);
        }
        
    }

    public void pictureFileButtonExit()
    {
        pictureFile.SetActive(false);
    }

    public void RecycleBinButtonExit()
    {
        RecycleBin.SetActive(false);
    }

    public void gameFolderOpen()
    {
        if(DraggingController.isDragging == false) 
        {
            GameFolder.SetActive(true);
        }
        
    }

    public void gameFolderClosed()
    {
        GameFolder.SetActive(false);
    }
}
