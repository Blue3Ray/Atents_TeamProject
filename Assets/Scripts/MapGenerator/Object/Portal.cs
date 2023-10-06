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

    // 이렇게 되면 안되는데 제길ㄴ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Use();
        }
    }
}
