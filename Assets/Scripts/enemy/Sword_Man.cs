using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;

public class Sword_Man : MonoBehaviour
{
    public GameObject objSwordMan;
    Animator animator;
    public int maxHp;
    public GameObject prfHpBar;
    public GameObject canvas;
    RectTransform hpBar;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1;
    public bool attacked = false;  
    public Image nowHpbar;

    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
        // 여러번 공격방지위해 추가 
    }
    void SetAttackSpeed(float speed)
    {
        animator.SetFloat("attackedSpeed", speed);
        atkSpeed = speed;
    }


    // Start is called before the first frame update

    void Start()
    {
        {
            hpBar = Instantiate(prfHpBar).GetComponent<RectTransform>();
            // Instantiate = 복사,복제 코드 
            // canvas= worldspace에 두면 자동적으로 플레이어 or 몬스터를 따라다님
            animator = GetComponent<Animator>();
            maxHp = 50;
            nowHp = 50;
            atkDmg = 10;

            transform.position = new Vector3(0, 0, 0);
            animator = GetComponent<Animator>();
            SetAttackSpeed(1.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
       nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        // FillAmount를 통해 줄어드는 hp를 확인
        float h = Input.GetAxis("Horizontal");
        if (h > 0)


        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else animator.SetBool("moving", false);
        transform.Translate(new Vector3(h,0,0) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("attack");
        }
    }
}
