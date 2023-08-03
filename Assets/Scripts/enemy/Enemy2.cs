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
      // ���忡 ���� 
      // Dir = Vector3.right;
      // ���ϴ� ����� �̸����� ã������� GameObject.Find
      // ���ϴ� ����� �±׷� ã�� ���Ҷ� ���� 
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
       
    }

    // Update is called once per frame

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < range) // �÷��̾� ���� 
        {
            Dir = (player.position - transform.position); //�÷��̾� ��ġ�� ���� ��ġ�� ���� �Ÿ� ���
                                                          //Dir.normalized �÷��̾� ���� �� ���ڿ������� ������� �� �����ϱ� ����   

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
                spriteRenderer.flipX = true; // �������� �ִ��� �� �� ���������� ���ư����� �ڿ������� ���� ���� �¿� ������ ��
            }
        }

        transform.Translate(Time.deltaTime * speed * Dir.normalized);
    
    }

}
    
