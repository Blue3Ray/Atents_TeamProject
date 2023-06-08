using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    // 클래스 내의 T 타입은 모두 <T>로 대체
    // 싱글톤의 T는 컴포넌트 타입

    /// <summary>
    /// 이미 종료 처리에 들어갔는지 확인하기 위한 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 싱글톤의 객체
    /// </summary>
    private static T instance;

    /// <summary>
    /// 싱글톤의 객체를 읽기 위한 프로퍼티 
    /// </summary>
    public static T Ins
    {
        get
        {
            if (isShutDown)      // 종료 처리에 들어간 상황인지 확인
            {
                Debug.LogWarning("싱글톤은 이미 삭제되었음");
                return null;
            }

            if (instance == null)    // instance가 없고
            {
                T singleton = FindObjectOfType<T>();
                if (singleton == null)  // 씬에도 없으면 새로 오브젝트 생성
                {
                    GameObject obj = new GameObject();
                    obj.name = $"{typeof(T).Name} Singleton";
                    singleton = obj.AddComponent<T>();
                }

                instance = singleton;       // intance를 찾았거나 만들어진 객체 대입
                DontDestroyOnLoad(instance.gameObject);     //씬이 없어져도 삭제 되지 않게 설정
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)  // 자기자신이 생성됬는데 instance가 없을 때
        {
            // 씬에 배치되어 있는 첫번째 싱글톤을 자기 자신으로 등록
            // Singleton<T> 와 T를 구분 못하므로 캐스팅 시도
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            // instance가 있는데 그게 자기 자신이 아닐 때
            if (instance != this) Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnInitalize();
    }

    protected virtual void OnInitalize()
    {
        
    }

    private void OnApplicationQuit()
    {
        isShutDown = true;
    }
}