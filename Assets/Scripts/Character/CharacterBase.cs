using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 속성 선택 enum

public class CharacterBase : PooledObject, IHealth
{

    // 캐릭터의 체력 부분 -----------------------------------

    /// <summary>
    /// 캐릭터의 체력
    /// </summary>
    [SerializeField] float hp = 1.0f;
    /// <summary>
    /// 캐릭터 체력의 프로퍼티
    /// </summary>
    public float HP
    {
        get => hp;
        set
        {
            if (IsAlive)
            {
                hp = value;
                if (hp <= 0)
                {
                    onDie?.Invoke();
                }
                hp = Mathf.Clamp(HP, 0, MaxHP);
                onHealthChange?.Invoke(HP / MaxHP);
            }

        }
    }

    /// <summary>
    /// 캐릭터의 최대 체력
    /// </summary>
    public float maxHP;
    /// <summary>
    /// 캐릭터 최대 체력의 프로퍼티
    /// </summary>
    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }

    /// <summary>
    /// 캐릭터의 체력이 변화될 때 불리는 델리게이트
    /// </summary>
    public System.Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 캐릭터가 죽을 때 불리는 델리게이트
    /// </summary>
    public System.Action onDie { get; set; }

    /// <summary>
    /// 캐릭터의 생존 여부
    /// </summary>
    public bool IsAlive => hp > 0;

    Rigidbody2D characterRigid;

    /// <summary>
    /// 맞은 캐릭터가 밀리는 방향
    /// </summary>
    public Vector3 knockBackDir;

    // 캐릭터의 스텟 부분 -----------------------------------

    /// <summary>
    /// 캐릭터의 공격력
    /// </summary>
    [SerializeField] protected float attackState;
    /// <summary>
    /// 캐릭터 공격력의 프로퍼티
    /// </summary>
    public float AttackState
    {
        get { return attackState; }
        set
        {
            attackState = value;
        }
    }


    public System.Action<float> onAttackChange { get; set; }

    /// <summary>
    /// 캐릭터의 방어력
    /// </summary>
    [SerializeField] protected float defenceState;
    /// <summary>
    /// 캐릭터 방어력의 프로퍼티
    /// </summary>
    public float DefenceState
    {
        get { return defenceState; }
        set
        {
            defenceState = value;
        }
    }

    public System.Action<float> onDefenceChange { get; set; }
   
    /// <summary>
    /// 캐릭터가 가질 원소 속성
    /// </summary>
    protected ElemantalStatus elemantalStatus;

    // 속성 공격력
    public float elemantalAttack;
    // 속성 방어력
    public float elemantalDefence;

    protected virtual void Awake()
    {
        elemantalStatus= new ElemantalStatus();
        characterRigid = GetComponent<Rigidbody2D>();
        onDie += Die;
        HP = MaxHP;
        knockBackDir = transform.right;
    }

    public virtual void OnInitialize()
    {
        HP = MaxHP;
        //Debug.Log($"{gameObject.name} 생성됨");
    }

    protected override void OnEnable()
    {
        OnInitialize();
    }

    // 사망 처리용 함수
    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} 죽음");
        //onDie?.Invoke();
        //gameObject.SetActive(false);
    }

    // 속성 선택 함수
    public void ElemantalSelect(ElementalType elemantal)
    {
        elemantalStatus.ChangeType(elemantal);
    }

    // 속성 업그레이드 함수
    public void ElemantalUpgrade(int elemantalLevel)
    {

    }

    // 체력 회복 기능 ----------------------------------------------
    
    /// <summary>
    /// 체력을 틱 단위로 증가 시키는 함수
    /// </summary>
    /// <param name="tickRegen"></param>
    /// <param name="tickTime"></param>
    /// <param name="totalTickCount"></param>
    public void HealthRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount)
    {
        StartCoroutine(HealthRegenerateByTickCoroutine(tickRegen, tickTime, totalTickCount));
    }

    IEnumerator HealthRegenerateByTickCoroutine(float tickRegen, float tickTime, uint totalTickCount)
    {
        WaitForSeconds wait = new WaitForSeconds(tickTime);
        for (uint tickCount = 0; tickCount < totalTickCount; tickCount++)
        {
            HP += tickRegen;
            yield return wait;
        }
    }

    /// <summary>
    /// 체력을 지속적으로 회복시키는 함수
    /// </summary>
    /// <param name="totalRegen"></param>
    /// <param name="duration"></param>
    public void HealthRegenetate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegetateCoroutine(totalRegen, duration));
    }

    IEnumerator HealthRegetateCoroutine(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;  // 초당 회복량 계산
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // 시간 카운팅
            HP += Time.deltaTime * regenPerSec;     // 초당 회복량만큼 증가
            yield return null;
        }
    }

    // 공격 방어 기능 -----------------------------------

    public virtual void Attack(CharacterBase target)
    {
        Debug.Log($"{gameObject.name}이(가) {target.name}을 공격했다!");

        target.Defence(AttackState);
    }
	public virtual void Attack(CharacterBase target, float knockBackPower)
	{
		Debug.Log($"{gameObject.name}이(가) {target.name}을 공격했다!");


		target.Defence(AttackState, knockBackPower, elemantalStatus);
	}

	public virtual void Defence(float damage, ElemantalStatus elemantal = null)
    {
        float resultDamage = 0;
        if (elemantal == null || elemantal.Elemantal == ElementalType.None)
        {
            resultDamage = Mathf.Clamp(damage, 0, MaxHP);
        }
        else
        {
            switch (elemantal.Elemantal)
            {
                case ElementalType.Fire:
                    resultDamage = Mathf.Clamp(damage, 0, MaxHP) + 10;
                    break;
                case ElementalType.Water:
                    damage *= 1.2f;
                    resultDamage = Mathf.Clamp(damage, 0, MaxHP);
                    break;
                case ElementalType.Wind:
                    damage *= 0.8f;
                    resultDamage = Mathf.Clamp(damage, 0, MaxHP);
                    break;
                case ElementalType.Thunder:
                    damage *= 2f;
                    resultDamage = Mathf.Clamp(damage, 0, MaxHP);
                    break;
            }
        }

        //Debug.Log($"{gameObject.name}이(가) {resultDamage}만큼 피해를 입었다!");
        HP -= resultDamage;
    }
    
    public virtual void Defence(float damage, float knockBackPower, ElemantalStatus elemantal = null)
    {
        if(characterRigid != null)
        {
            characterRigid.AddForce(knockBackDir * knockBackPower, ForceMode2D.Impulse);
        }
		float resultDamage = 0;
		if (elemantal == null || elemantal.Elemantal == ElementalType.None)
		{
			resultDamage = Mathf.Clamp(damage, 0, MaxHP);
		}
		else
		{
			switch (elemantal.Elemantal)
			{
				case ElementalType.Fire:
					resultDamage = Mathf.Clamp(damage, 0, MaxHP) + 10;
					break;
				case ElementalType.Water:
					damage *= 1.2f;
					resultDamage = Mathf.Clamp(damage, 0, MaxHP);
					break;
				case ElementalType.Wind:
					damage *= 0.8f;
					resultDamage = Mathf.Clamp(damage, 0, MaxHP);
					break;
				case ElementalType.Thunder:
					damage *= 2f;
					resultDamage = Mathf.Clamp(damage, 0, MaxHP);
					break;
			}
		}

		Debug.Log($"{gameObject.name}이(가) {resultDamage}만큼 피해를 입었다!");
		HP -= resultDamage;
	}

}
