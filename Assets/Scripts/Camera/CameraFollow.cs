using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target; // 카메라가 따라갈 타겟, unity 툴에서 Player를 드래그 해서 참조시켜 줄 것이다.
    public float smoothing = 5f; // 카메라의 속도 (0은 카메라가 계쏙 추적하기에 너무 빈틈이 없다)

    Vector3 offset; // 카메라와 플레이어의 오프셋을 저장하기 위한 변수

    void Start()
    {
        offset = transform.position - target.position; // offset은 카메라에서 플레이어까지의 벡터
    }

    // 물리 오브젝트를 추적하기 때문에 그냥 Update를 사용하면 플레이어와 다른 시간대에 움직이게 된다.
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset; // 카메라가 도달하려는 타겟 위치.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime); // 실제로 카메라를 이동시키는 lerp 메소드, 두 위치 사이를 부드럽게 이동할 수 있따.
    }
}
