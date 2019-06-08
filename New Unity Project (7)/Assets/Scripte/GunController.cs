using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //Gun의 변수 사용   

    private float currentFireRate; //연사 속도 계산

    private AudioSource audio_Source; //오디오 소스

    void Start()
    {       
        audio_Source = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    //연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; //1초에 1씩 감소
        }
    }

    //마우스 왼쪽 클릭시 발사
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        currentGun.muzzleFlash.Play();
        PlaySE(currentGun.fire_Sound);
        Debug.Log("발사");
    }

    private void PlaySE(AudioClip _clip)
    {
        audio_Source.clip = _clip;
        audio_Source.Play();
    }
}
