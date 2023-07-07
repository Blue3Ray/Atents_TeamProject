using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Man : MonoBehaviour
{
    public GameObject objSwordMan;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        objSwordMan.transform.position = new Vector3(0,0,0);
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (h > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else animator.SetBool("moving", false);
        transform.Translate(new Vector3(h,0,0) * Time.deltaTime);      
    }
}
