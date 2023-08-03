using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.InputSystem;
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

    ActionControl ac;

    private void Awake()
    {
        ac = new ActionControl();
        //ac.chimyeongtest.Enable();
        //ac.chimyeongtest.Attack.performed += OnAttack;

    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("attack");
    }

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
        //임시 주석처리     hpBar = Instantiate(prfHpBar).GetComponent<RectTransform>();
            // Instantiate = 복사,복제 코드 
            // canvas= worldspace에 두면 자동적으로 플레이어 or 몬스터를 따라다님
            animator = GetComponent<Animator>();
            maxHp = 50;
            nowHp = 50;
            atkDmg = 10;

            
            animator = GetComponent<Animator>();
            SetAttackSpeed(1.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        
        float h = Input.GetAxis("Horizontal");
        /*방향키 <,> 에 따라 반환값이 바뀌는 함수 
        <방향키를 누르면 -1
        안누르면 0, >방향키를 누르면 1을 반환함
         */
        if (h > 0)


        {
            transform.localScale = new Vector3(1, 1, 1);
            /*transform.localScale = 혀냊 오브젝트의 크기를 조절
            1이 기본값인 x좌표의 크기를 반대로 반대로 바꾸면 좌우가 반전이 됨, (-1, 1, 1)로 설정했다면 
            좌우가 변해서 이동했지만 좌우가 변할 필요는없어 (1, 1, 1)로 설정했음.
             */
            animator.SetBool("moving", true);
            transform.Translate(Vector3.right * Time.deltaTime);
            
            //Translate: 지정된 방향으로 움직이게 하는 함수 
            //Time.deltaTime: 1프레임 당 걸리는 시간 
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);       
            animator.SetBool("moving", true);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else animator.SetBool("moving", false);
        transform.Translate(new Vector3(h,0,0) * Time.deltaTime);
        //방향 값을 가지고 있는 h변수에 따라 움직이도록 설정.


    }
}
