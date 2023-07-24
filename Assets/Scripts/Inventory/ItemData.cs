using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ScriptableObject/Items", order =1)]

public class ItemData : ScriptableObject
{
    //아이템의 이름
    public string itemName = null;

    //아이템의 이미지
    public Sprite itemSprite;

    //아이템 코드
    public ItemCode itemCode = ItemCode.Potion;

    //아이템 가격
    public int price = 0;

    //아이템 설명
    public string[] itemDes = null;

    


    
}
