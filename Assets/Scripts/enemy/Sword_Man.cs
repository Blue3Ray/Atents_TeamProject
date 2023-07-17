using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword_Man : MonoBehaviour
{
    public GameObject objSwordMan;
    Animator animator;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1;
    public bool attacked = false;
    // attacked 여러번 공격 방지용
    public Image nowHpbar;

    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }
    void SetAttackSpeed(float speed)
    {
        animator.SetFloat("attackSpeed", speed);
        atkSpeed = speed;
    }


    // Start is called before the first frame update

    void Start()
    {

        animator = GetComponent<Animator>();
        maxHp = 50;
        nowHp = 50;
        atkDmg = 10;

        transform.position = new Vector3(0, 0, 0);
        animator = GetComponent<Animator>();
        SetAttackSpeed(1.5f);
    }



    // Update is called once per frame
    void Update()
    {
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        //현재 남은 피의 양에 따라 달라지게 설정 
        if (Input.GetKey(KeyCode.RightArrow));
        
        float h = Input.GetAxis("Horizontal");
        if (h > 0)


        {
            transform.localScale = new Vector3(1, 1, 1);
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
        transform.Translate(new Vector3(h, 0, 0) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("attack");
        }
    }


}   