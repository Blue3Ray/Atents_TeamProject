using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy_test : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float range = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= range)
        {
            transform.LookAt(player);

            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        }

        
    }
}
