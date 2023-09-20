using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Action<CharacterBase> onPlayerIn;
    public Action<CharacterBase> onPlayerOut;

    public Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterBase target = other.GetComponent<CharacterBase>();
            onPlayerIn?.Invoke(target);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterBase target = other.GetComponent<CharacterBase>();
            onPlayerOut?.Invoke(target);
        }
    }
}
