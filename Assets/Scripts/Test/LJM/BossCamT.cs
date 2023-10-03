using System.Collections;
using UnityEngine;
using Cinemachine;

public class BossCamT : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;
    public Transform boss;
    public float transitionTime = 5.0f; 

    //private bool isCameraMoving = false;

    void Start()
    {

        player = GameManager.Ins.Player.transform;

        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;

        
        StartCoroutine(MoveCameraToBoss());


    }
    IEnumerator MyCoroutine()
    {
        // �ڷ�ƾ ����
        yield return new WaitForSeconds(2.0f); // 2�� ���

        Debug.Log("�ڷ�ƾ�� �����");
    }
    IEnumerator MoveCameraToBoss()
    {
        
        //isCameraMoving = true;

        
        virtualCamera.Follow = boss;
        virtualCamera.LookAt = boss;

        
        yield return new WaitForSeconds(transitionTime);

        
        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;

        
        //isCameraMoving = false;
    }
}
