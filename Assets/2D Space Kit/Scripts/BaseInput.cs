using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseInput : StandaloneInputModule
{
    /*
    public CursorLockMode CursorLockMode = CursorLockMode.Locked;

    public override void Process()
    {
        //Cursor.lockState = CursorLockMode.None;
 
        base.Process();
 
        bool isDragging = false;
        foreach (PointerEventData p in this.m_PointerData.Values)
        {
            if (p.dragging)
            {
                isDragging = true;
                break;
            }
        }
        
        if (!isDragging)
            Cursor.lockState = this.CursorLockMode;

    
    }
    */
}