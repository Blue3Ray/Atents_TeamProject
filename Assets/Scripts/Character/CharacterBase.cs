using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �Ӽ� ���� enum

public class CharacterBase : PooledObject, IHealth
{

    // ĳ������ ü�� �κ� -----------------------------------

    /// <summary>
    /// ĳ������ ü��
    /// </summary>
    [SerializeField] float hp = 1.0f;
    /// <summary>
    /// ĳ���� ü���� ������Ƽ
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
    /// ĳ������ �ִ� ü��
    /// </summary>
    public float maxHP;
    /// <summary>
    /// ĳ���� �ִ� ü���� ������Ƽ
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
    /// ĳ������ ü���� ��ȭ�� �� �Ҹ��� ��������Ʈ
    /// </summary>
    public System.Action<float> onHealthChange { get; set; }

    /// <summary>
    /// ĳ���Ͱ� ���� �� �Ҹ��� ��������Ʈ
    /// </summary>
    public System.Action onDie { get; set; }

    /// <summary>
    /// ĳ������ ���� ����
    /// </summary>
    public bool IsAlive => hp > 0;

    Rigidbody2D characterRigid;

    /// <summary>
    /// ���� ĳ���Ͱ� �и��� ����
    /// </summary>
    public Vector3 knockBackDir;

    // ĳ������ ���� �κ� -----------------------------------

    /// <summary>
    /// ĳ������ ���ݷ�
    /// </summary>
    [SerializeField] protected float attackState;
    /// <summary>
    /// ĳ���� ���ݷ��� ������Ƽ
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
    /// ĳ������ ����
    /// </summary>
    [SerializeField] protected float defenceState;
    /// <summary>
    /// ĳ���� ������ ������Ƽ
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
    /// ĳ���Ͱ� ���� ���� �Ӽ�
    /// </summary>
    protected ElemantalStatus elemantalStatus;

    // �Ӽ� ���ݷ�
    public float elemantalAttack;
    // �Ӽ� ����
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
        //Debug.Log($"{gameObject.name} ������");
    }

    protected override void OnEnable()
    {
        OnInitialize();
    }

    // ��� ó���� �Լ�
    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} ����");
        //onDie?.Invoke();
        //gameObject.SetActive(false);
    }

    // �Ӽ� ���� �Լ�
    public void ElemantalSelect(ElementalType elemantal)
    {
        elemantalStatus.ChangeType(elemantal);
    }

    // �Ӽ� ���׷��̵� �Լ�
    public void ElemantalUpgrade(int elemantalLevel)
    {

    }

    // ü�� ȸ�� ��� ----------------------------------------------
    
    /// <summary>
    /// ü���� ƽ ������ ���� ��Ű�� �Լ�
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
    /// ü���� ���������� ȸ����Ű�� �Լ�
    /// </summary>
    /// <param name="totalRegen"></param>
    /// <param name="duration"></param>
    public void HealthRegenetate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegetateCoroutine(totalRegen, duration));
    }

    IEnumerator HealthRegetateCoroutine(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;  // �ʴ� ȸ���� ���
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // �ð� ī����
            HP += Time.deltaTime * regenPerSec;     // �ʴ� ȸ������ŭ ����
            yield return null;
        }
    }

    // ���� ��� ��� -----------------------------------

    public virtual void Attack(CharacterBase target)
    {
        Debug.Log($"{gameObject.name}��(��) {target.name}�� �����ߴ�!");

        target.Defence(AttackState);
    }
	public virtual void Attack(CharacterBase target, float knockBackPower)
	{
		Debug.Log($"{gameObject.name}��(��) {target.name}�� �����ߴ�!");


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

        //Debug.Log($"{gameObject.name}��(��) {resultDamage}��ŭ ���ظ� �Ծ���!");
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

		Debug.Log($"{gameObject.name}��(��) {resultDamage}��ŭ ���ظ� �Ծ���!");
		HP -= resultDamage;
	}

}
