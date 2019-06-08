using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //총이름
    public float range;    //사정거리
    public float accuracy; //정확도
    public float fireRate; //연사속도
    public float reloadTime;

    public int damage;

    public int reloadBulletCount;  //재장전 개수
    public int currentBulletCount; //탄창에 남은 총알수
    public int maxBulletCount;     //총알 최대 수
    public int carryBulletCount;   //현재 보유 총알 수

    public float retroActionForce; //반동세기
    public float retroActionFineSightForce; //정조준시 반동세기

    public Vector3 fineSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash; //총구 화염 이펙트
    public AudioClip fire_Sound;
}
