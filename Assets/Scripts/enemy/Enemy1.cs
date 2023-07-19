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
    // 높이 조절 y축 (public으로 설정해서 유니티에서도 수정 가능)

    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        // 체력바를 생성하되 canvas의 자식으로 생성하고,체력바의 위치 변경을 쉽게 하기 위해 hpBar에 저장한다.
        // Instantiate(게임오브젝트,부모의transform), 게임오브젝트를 생성하는 함수.
        // UI = CANVAS 내에 있어야 함.

        if (name.Equals("Enemy1"))
        {
            SetEnemyStatus("Enemy1", 100, 10, 1);        
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();
        //GetComponent<Image>(); : 게임오브잭트에 붙어있는 컴포넌트를 받아 오는 함수.
    }

    private void SetEnemyStatus(string _enemyName, int _maxHp, int _atkDmg, int _atkSpeed)
        //적마다 다른 스텟을 가지고 있어야 함으로 편하게 스텟을 할당할 수 있게 함수 설정.
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = +atkDmg;
        atkSpeed = _atkSpeed;
    }
    private void OnTriggerEnter2D(Collider2D col)
        //OnTriggerEnter : 이 스크립트가 붙어있는 오브젝트에 Trigger가 닿았을 때 실행되는 이벤트.
    {
        if (col.CompareTag("Player"))
        {
            if (sword_man.attacked)
            {
                nowHp -= sword_man.atkDmg;
                Debug.Log(nowHp);
                sword_man.attacked = false;
                if (nowHp <= 0) // 적 사망 
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
        //Camera.main.WorldToScreenPoint(월드 좌표 값), 월드 좌표를 스크린좌표(UI좌표로 바꿔주는 함수)
        //HEIGHT : 체력바를 적 머리 위에 위치시키기 위해 추가함 
        hpBar.position = _hpBarPos;
        //스크린 좌표로 바꾼 값으로 체력바를 이동시킴.
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        //HPBAR의 Fillamount를 현재 남은 피의 양에 따라 달라지게 설정.
        //FillAmount : 그림의 길이가 달라짐을 이용해 현재 남은 체력을 알 수 있음.

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(-1, 1, 1);
          //  animator.SetBool("moving", true);

        }
    }
}
  