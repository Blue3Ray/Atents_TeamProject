using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ElementalType
{
    None,
    Fire,
    Water,
    Wind,
    Thunder
}



public class ElemantalStatus 
{
    ElementalType elemantal = ElementalType.None;
    public ElementalType Elemantal
    {

        get => elemantal;
        private set
        {
            elemantal = value;
        }
    }

    public int elemantalLevel = 0;

    public System.Action<ElementalType> onElemantaltypeChange;

    public void ChangeType(ElementalType type)
    {
        elemantal = type;
        onElemantaltypeChange?.Invoke(elemantal);
    }
  

}
