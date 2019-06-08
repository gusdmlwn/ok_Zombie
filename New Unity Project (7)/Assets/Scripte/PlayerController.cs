using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField] //private으로 선언해도 인스펙터 창에서 값을 조정할 수 있다.
    private float walkSpeed;  //걷는 속도  
    [SerializeField]
    private float runSpeed;   //뛰는 속도
    [SerializeField]
    private float crouchSpeed; //앉았을 때 이동 속도

    private float applySpeed; //이동 속도 (통합)

    [SerializeField]
    private float jumpForce; //치솟는 힘

    //상태변수
    private bool isWalk = false;    //걷는 상태 체크
    private bool isRun = false;     //뛰는 상태 체크
    private bool isCrouch = false;  //앉기 상태 체크
    private bool isGround = true;   //지면 충돌 체크

    //움직임 체크 변수
    private Vector3 lastPos; //전 프레임의 플레이어 위치


    private CapsuleCollider capsuleCollider;    //땅과 충돌 여부 체크

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;       //얼마나 숙일지
    private float originPosY;       //원래 값
    private float applyCrouchPosY;   //통합


    //민감도
    [SerializeField]
    private float lookSensitivity; //카메라 시점이동 민감도


    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit; //시점 이동 각도 제한
    private float currentCameraRotationX = 0;


    //필요한 컴퍼넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
   
    //private Crosshair theCrosshair;

    // Start is called before the first frame update
    void Start()
    {
       //컴퍼넌트 초기화
       myRigid = GetComponent<Rigidbody>();
       capsuleCollider = GetComponent<CapsuleCollider>();

       // theCrosshair = FindObjectOfType<Crosshair>();

        //변수 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCroch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    void FixedUpdate()
    {
        MoveCheck();
    }

    //키 입력시 앉기
    private void TryCroch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
       // theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    //부드러운 앉기
    IEnumerator CrouchCoroutine() //코드 병렬처리 기능
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY)
        {
            count++;
            //Lerp() 보간 함수 = 첫번째 좌표에서 두번째 좌표로 이동하는 시간 조정
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.5f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null; //1프레임 마다 위의 과정을 실행
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

    }

    //키 입력시 점프
    private void TryJump()
    {
        //지면과 충돌상태이고, 스페이스 키를 눌렀을 경우
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    //점프 동작
    private void Jump()
    {
        //앉은 상태에서 점프시 앉기 해제
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce; //벨로시티값을 변경시켜 점프시킴
    }

    //지면과의 충돌상태 체크
    private void IsGround()
    {
        //캡슐의 y축 절반부터 지면까지 레이저를 쏴 지면과 충돌체크를 하고 충돌 시엔 true, 점프 시엔 false 값을 반환
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    //키 입력시 달리기
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    //달리기 동작
    private void Running()
    {
        //앉은 상태에서 뛸 시 앉기 해제
        if (isCrouch)
            Crouch();

        //theGunController.CancelFineSight();

        isRun = true;
       //theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;

    }

    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        //theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    //이동
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //좌우 방향키 또는 'a','d'키를 눌렀을 경우 1(오른쪽)과 -1(왼쪽) 값을 리턴 시켜줌
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; //(1,0,0)의 벡터 값에 _moveDirX의 값을 곱해줌으로써 (1,0,0) 또는 (-1,0,0)의 값을 만들어 줌
        Vector3 _moveVertical = transform.forward * _moveDirZ; //(0,0,1)의 벡터 값에 _moveDirZ의 값을 곱해줌으로써 (0,0,1) 또는 (0,0,-1)의 값을 만들어 줌

        //normalized (정규화) 벡터 합의 값을 1로 정규화 시켜주고 거기에 applySpeed(이동속도)값을 곱해줌
        Vector3 _verocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //현재 위치 좌표값에 _verocity값을 더해주어 위치 좌표값을 바꿔주고 deltaTime 값을 곱해서 1초동안 변한 좌표값 만큼 움직임 
        myRigid.MovePosition(transform.position + _verocity * Time.deltaTime);
    }

    //이동 체크
    private void MoveCheck()
    {
        if (!isRun && !isCrouch)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.02f)
            {
                isWalk = true;
            }

            else
            {
                isWalk = false;
            }

        //    theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    //상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); //마우스 Y좌표 값을 가져옴
        float _cameraRotationX = _xRotation * lookSensitivity; //마우스 좌표값과 민감도를 곱해줘 시점 이동속도를 만듬
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //카메라 시점 제한 

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //캐릭터 좌우 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); //벡터 값을 쿼터니언 값으로 변환
    }
}
