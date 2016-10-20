using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // 스타트 체력(만땅)
    public int currentHealth;                   
    public float sinkSpeed = 2.5f;              // 좀비가 죽으면 바로 사라지지 않고 땅으로 빨리는 속도
    public int scoreValue = 10;                 
    public AudioClip deathClip;                 // 좀비 죽을 때 사운드


    Animator anim;                             
    AudioSource enemyAudio;                     
    ParticleSystem hitParticles;                // particle 참조 좀비가 맞으면 먼지가 나오도록 particle을 설정 했었따.
    CapsuleCollider capsuleCollider;           
    bool isDead;                                
    bool isSinking;                            // boolean 변수를 이용하여 적들이 가라앉는지와 사망 여부를 알 수 있다.


    void Awake()
    {
        // 참조 
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>(); // hitParticles의 경우는 자식 오브젝트인 hitParticles에서 컴포넌트를 찾아야 한다.
                                                                 // GetComponentInchildren : 모든 자식 게임 오브젝트를 검토해 첫 번째 파티클 시스템을 찾아서 반환.
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentHealth = startingHealth;
    }

    /* 좀비가 가라앉을 것인지를 검토하는 것이 전부 !
     */
    void Update() 
    {
        // isSinking이 true라면
        if (isSinking)
        {
            // transform을 변위시킨다. -Vector 이므로 아래로 초당 sinkSpeed로 이동(초당이므로 Time.delta Time 을 곱), frame 단위가 아닌 초 단위로 이동하게 된다.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    /* 
     * public 함수 이므로 다른 스크립트에서 참조 할 수 있다.
     * parameter : amount : 얼마나 타격을 입었나?
     *             hitPoint : 어디를 타격 입었는가?, hitPoint를 이용하여 적 주위에서 파티클 시스템을 움직여 타격을 입을 때 마다 가루 구름이 날릴 수 있다. 
     */
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        // 죽었으면 아무 조치를 취할 필요가 없다.
        if (isDead)
            return;
        
        // 타격을 입었으므로 공격 받는 소리
        enemyAudio.Play();

        currentHealth -= amount;

        // 트랜스폼의 위치를 hitPoint로 이동시키는 파티클 시스템이다. 따라서 위치를 찾아서 trnasform의 위치를 타격받는 장소로 이동시키는 것이다.
        hitParticles.transform.position = hitPoint;
        hitParticles.Play(); // 파티클의 위치를 찾았기 때문에 그 위치에서 가루가 날리면 된다.

        // 체력이 0 이하라면
        if (currentHealth <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        // 캡슐 콜라이더를 trigger로 저장했기 떄문에 물리적으로 맞힐 수 없으므로 플레이어가 적들을 쓰러뜨리며 이동할 때 장애를 받지 않을 수 있다.
        capsuleCollider.isTrigger = true;

        // Dead 애니메이션 시작
        anim.SetTrigger("Dead");

        
        enemyAudio.clip = deathClip;
        enemyAudio.Play();

        StartSinking();
    }


    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false; // 주의 : 오브젝트 자체를 멈추는게 아니라 component 기능을 멈춘 것임. 
                                                      // 만일 오브젝트를 끄려고 했다면 .setActive = false;이다. 즉, 전체 게임 오브젝트가 아니라 해당 게임 오브젝트의 컴포넌트 하나만 끈다는 것을 의미한다.

        GetComponent<Rigidbody>().isKinematic = true; // kinematic 으로 설정하는 것은 씬에서 콜라이더를 움직일 때 Unity가 모든 스태틱 지오메트리를 
                                                      // 다시 계산할 것인데, Kinematic 리지드바디로 설정하고 이 오브젝트를 변환하면 Unity는 그것을 무시한다.

        // 가라앉고 있으므로
        isSinking = true;
        
        ScoreManager.score += scoreValue;
        
        Destroy(gameObject, 2f); // 2초 후에 완전히 가라앉게 되므로 게임 오브젝트를 파괴한다.
    }
}