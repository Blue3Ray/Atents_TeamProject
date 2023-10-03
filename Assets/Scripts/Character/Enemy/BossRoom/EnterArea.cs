using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterArea : MonoBehaviour
{
    public Action onEnterPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            onEnterPlayer?.Invoke();
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,0,1, 0.5f);
        Gizmos.DrawCube(transform.position, Vector3.one * 3);  
    }
#endif
}
