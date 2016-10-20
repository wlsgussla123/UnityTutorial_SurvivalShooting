using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim; // 에니메이션 컴포넌트 참조
    Rigidbody playerRigidbody; // 리지바디 참조
    int floorMask; // 레이캐스트가 플로어(quad가 씌인)만 충돌할 수 있도록 조절하기 위한 변수.
    float camRaylength = 100f; // 카메라에서 발사하는 광선의 길이

    // 참조 설정 등에 유리한 함수 (스크립트 활성화 여부에 상관없이 항상 호출)
    //__ Awake : __이 함수는 항상 Start 함수의 이전 및 프리팹의 인스턴스화 직후에 호출됩니다. (만약 게임 오브젝트가 시작할 때 무효인 경우, 활성화되거나 연결된 하나의 스크립트 함수가 호출될 때까지, Awake는 호출되지 않습니다.)

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor"); // 플로어 레이어에 플로어 쿼드를 놓고 플로어에서 마스크를 확보
        anim = GetComponent<Animator>(); // GetComponent를 이용하여 애니메이터의 참조 설정
        playerRigidbody = GetComponent<Rigidbody>();
    }

    /***** FixedUpdate vs Update ? : http://developug.blogspot.com/2014/09/update-fixedupdate-lateupdate.html  *****/

    // physics update를 유발, 플레이어의 물리적 행동을 정의할 것이므로 사용
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v); // Set을 통하여 x,y 및 z 컴포넌트를 입력할 수 있따. (X와Z는 바닥과 수평을 이룬다)

        movement = movement.normalized * speed * Time.deltaTime; // 

        // 플레이어를 movement + 현재위치로 이동
        playerRigidbody.MovePosition(transform.position + movement);
    }

    // Turning 은 우리가 이미 저장한 입력이 아니라 마우스 입력을 기준으로 하기 때문에 파라미터가 필요없다.
    void Turning()
    {
        // 스크린에서 포인트를 잡아 그 포인트에서 앞쪽의 장면으로 Ray를 쏜다. 
        // 우리가 지정하려는 포인트는 마우스 포인트.
        // 즉, 스크린에 마우스가 있고, 마우스 밑의 포인트는 플로워 쿼드를 맞히면 찾을 수 있는 포인트이다.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        // Raycast 함수는 무엇인가 맞췄을 때 true, 그렇지 않으면 false
        // 1. 우리가 가지려는 캐스트의 위치와 방향
        // 2. out은 이 함수에서 정보를 입수해 floorHit에 저장할 것임을 암시.
        // 3. 레이캐스트가 얼마나 멀리 영향을 주도록 할 지 결정.
        // 4. 이 레이캐스트가 플로어 레이어에서만 맞히기를 시도하는지 확인하는 파라미터
        if (Physics.Raycast(camRay, out floorHit, camRaylength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse); // 회전을 저장하는 방식(Vector3로는 회전을 저장할 수 없기 때문에)
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    // 플레이어가 걷는지 대기상태인지 여부는 입력에 좌우되기 때문에 파라미터 필요
    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f; // 둘 중 하나라도 0이 아니면 걷는 것,
        anim.SetBool("IsWalking", walking);
    }
}