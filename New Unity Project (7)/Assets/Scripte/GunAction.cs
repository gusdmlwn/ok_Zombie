using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunAction : MonoBehaviour
{
    GameObject player;
    GameObject playerEquipPoint;

    Vector3 forceDirection;
    bool isPlayerEnter;

    public string gunName; //총 이름
    public float range;         //사정거리
    public float accuracy;      //정확도
    public float fireRate;      //발사속도
    public float reloadTime;    //재장전 속도

    public int damage;          //데미지

    public int reloadBulletCount;   //재장전 개수
    public int currentBulletCount;  //탄알집에 남아있는 탄알수
    public int maxBulletCount;      //최대 소유가능한 탄알 수
    public int carryBulletCount;    //현재 소유하고 있는 탄알수

    public ParticleSystem muzzleFlash;
    public AudioClip fire_Sound;

    // Start is called before the first frame update
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
            transform.SetParent(playerEquipPoint.transform);
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
