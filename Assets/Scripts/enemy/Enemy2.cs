using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    public float distance;
    public float atkDistance;
    public LayerMask isLayer;
    public float speed;
/*
    public GameObject bullet;
    public Transform pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float cooltime;
    private float currenttime;
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        if (raycast.collider != null)
        {
            currenttime = 0;
        
           if (Vector2.Distance(transform.position, raycast.collider.transform.position, Time.deltaTime * speed));
            {
                if (currenttime <= 0)
                { 
                GameObject bulletcopy = Instantiate(bullet, pos.position, transform.rotation);
                }
            }
            else if
            {
            transform.position = Vector3.MoveTowards(transform.position, raycast.collider.transform.position, Time.deltaTime * speed);
            }
            currenttime -= Time.deltaTime;
        }
    } 
    
}
*/