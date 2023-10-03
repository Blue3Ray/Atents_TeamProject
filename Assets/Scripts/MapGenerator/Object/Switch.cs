using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Switch : MonoBehaviour, IInteractable
{
    public bool IsDirectUse => true;
    bool isPlayerIn = false;

    Animator animator;
    readonly int Hash_Open = Animator.StringToHash("TrunOn");

    [SerializeField]
    public IInteractable target;

    void Awake()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<DoorBySwitch>().GetComponent<IInteractable>();    
    }
	private void Start()
	{
        GameManager.Ins.Player.OnUsePerformed += Use;
	}

	public void Use()
    {
        if (isPlayerIn)
        {
			animator.SetTrigger(Hash_Open);
			target?.Use();
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			isPlayerIn = false;
		}
	}
}
