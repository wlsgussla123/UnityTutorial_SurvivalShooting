using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player; // 적들이 게임을 시작할 때 있는 것이 아니므로 (즉, 게임이 시작한 후에 스폰이 되므로) public으로 둘 수가 없다. * 적들이 스스로 플레이어를 찾아야 하므로 private
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform; // Player에게 부여한 태그 "Player"를 이용하여 FindGameObjectWithTag 로 찾을 수 있다, 그 다음 transform 을 이용해 위치 확보 가능!
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }

    // NavMesh이기 때문에 물리효과를 시간에 따라 맞추지 않기 때문에 Update()
    void Update ()
    {
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination (player.position); // Nav의 목적지 설정. 나는 Player가 위치한 곳에 가겠어! 라고 설정을 한 것. (장점 : 적들끼리 부딪히지 않으며 추적 가능)
        }
        else
        {
            nav.enabled = false;
        }
    }
}
