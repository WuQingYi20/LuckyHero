using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTransformData
{
    public List<string> destroyItems;
    public List<string> addItems;

    public LevelTransformData()
    {
        addItems = new List<string> { "Old King Beowulf", "Shipping", "Dragon sleeping" };
        destroyItems = new List<string> { "Beowulf with the sword", "Hrothgar's wife", "Hrothgar", "The mead hall" };
    }
}
