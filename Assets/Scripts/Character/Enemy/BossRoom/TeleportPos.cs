using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPos : MonoBehaviour
{
    Transform[] teleportPos;

    private void Awake()
    {
        teleportPos = GetComponentsInChildren<Transform>();     // 0은 자기 자신으로 잡힐것임
    }

    public Vector3 GetRandomPos()
    {
        Vector3 result = Vector3.zero;

        int i = Random.Range(1, teleportPos.Length);

        result = teleportPos[i].position;

        return result;  
    }

    public Vector3 GetCenterPos(BossPos pos)
    {
        return teleportPos[(int)pos].position; 
    }
}
