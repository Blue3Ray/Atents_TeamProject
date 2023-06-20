using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Transform player;
    public float speed;
    float range =20;
    public float hp = 1000.0f;
    public float maxhp = 1000.0f;
    public float exp = 0;

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
        Dir = Vector3.right;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime*Dir*speed);

        if ((player.position.x - transform.position.x)<range) //플레이어 추적
        {
            Dir = (player.position - transform.position); //플레이어 위치와 몬스터 위치를 통해 거리 계산
            Dir.Normalize(); //플레이어 추적 시 부자연스럽게 따라오는 걸 방지하기 위해 속도 정함
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

    }
}
