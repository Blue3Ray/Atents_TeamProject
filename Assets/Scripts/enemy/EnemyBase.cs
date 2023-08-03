using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// 사용하지 않는 스크립트인데 참고용으로 냅두고 있습니다.

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
     // public Vector3(변수타입) dir(변수); <-변수문
    public Vector3 dir;
    Vector3 Dir
    {
        get { return dir; } // 값을 받으려고 할때 ,프로퍼티 문
        set // 특정 조건에만 실행하는 
        {           
            dir = value;// 값을 넣으려고 할때 
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
        // 월드에 방향 
       // Dir = Vector3.right;
        // 원하는 대상을 이름으로 찾고싶으면 GameObject.Find
      // 원하는 대상을 태그로 찾기 위할때 쓰는 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maxhp = 1000;
        nowhp = 1000;
        atkDmg = 10;

        
       // animator = GetComponent<Animator>();
       // SetAttackSpeed(1.5f);
        
       // 임시 짜놨고 추후에 보완할 예정 (빨간줄도 추후 수정 예정입니다)

    }

    // Update is called once per frame
    void Update()
    {
       //nowHpbar.fillAmount = (float)nowhp / (float)maxhp;

        
        
        // 플레이어,몬스터 거리계산 후 몬스터 거리 보다 낮을때 추적 x 
        if (Vector3.Distance(player.position, transform.position)<range) //플레이어 추적
        {
            Dir = (player.position - transform.position); //플레이어 위치와 몬스터 위치를 통해 거리 계산
                                                          //Dir.normalized 플레이어 추적 시 부자연스럽게 따라오는 걸 방지하기 위해 속도 정함 (속도 1)


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
                spriteRenderer.flipX = true; // 왼쪽으로 최대한 간 뒤 오른쪽으로 돌아갔을때 자연스럽게 가기 위해 좌우반전을 줌
            }


        }



        transform.Translate(Time.deltaTime * speed * Dir.normalized);
    }

    public void Hit(float damage)
    {
       // hp -= damage; // hp는 맞은 데미지 만큼 깎인다.
        Console.WriteLine("-00hp"); 

    }
}
 

