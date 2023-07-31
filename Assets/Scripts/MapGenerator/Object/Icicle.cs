using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    Rigidbody2D rb;
    public bool isFreezen = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    public void DropObject()
    {
        isFreezen = false;
        rb.gravityScale = 1;
    }

    private void Update()
    {
        if (isFreezen)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up, -transform.up, 5.0f);
            if (hit.transform.CompareTag("Player"))
            {
                DropObject();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Character>(out Character character))
        {
            character.HP -= 10;
        }
        gameObject.SetActive(false);
    }


}
