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
            GameManager.Ins.Player.transform.position = transform.position + new Vector3(4,0,0);
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
