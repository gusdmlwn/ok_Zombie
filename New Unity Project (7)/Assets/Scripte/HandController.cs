using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 장착된 Hand 무기
    [SerializeField]
    private Hand currentHand;

    //공격중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; //레이져에 닿은 물체의 정보를 얻어올 수 있음

    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    //마우스 좌클릭 시 코루틴 실행
    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))  //Fire1 = 마우스 좌클릭
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCorolutine());
            }
        }
    }

    IEnumerator AttackCorolutine()
    {
        isAttack = true;                        //중복 실행 방지
        currentHand.anim.SetTrigger("Attack");  //공격 애니메이션 실행

        //딜레이
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;
        //공격시 충돌 체크
        StartCoroutine(HitCorolutine());

        //공격 활성화 시점
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    //공격 충돌 체크
    IEnumerator HitCorolutine()
    {
        while (isSwing)
        {
            //공격시 물체와 충돌 했을 경우
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
