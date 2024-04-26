using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OseroGridScript : MonoBehaviour
{
    int _x;
    
    int _y;

    public int X
    {
        get => _x;
        set => _x = value;
    }

    public int Y
    {
        get => _y;
        set => _y = value;
    }

    public GridProperties PGridProperties
    {
        get => _gridProperties;
        set => _gridProperties = value;
    }

    GridProperties _gridProperties = GridProperties.Empty;


    

  

    
    
    void OnMouseDown()
    {
        if (PGridProperties == GridProperties.CanPut)
        {
            
        }
        
        
        Debug.Log("X:" + _x + "Y:" + _y);
    }
}

public enum GridProperties
{
    White,
    Black,
    CanPut,
    Empty,
}