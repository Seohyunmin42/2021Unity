using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //플레이어 이동 변수
    public float turnSpeed = 4.0f;
    private float xRotate = 0.0f;
    public float moveSpeed = 10.0f; // 이동 속도
    // Start is called before the first frame update
    Rigidbody rigid;
    bool jDown;
    [SerializeField] GameObject torch;
    private bool torchActive = false;

    //점프 변수
    public CharacterController SelectPlayer; // 제어할 캐릭터 컨트롤러
    //public float Speed;  // 이동속도
    public float JumpPow;
    private float Gravity; // 중력   
    private Vector3 MoveDir; // 캐릭터의 움직이는 방향.
    private bool JumpButtonPressed;  //  최종 점프 버튼 눌림 상태

    GameObject nearObject;
    bool MLbtn;

    public Text keyCountText;
    int keyCount = 0;

    void Start()
    {  
        rigid = GetComponent<Rigidbody>();

        //Speed = 5.0f;
        Gravity = 20.0f;
        MoveDir = Vector3.zero;
        JumpPow = 9.0f;
        JumpButtonPressed = false;
        torch.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Jump1();
        MLbtnClick();
        PlayerMove();

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(torchActive == false)
            {
                torch.gameObject.SetActive(true);
                torchActive = true; 
            }
            else
            {
                torch.gameObject.SetActive(false);
                torchActive = false;
            }
        }
    }
    void PlayerMove()
    {
        //플레이어 이동
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        //  키보드에 따른 이동량 측정
        Vector3 move =
            transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal");

        // 이동량을 좌표에 반영
        transform.position += move * moveSpeed * Time.deltaTime;
    }
    void Jump1()
    {
        //점프
        if (SelectPlayer == null) return;
        // 캐릭터가 바닥에 붙어 있는 경우만 작동합니다.
        // 캐릭터가 바닥에 붙어 있지 않다면 바닥으로 추락하고 있는 중이므로
        // 바닥 추락 도중에는 방향 전환을 할 수 없기 때문입니다.
        if (SelectPlayer.isGrounded)
        {
            // 키보드에 따른 X, Z 축 이동방향을 새로 결정합니다.
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // 오브젝트가 바라보는 앞방향으로 이동방향을 돌려서 조정합니다.
            MoveDir = SelectPlayer.transform.TransformDirection(MoveDir);
            // 속도를 곱해서 적용합니다.
            MoveDir *= moveSpeed;

            // 스페이스 버튼에 따른 점프 : 최종 점프버튼이 눌려있지 않았던 경우만 작동
            if (JumpButtonPressed == false && Input.GetButton("Jump"))
            {
                JumpButtonPressed = true;
                MoveDir.y = JumpPow;
            }
        }
        // 캐릭터가 바닥에 붙어 있지 않다면
        else
        {
            // 중력의 영향을 받아 아래쪽으로 하강합니다.           
            MoveDir.y -= Gravity * Time.deltaTime;
        }

        // 점프버튼이 눌려지지 않은 경우
        if (!Input.GetButton("Jump"))
        {
            JumpButtonPressed = false;  // 최종점프 버튼 눌림 상태 해제
        }
        // 앞 단계까지는 캐릭터가 이동할 방향만 결정하였으며,
        // 실제 캐릭터의 이동은 여기서 담당합니다.
        SelectPlayer.Move(MoveDir * Time.deltaTime);
    }
    void GetInput()
    {
        jDown = Input.GetButtonDown("Jump");
        MLbtn = Input.GetButtonDown("Interation");
    }
    void KeyCount()
    {

    }
    void Jump()
    {
        if(jDown){
            rigid.AddForce(Vector3.up * 400, ForceMode.Impulse);
        }
    }      
    //플레이어가 열쇠의 Collider 안에있으면 인식해주는 함수
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Key")
        {
            nearObject = other.gameObject;
        }
    }
    //플레이어가 열쇠의 Collider 밖으로 나갈때 인식해주는 함수
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Key")
        {
            nearObject = null;
        }
    }
    //플레이어가 열쇠 범위 안에서 마우스 왼쪽 클릭을 했을때 사물이 사라지는 형식
    void MLbtnClick()
    {
        if(MLbtn && nearObject != null)
        {
            if(nearObject.tag == "Key")
            {
                Destroy(nearObject);
                keyCount++;

                keyCountText.text = keyCount.ToString() + " / 12";
            }
        }
    }
}
                                                     