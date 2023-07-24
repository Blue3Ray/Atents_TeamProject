using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ScriptableObject/Items", order =1)]

public class ItemData : ScriptableObject
{
    //�������� �̸�
    public string itemName = null;

    //�������� �̹���
    public Sprite itemSprite;

    //������ �ڵ�
    public ItemCode itemCode = ItemCode.Potion;

    //������ ����
    public int price = 0;

    //������ ����
    public string[] itemDes = null;

    


    
}
