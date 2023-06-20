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
        Dir = Vector3.right;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime*Dir*speed);

        if ((player.position.x - transform.position.x)<range) //�÷��̾� ����
        {
            Dir = (player.position - transform.position); //�÷��̾� ��ġ�� ���� ��ġ�� ���� �Ÿ� ���
            Dir.Normalize(); //�÷��̾� ���� �� ���ڿ������� ������� �� �����ϱ� ���� �ӵ� ����
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

    }
}
