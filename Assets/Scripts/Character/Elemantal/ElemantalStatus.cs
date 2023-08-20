using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ElemantalType
{
    None,
    Fire,
    Water,
    Wind,
    Thunder
}

public class ElemantalStatus 
{
    ElemantalType elemantal = ElemantalType.None;
    public ElemantalType Elemantal
    {
        get => elemantal;
        private set
        {
            elemantal = value;
        }
    }

    public int elemantalLevel = 0;

    public void ChangeType(ElemantalType type)
    {
        elemantal = type;
    }
}
