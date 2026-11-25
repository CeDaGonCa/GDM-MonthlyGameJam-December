using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ScenePart
{
    [TextArea]
    [SerializeField] List<string> lines;
    [SerializeField] Sprite background;
    [SerializeField] bool isSpoken = false;
    [SerializeField] List<string> dialogOrder;

    public List<string> Lines
    {
        get { return lines; }
    }
    public Sprite Background => background;
    public bool IsDialog => isSpoken; 
    public List<string> DialogOrder => dialogOrder;
}
