using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class boss : MonoBehaviour
{
    public Transform target;
    float attackDelay;
    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;
    public string IEnumerator;

    Rigidbody2D rigidbodyToMove;

    boss enemy;
    Animator enemyAnimator;
    private void Awake()
    {
        rigidbodyToMove = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<boss>();
        enemyAnimator = enemy.enemyAnimator;
        
    }

    // Update is called once per frame
    void Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0) attackDelay = 0;

        float distance = Vector3.Distance(transform.position, target.position);

        if (attackDelay == 0 && distance <= enemy.fieldOfVision)
        {
            FaceTarget();

            if (distance <= enemy.atkRange)
            {
                enemyAnimator.SetBool("moving", false);
            }
            else
            {
                if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    MoveToTarget();
                }

            }

            enemyAnimator.SetBool("idle", true);

        }

    
    }
    void MoveToTarget()
    {
        float dir = target.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;
        transform.Translate(new Vector2(dir, 0) * enemy.moveSpeed * Time.deltaTime);
        enemyAnimator.SetBool("moving", true);
    }

    void FaceTarget()
    {
        if (target.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        void AttackTarget()
        {
            target.GetComponent<Player>().HP -= enemy.atkDmg;
            enemyAnimator.SetTrigger("attack");
            attackDelay = enemy.atkSpeed;
        }



    }
    //public IEnumerator Move(float speed)
    //{
    //    float Distance = (target.position.x - transform.position.);
    //    yield return new WaitForSeconds(speed);
        
    //   if (targetTransform != null) 
    //    {
    //        Vector3.Distance(transform.position, Player.position, speed * Time.deltaTime);
        
    //    }
       
    //   if (targetTransform = Player)
    //}
}


