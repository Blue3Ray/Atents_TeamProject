using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTrap : MonoBehaviour
{
    public float power;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(Vector2.down * power, ForceMode2D.Impulse);
    }
}
