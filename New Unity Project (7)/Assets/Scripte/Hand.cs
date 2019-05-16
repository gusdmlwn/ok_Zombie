﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName; //무기 구분
    public float range;     //공격범위
    public int damage;      //공격력
    public float workSpeed; //작업속도
    public float attackDelay; //공격 딜레이
    public float attackDelayA; //공격 활성화 시점
    public float attackDelayB; //비활성화 시점

    public Animator anim;
    public BoxCollider boxCollider;
}
