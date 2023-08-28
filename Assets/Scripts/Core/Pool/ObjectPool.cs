using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObjectPool<T> : MonoBehaviour where T : PooledObject
{
    /// <summary>
    /// Pool에 담아 놓을 오브젝트의 프리펩
    /// </summary>
    public GameObject origianlPrefab;

    /// <summary>
    /// Pool의 크기, 처음에 생성하는 오브젝트 개수는 2^n으로 잡는 것이 좋음
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// Pool이 생성한 오브젝트가 들어있는 배열
    /// </summary>
    T[] pool;

    /// <summary>
    /// 사용가능한(비활성화 되어 있는) 오브젝트가 들어있는 Queue
    /// </summary>
    Queue<T> readyQueue;

    public void Initialize()
    {
        
        if(pool == null)
        {
            // Pool이 없을 때
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);

            //readyQueue.Count;         // 실제 있는 개수
            //readyQueue.Capatity;      // 현재 미리 준비해 놓은 갯수

            GenerateObject(0, poolSize, pool);
        }
        else       
        {
            // Pool이 이미 있을 때
            foreach(T obj in pool) 
            { 
                obj.gameObject.SetActive(false);
            }
        }
        
    }

    public T GetObject()
    {
        if (readyQueue.Count > 0)  // Queue에 남아있는게 있을 때
        {
            T comp = readyQueue.Dequeue();
            comp.gameObject.SetActive(true);
            return comp;
        }
        else      // Queue에 남아있는게 없을 때
        {
            ExpandPool();           // Pool 확장
            return GetObject();     // 확장 시킨 Queue에서 가져옴
        }
    }

    /// <summary>
    /// 풀을 두배로 확장시키는 메서드
    /// </summary>
    void ExpandPool()
    {
        Debug.LogWarning($"{gameObject.name} 풀 사이즈 증가.({poolSize} -> {poolSize * 2})");

        int newSize = poolSize * 2;
        T[] newPool = new T[newSize];
        //Queue는 알아서 잘 큼

        for(int i = 0; i < poolSize; i++)
        {
            newPool[i] = pool[i];
        }

        GenerateObject(poolSize, newSize, newPool);
        //pool.Free();
        pool = newPool;
        poolSize = newSize;
    }

    /// <summary>
    /// Pool에 오브젝트를 생성하는 코드
    /// </summary>
    /// <param name="startIndex">시작 인덱스</param>
    /// <param name="endIndex">끝 인덱스 - 1</param>
    /// <param name="arr">생성할 배열</param>
    void GenerateObject(int startIndex, int endIndex, T[] arr)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject obj = Instantiate(origianlPrefab, transform);            // 현재 오브젝트의 자식으로 생성
            obj.name = $"{origianlPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();                                     // T는 PooledObject를 가질수 밖에 없음(초기 설정 where)
            comp.onDisable += () => readyQueue.Enqueue(comp);                   // 비활성화 되면 Queue로 들어가기

            arr[i] = comp;
            obj.SetActive(false);           // onDisable 함수 호출되어 바로 readyQueue로 들어감(그런데 처음부터 비활성화 되면 호출 안됨)
        }
    }
}
