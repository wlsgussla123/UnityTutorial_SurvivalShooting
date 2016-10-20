using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth; // 플레이어에 대한 체력이 있어야 게임종료 시기를 알 수 있음.
    public float restartDelay = 5f; // 플레이어가 죽은 후 어느 정도 경과한 후에 다시 시작

    Animator anim; // 애니메이터의 파라미터 트리거를 설정해야 하기 때문에
    float restartTimer; 

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver"); // 체력이 0이라면 애니메이터 트리거 파라미터를 GameOver로 설정하여 해당 애니메이션 트리거 되도록.

            restartTimer += Time.deltaTime;
            
            // restartTimer 가 5초를 경과하면
            if(restartTimer >= restartDelay)
            {
                Application.LoadLevel(Application.loadedLevel); // 씬을 리로드(파라미터에 다시 똑같은 것을 넣음으로써 재시작 될 수 있도록)
            }
        }
    }
}
