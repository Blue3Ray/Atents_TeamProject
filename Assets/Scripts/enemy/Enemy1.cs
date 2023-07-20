using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{
    
    public GameObject prfHpBar;
    public GameObject canvas;
    Animator animator;
    RectTransform hpBar;
    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public int atkSpeed;

    public float height = 1.7f;
    // ���� ���� y�� (public���� �����ؼ� ����Ƽ������ ���� ����)

    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        // ü�¹ٸ� �����ϵ� canvas�� �ڽ����� �����ϰ�,ü�¹��� ��ġ ������ ���� �ϱ� ���� hpBar�� �����Ѵ�.
        // Instantiate(���ӿ�����Ʈ,�θ���transform), ���ӿ�����Ʈ�� �����ϴ� �Լ�.
        // UI = CANVAS ���� �־�� ��.

        if (name.Equals("Enemy1"))
        {
            SetEnemyStatus("Enemy1", 100, 10, 1);        
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();
        //GetComponent<Image>(); : ���ӿ�����Ʈ�� �پ��ִ� ������Ʈ�� �޾� ���� �Լ�.
    }

    private void SetEnemyStatus(string _enemyName, int _maxHp, int _atkDmg, int _atkSpeed)
        //������ �ٸ� ������ ������ �־�� ������ ���ϰ� ������ �Ҵ��� �� �ְ� �Լ� ����.
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = +atkDmg;
        atkSpeed = _atkSpeed;
    }
    private void OnTriggerEnter2D(Collider2D col)
        //OnTriggerEnter : �� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� Trigger�� ����� �� ����Ǵ� �̺�Ʈ.
    {
        if (col.CompareTag("Player"))
        {
            if (sword_man.attacked)
            {
                nowHp -= sword_man.atkDmg;
                Debug.Log(nowHp);
                sword_man.attacked = false;
                if (nowHp <= 0) // �� ��� 
                {
                    Destroy(gameObject);
                    Destroy(hpBar.gameObject);
                }
            }
        }
    }

    public Sword_Man sword_man;
    Image nowHpbar;

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        //Camera.main.WorldToScreenPoint(���� ��ǥ ��), ���� ��ǥ�� ��ũ����ǥ(UI��ǥ�� �ٲ��ִ� �Լ�)
        //HEIGHT : ü�¹ٸ� �� �Ӹ� ���� ��ġ��Ű�� ���� �߰��� 
        hpBar.position = _hpBarPos;
        //��ũ�� ��ǥ�� �ٲ� ������ ü�¹ٸ� �̵���Ŵ.
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        //HPBAR�� Fillamount�� ���� ���� ���� �翡 ���� �޶����� ����.
        //FillAmount : �׸��� ���̰� �޶����� �̿��� ���� ���� ü���� �� �� ����.

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(-1, 1, 1);
          //  animator.SetBool("moving", true);

        }
    }
}
  