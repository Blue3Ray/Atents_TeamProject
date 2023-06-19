using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Transform player;
    public Vector3 dir;
    public float speed;
    float range =20;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.right;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime*dir*speed);

        if ((player.position.x - transform.position.x)<range)
        {
            dir = (player.position - transform.position);
            dir.Normalize();
        //  dir = (player.position < transform.position(spriteRenderer.flipX = false);
        }
        else
        {
            if (transform.position.x > 5)
            {
                dir = Vector3.left;
                spriteRenderer.flipX = false;
            }
            else if (transform.position.x < -5)
            {
                dir = Vector3.right;
                spriteRenderer.flipX = true;
            }
        }

    }
}
