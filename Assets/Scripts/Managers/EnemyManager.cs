using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Unity에서 끌어다가 쓸 것들!!
    public PlayerHealth playerHealth; // 체력이 0보다 크면 더 많은 적을 스폰할 것인지 판단.
    public GameObject enemy; // 스폰하려는 적의 참조
    public float spawnTime = 3f; // 스폰 타임.
    public Transform[] spawnPoints; //  이 관리자의 인스턴스 당 하나의 스폰 포인터를 사용한다. (이 튜토리얼에서는 하나의 스폰 포인터만 사용할 것.)

    void Start ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime); // 반복되는 어떤 것을 위해 타이머를 가질 필요가 없다는 것을 의미. 
                                                         // 첫 번째 파라미터 : Spawn 함수 호출, 두 번째 파라미터 : 실행 전 대기 시간이 있다. 세 번째 파라미터 : 반복 전 대기 시간. (3초에 한 번 반복)
    }


    void Spawn ()
    {
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length); // 특정 포인트를 선택해 그 포인트에서 스폰하려고 한다. (0부터 SpawnPoints.length까지의 어느 것이라도 선택되면 되기 때문에)

        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation); // 적을 게임에 인스턴스화 한다. 
                                                                                                           // 파라미터 1 : 랜덤 position
                                                                                                           // 파라미터 2 : 랜덤 rotation
    }
}
