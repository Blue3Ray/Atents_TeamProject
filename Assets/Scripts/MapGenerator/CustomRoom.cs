using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoom : MonoBehaviour
{
    [SerializeField]
    CustomSingleTiles[] tileLayers;

    private void Awake()
    {
        tileLayers = GetComponentsInChildren<CustomSingleTiles>();
    }
}
