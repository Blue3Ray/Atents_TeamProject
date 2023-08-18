using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEnemyBase : Character
{
    public float moveSpeed;
    float currentSpeed;

    Vector3 targetPos = Vector3.zero;
    Vector3 startPos;

    private void Start()
    {
        currentSpeed = moveSpeed;
        startPos = transform.position;
        targetPos = startPos + new Vector3(-5, 0, 0);
        StartCoroutine(SetTargetPos());
    }


    private void Update()
    {
        Vector3 dir = targetPos - transform.position;
        if (dir.sqrMagnitude > 0.1f)
        {
            dir.y = 0;
            
            transform.Translate(currentSpeed * Time.deltaTime * dir.normalized);
        }
    }

    IEnumerator SetTargetPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            if (transform.position.x < startPos.x)
            {
                targetPos = startPos + new Vector3(5, 0, 0);
            }
            else
            {
                targetPos = startPos + new Vector3(-5, 0, 0);
            }
        }
    }
}
