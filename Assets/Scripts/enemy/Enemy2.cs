using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
 
public class Enemy2 : MonoBehaviour
{
    public Transform player;  
    float range = 20;
    public float speed;

    SpriteRenderer spriteRenderer;
    public Vector3 dir;
    Vector3 Dir
    {
        get { return dir; }
        set
        {
            dir = value;
            if (dir.x < 0) spriteRenderer.flipX = false;
            if (dir.x > 0) spriteRenderer.flipX = true;
        }
    }



    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
    }


    // Start is called before the first frame update
    void Start()
    {
      // 월드에 방향 
      // Dir = Vector3.right;
      // 원하는 대상을 이름으로 찾고싶으면 GameObject.Find
      // 원하는 대상을 태그로 찾기 위할때 쓰는 
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
       
    }

    // Update is called once per frame

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < range) // 플레이어 추적 
        {
            Dir = (player.position - transform.position); //플레이어 위치와 몬스터 위치를 통해 거리 계산
                                                          //Dir.normalized 플레이어 추적 시 부자연스럽게 따라오는 걸 방지하기 위해   

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
                spriteRenderer.flipX = true; // 왼쪽으로 최대한 간 뒤 오른쪽으로 돌아갔을때 자연스럽게 가기 위해 좌우 반전을 줌
            }
        }

        transform.Translate(Time.deltaTime * speed * Dir.normalized);
    
    }

}
    
