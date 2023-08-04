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
        // ������ ���ݹ������� �߰� 
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
        //�ӽ� �ּ�ó��     hpBar = Instantiate(prfHpBar).GetComponent<RectTransform>();
            // Instantiate = ����,���� �ڵ� 
            // canvas= worldspace�� �θ� �ڵ������� �÷��̾� or ���͸� ����ٴ�
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
        /*����Ű <,> �� ���� ��ȯ���� �ٲ�� �Լ� 
        <����Ű�� ������ -1
        �ȴ����� 0, >����Ű�� ������ 1�� ��ȯ��
         */
        if (h > 0)


        {
            transform.localScale = new Vector3(1, 1, 1);
            /*transform.localScale = ��T ������Ʈ�� ũ�⸦ ����
            1�� �⺻���� x��ǥ�� ũ�⸦ �ݴ�� �ݴ�� �ٲٸ� �¿찡 ������ ��, (-1, 1, 1)�� �����ߴٸ� 
            �¿찡 ���ؼ� �̵������� �¿찡 ���� �ʿ�¾��� (1, 1, 1)�� ��������.
             */
            animator.SetBool("moving", true);
            transform.Translate(Vector3.right * Time.deltaTime);
            
            //Translate: ������ �������� �����̰� �ϴ� �Լ� 
            //Time.deltaTime: 1������ �� �ɸ��� �ð� 
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);       
            animator.SetBool("moving", true);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else animator.SetBool("moving", false);
        transform.Translate(new Vector3(h,0,0) * Time.deltaTime);
        //���� ���� ������ �ִ� h������ ���� �����̵��� ����.


    }
}
