using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour, IInteractable
{
    protected bool isOpen = false;

    public virtual bool IsDirectUse => true;

    public void Use()
    {
        Open();
    }

    protected virtual void Open()
    {
        
    }
}
