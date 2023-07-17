using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Sword_Man : MonoBehaviour
{
    public GameObject objSwordMan;
    Animator animator;
    public int maxHp;
    //*주석처리 없엘 예정
  //  public interface nowHp;
 //   public interface atkDmg;
    public float atkSpeed = 1;
    public bool attacked = false;  
  //  public Image nowHpbar;

    void AttackTrue()
    {

    }
    void AttackFalse()
    {

    }
    void SetAttackSpeed(float speed)
    {
        
    }


    // Start is called before the first frame update

    void Start()
    {
        {

            animator = GetComponent<Animator>();
            maxHp = 50;
            //    nowHp = 50;
            //    atkDmg = 10;

            transform.position = new Vector3(0, 0, 0);
            animator = GetComponent<Animator>();
            //    SetAttackSpeed(1.5)f
        }

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

        if (Input.GetKeyDown(KeyCode.A) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("attack");
        }
    }
}
