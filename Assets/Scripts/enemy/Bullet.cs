using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Start is called before the first frame update
    private Rigidbody2D bulletRigidbody;
    Transform playerPos;
    Vector2 dir;

    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
   //     dir = playerPos.position; - Transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir.normalized * Time.deltaTime * 100000);

        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
