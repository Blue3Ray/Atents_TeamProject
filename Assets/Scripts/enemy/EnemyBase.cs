using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// ������� �ʴ� ��ũ��Ʈ�ε� ��������� ���ΰ� �ֽ��ϴ�.

public class EnemyBase : MonoBehaviour
{
    public Transform player;
    public float speed;
    float range =20;
    public float nowhp = 1000.0f;
    public float maxhp = 1000.0f;
    public int atkDmg;
    public float atkSpeed = 1;
    public float exp = 0;
    public float maxexp = 0;
    public float df = 50;
    
    

    SpriteRenderer spriteRenderer;
     // public Vector3(����Ÿ��) dir(����); <-������
    public Vector3 dir;
    Vector3 Dir
    {
        get { return dir; } // ���� �������� �Ҷ� ,������Ƽ ��
        set // Ư�� ���ǿ��� �����ϴ� 
        {           
            dir = value;// ���� �������� �Ҷ� 
            if (dir.x < 0) spriteRenderer.flipX = false;
            if (dir.x > 0) spriteRenderer.flipX = true;

        } 
    }
    
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���忡 ���� 
       // Dir = Vector3.right;
        // ���ϴ� ����� �̸����� ã������� GameObject.Find
      // ���ϴ� ����� �±׷� ã�� ���Ҷ� ���� 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maxhp = 1000;
        nowhp = 1000;
        atkDmg = 10;

        
       // animator = GetComponent<Animator>();
       // SetAttackSpeed(1.5f);
        
       // �ӽ� ¥���� ���Ŀ� ������ ���� (�����ٵ� ���� ���� �����Դϴ�)

    }

    // Update is called once per frame
    void Update()
    {
       //nowHpbar.fillAmount = (float)nowhp / (float)maxhp;

        
        
        // �÷��̾�,���� �Ÿ���� �� ���� �Ÿ� ���� ������ ���� x 
        if (Vector3.Distance(player.position, transform.position)<range) //�÷��̾� ����
        {
            Dir = (player.position - transform.position); //�÷��̾� ��ġ�� ���� ��ġ�� ���� �Ÿ� ���
                                                          //Dir.normalized �÷��̾� ���� �� ���ڿ������� ������� �� �����ϱ� ���� �ӵ� ���� (�ӵ� 1)


            //  Dir = (player.position < transform.position(spriteRenderer.flipX = false);
        }
        else
        {
            if (transform.position.x > 5)
            {
                Dir = Vector3.left;
                spriteRenderer.flipX = false; 
            }
            else if (transform.position.x < -5)
            {
                Dir = Vector3.right;
                spriteRenderer.flipX = true; // �������� �ִ��� �� �� ���������� ���ư����� �ڿ������� ���� ���� �¿������ ��
            }


        }



        transform.Translate(Time.deltaTime * speed * Dir.normalized);
    }

    public void Hit(float damage)
    {
       // hp -= damage; // hp�� ���� ������ ��ŭ ���δ�.
        Console.WriteLine("-00hp"); 

    }
}
 

