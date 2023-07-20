using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBySwitch : DoorBase, IInteractable
{
    public override bool IsDirectUse => false;

    Animator animator;
    readonly int Hash_Open = Animator.StringToHash("Open");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Open()
    {
        animator.SetTrigger(Hash_Open);
    }
}
