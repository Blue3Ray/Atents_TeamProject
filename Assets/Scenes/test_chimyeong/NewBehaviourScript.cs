using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    float rightMax = 20.0f;
    float leftMax = -20.0f;  
    float direction = 3.0f;
    public Transform player;
    public float speed =  5f;
    public float range = 10f;
 


     private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {

  
    }

    // Update is called once per frame
    void Update()
    {

        float playerDis = Vector3.Distance(transform.position, player.position);
        if (playerDis <= range)
        {
            float dir = (player.position.x - transform.position.x);

            transform.Translate(Time.deltaTime * dir * 10.0f, 0, 0);
        }
        else
        {
            if (transform.position.x > rightMax)
            {
                direction *= -1;
            }
            //���� ��ġ(x)�� ��� �̵�������(x)�ִ밪���� ũ�ų� ���ٸ�
            //�̵��ӵ�+���⿡ =1�� ���� ������ ���ְ� ������ġ�� ��� �̵�������(x)�ִ밪���� ����
            else if (transform.position.x < leftMax)
            {
                direction *= -1;

            }
            //���� ��ġ(x)�� �·� �̵������� (x)�ִ밪���� ũ�ų� ���ٸ�
            //�̵��ӵ�+1�� ���� ������ ���ְ� ������ġ�� �·� �̵������� (x)�ִ밪���� ����
            transform.Translate(Time.deltaTime * direction * 10.0f, 0, 0);
            //"Stone"�� ��ġ�� ���� ������ġ�� ó��

            //if (Input.GetKey(KeyCode.W))
            //{
            //    this.transform.Translate(new Vector3(-3 * Time.deltaTime, 0, 0)); // �ܼ� ���ʹ��� �̵�
            //}

            //if (Input.GetKey(KeyCode.S))
            //{
            ////this.transform.position = new Vector3(0, 0, 2) ;

            //    this.transform.Translate(new Vector3(3*Time.deltaTime , 0, 0)); //�ܼ� �����ʹ��� �̵�

            //}
        }
    }
}
