using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f; // 각 공격 사이의 시간의 차
    public int attackDamage = 10; // 얼만큼의 데미지를 입힐 것인가

    /*  private 변수 */
    Animator anim;
    GameObject player; // Player의 참조가 있어야 공격을 할 수 있다.
    PlayerHealth playerHealth; // player의 health 참조
    EnemyHealth enemyHealth; 
    bool playerInRange; // Player가 적의 사정거리에 있는가?
    float timer; // 적의 공격이 너무 빠르거나 느려지지 않도록 동기화하기 위한 변수

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Player의 위치를 확인하고 참조를 로컬에 저장.
                                                             //  Awake 함수로 작업을 수행한 후 저장하므로 모든 프레임 마다 호출 할 필요가 없다.(호출 할 시에 비효율이므로 Awake에서 한 번만 하자)
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>(); // 컴포넌트의 이름은 스크립트 이름
        anim = GetComponent<Animator>();
    }

    /* 트리거이기 때문에 물리적으로 반응하지 않는다. 대신 OnTriggerEnter 호출 (어떤 것이 트리거에 진입할 때 마다 호출) 
     * parameter : other란 collider와 충돌하는 다른 것을 의미 (여기서는 Player)
     */
    void OnTriggerEnter (Collider other)
    {
        // player임을 확인
        if(other.gameObject == player)
        {
            playerInRange = true; // 사정거리 안.
        }
    }

    /* 트리거 안에 있다가 나갈 때 호출되는 함수
     * parameter : OnTirggerEnter와 동일.
     */
    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    /* 실제 공격이 이루어지는 곳.
     * 
     */
    void Update ()
    {
        timer += Time.deltaTime;

        // 타이머가 공격시간 차보다 크다면 공격 사이에 시간이 충분히 지났고 플레이어가 가까운 위치에 있으므로 플레이어를 공격한다고 선언하여라.
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead"); // PlayerDead 파라미터가 호출된다면 애니메이션이 Move -> Idle로 가도록 transition을 해놓았다.
        }
    }


    void Attack ()
    {
        timer = 0f; // 공격중이므로 타이머를 0으로 리셋

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
