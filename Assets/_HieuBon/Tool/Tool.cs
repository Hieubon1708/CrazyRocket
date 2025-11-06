using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public GameObject levelModifier;
    public GameObject map;

    GameObject level;

    private void Start()
    {
        if (levelModifier != null)
        {
            level = Instantiate(levelModifier, transform);
        }
        else
        {
            level = Instantiate(map, transform);    
        }
    }
}
