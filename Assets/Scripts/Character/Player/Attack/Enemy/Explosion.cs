using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PooledObject
{
    public void SetDisalbe()
    {
        gameObject.SetActive(false);
    }
}
