using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public bool IsDirectUse => true;

    Animator animator;
    readonly int Hash_Open = Animator.StringToHash("TrunOn");

    [SerializeField]
    public IInteractable target;

    void Awake()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<DoorBySwitch>().GetComponent<IInteractable>();    
    }

    public void Use()
    {
        animator.SetTrigger(Hash_Open);
        target?.Use();
    }
}
