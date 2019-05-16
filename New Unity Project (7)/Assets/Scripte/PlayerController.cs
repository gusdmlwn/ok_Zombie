using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed;    //걷기 속도
    [SerializeField]
    private float runSpeed;     //달리기 속도
    private float applySpeed;   //속도값 변환을 위한 통합 속도

    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool isRun = false;
    private bool isWalk = false;
    private bool isGround = true;
    private Vector3 lastPos;

    //컴퍼넌트
    private Rigidbody myRigid; //플레이어 몸체
    private Animator myAnim;
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        applySpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun(); //이동 상태 체크
     //   Moving();
        Move();        
    }

    void FixedUpdate()
    {
        MoveCheck();
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
            myAnim.SetBool("Jump", true);
        }      
        else
            myAnim.SetBool("Jump", false);
    }

    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            myAnim.SetBool("Run", true);
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("Run", false);
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    { 
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //좌우
        float _moveDirZ = Input.GetAxisRaw("Vertical");   //앞뒤

        Vector3 _moveHorizontal = transform.right * _moveDirX; //(1,0,0)
        Vector3 _moveVertical = transform.forward * _moveDirZ; //(0,0,1)

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //벡터 정규화
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); //Time.deltaTime -> 1초 동안 움직임
    }

    private void MoveCheck()
    {
        if (!isRun)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.02f)
            {
                isWalk = true;
                myAnim.SetBool("Walk", true);
            }

            else
            {
                isWalk = false;
                myAnim.SetBool("Walk", false);
            }          
            lastPos = transform.position;
        }
    }

    private void Moving()
    {
        if(isWalk == true)
            myAnim.SetBool("Walk", true);
        else
            myAnim.SetBool("Walk", false);
    }
}
