using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemCode
{
	Potion = 0,
	Ring
}

public class ItemDataManager : MonoBehaviour
{
	public ItemData[] itemDatas;

	public ItemData this[ItemCode code] => itemDatas[(int)code];


} 
