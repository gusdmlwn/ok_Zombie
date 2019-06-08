using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //Gun의 변수 사용   

    private float currentFireRate; //연사 속도 계산

    private AudioSource audio_Source; //오디오 소스

    //충돌 정보 받아옴
    private RaycastHit hitInfo;

    //카메라 시점 정가운데 
    [SerializeField]
    private Camera theCam;

    //피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;

    private bool isReload = false; // false = 기본 상태 // true = 재장전 중인 상태

    void Start()
    {
      
        audio_Source = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
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
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        //재장전 상태가 아닐시에만
        if (!isReload)
        {
            //현재 탄창에 탄알이 남아있을 경우
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }

            //탄창에 탄알이 없는 경우 재장전 코루틴 실행
            else
            {               
                StartCoroutine(ReloadCoroutine());
            }
        }

    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {            
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void Hit()
    {
        //카메라 시점의 중간으로 레이져를 발사해 충돌한 오브젝트의 정보를 hitInfo에 저장
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    IEnumerator ReloadCoroutine()
    {
        //소유한 탄알이 있을 경우
        if (currentGun.carryBulletCount > 0)
        {
            //재장전 상태로
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            //현재 탄창에 있는 탄알을 소유한 탄알과 합치고 거기서 재장전
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            //소유한 탄알이 최대 장전수 보다 클 경우
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                //탄창을 최대로 장전
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                //소유한 탄알 수에서 최대 장전수를 뺌
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            //소유한 탄알이 최대 장전수 보다 작을 경우
            else
            {
                //소유한 탄알을 모두 탄창에 장전
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                //소유한 탄알은 0
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다");
        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;        //탄창의 탄알 수 1 감소
        currentFireRate = currentGun.fireRate;  //연사 속도 재계산
        PlaySE(currentGun.fire_Sound);          //발사 사운드
        currentGun.muzzleFlash.Play();          //총구 이펙트
        currentGun.anim.SetTrigger("Shot");
        Hit();
        Debug.Log("발사");
    }

   
    private void PlaySE(AudioClip _clip)
    {
        audio_Source.clip = _clip;
        audio_Source.Play();
    }
}
