using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    public string poolItemName = string.Empty;  //객체 검색할 때 사용할 이름
    public GameObject prefab = null;            //풀에 저장할 프리팹
    public int poolCount = 0;                   //초기화할 때 생성할 객체의 수
    [SerializeField]                           
    private List<GameObject> poolList = new List<GameObject>();  //생성한 객체를 저장할 리스트

    //객체 초기화시 처음 한번 호출
    //poolCount에 지정한 수 만큼 객체를 생성해 List에 추가하는 역할
    public void Initialize(Transform parent = null)
    {
        for (int i = 0; i < poolCount; ++i)
        {
            poolList.Add(CreateItem(parent));
        }
    }

    //사용한 객체를 다시 풀에 반환하는 역할
    public void PushToPool(GameObject item, Transform parent = null)
    {
        item.transform.SetParent(parent);
        item.SetActive(false);
        poolList.Add(item);
    }

    //객체가 필요할 때 풀에 요청하는 역할
    public GameObject PopFromPool(Transform parent = null)
    {
        if(poolList.Count == 0)
        {
            poolList.Add(CreateItem(parent));
        }
        GameObject item = poolList[0];
        poolList.RemoveAt(0);
        return item;
    }

    //프리팹 오브젝트를 생성하는 역할
    private GameObject CreateItem(Transform parent = null)
    {
        GameObject item = Object.Instantiate(prefab) as GameObject;
        item.name = poolItemName;
        item.transform.SetParent(parent);
        item.SetActive(false);
        return item;
    }
}

