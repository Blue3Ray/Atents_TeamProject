using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlock : MonoBehaviour
{
    Transform[] childs;
    private void Awake()
    {
        childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterArea enterArea = FindObjectOfType<EnterArea>();
        enterArea.onEnterPlayer += DoorClose;
        DoorOpen();
    }


    void DoorClose()
    {
        StartCoroutine(CloseWithTime());
    }

    float time = 3.0f;

    IEnumerator CloseWithTime()
    {
        yield return new WaitForSeconds(time);
        ActiveChilds(true);
    }

    void DoorOpen()
    {
        ActiveChilds(false);
    }

    void ActiveChilds(bool isActive)
    {
        foreach (Transform child in childs)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
