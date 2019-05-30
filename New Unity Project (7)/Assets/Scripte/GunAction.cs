using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunAction : MonoBehaviour
{
    GameObject player;
    GameObject playerEquipPoint;

    Vector3 forceDirection;
    bool isPlayerEnter;
    
   
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerEquipPoint = GameObject.FindGameObjectWithTag("Equipment");
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetButtonDown("Action")&& isPlayerEnter)
        {
            //transform.SetParent(playerEquipPoint.transform);
            transform.localPosition = Vector3.zero;           
            transform.rotation = new Quaternion(0, 0, 0, 0);

            transform.position = playerEquipPoint.transform.position;
            isPlayerEnter = false;
        } 
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerEnter = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            isPlayerEnter = false;
        }
    }

    
}
