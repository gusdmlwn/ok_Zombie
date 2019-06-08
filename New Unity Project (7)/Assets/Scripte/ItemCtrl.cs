using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    public GameObject ItemBox;
    private GameObject[] WayPoint = new GameObject[8];
    private int[] RandomNumber = new int[3];
    private bool[] Flag = new bool[8];
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0;i<8;i++)
        {
            WayPoint[i] = transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < 8; i++)
            Flag[i] = false;
        StartCoroutine(this.InitItem());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InitItem()
    {
        while(true)
        {
            yield return new WaitForSeconds(5.0f);

            int number = Random.Range(0, 8);
            if (Flag[number] == false)
            {
                GameObject itembox = (GameObject)Instantiate(ItemBox, WayPoint[number].transform.position, Quaternion.identity);
                Flag[number] = true;
            }

        }
    }
}
