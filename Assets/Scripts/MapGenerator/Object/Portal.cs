using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    public bool IsDirectUse => true;

    public void Use()
    {
        LevelManager.Ins.Level++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Use();
        }
    }
}
