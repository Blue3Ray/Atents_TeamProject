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
            //현재 위치(x)가 우로 이동가능한(x)최대값보다 크거나 같다면
            //이동속도+방향에 =1을 곱해 반전을 해주고 현재위치를 우로 이동가능한(x)최대값으로 설정
            else if (transform.position.x < leftMax)
            {
                direction *= -1;

            }
            //현재 위치(x)가 좌로 이동가능한 (x)최대값보다 크거나 같다면
            //이동속도+1을 곱해 반전을 해주고 현재위치를 좌로 이동가능한 (x)최대값으로 설정
            transform.Translate(Time.deltaTime * direction * 10.0f, 0, 0);
            //"Stone"의 위치를 계산된 현재위치로 처리

            //if (Input.GetKey(KeyCode.W))
            //{
            //    this.transform.Translate(new Vector3(-3 * Time.deltaTime, 0, 0)); // 단순 왼쪽방향 이동
            //}

            //if (Input.GetKey(KeyCode.S))
            //{
            ////this.transform.position = new Vector3(0, 0, 2) ;

            //    this.transform.Translate(new Vector3(3*Time.deltaTime , 0, 0)); //단순 오른쪽방향 이동

            //}
        }
    }
}
