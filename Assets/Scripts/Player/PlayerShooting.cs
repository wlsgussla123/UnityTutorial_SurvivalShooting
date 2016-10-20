using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20; // 한 발에 데미지가 20
    public float timeBetweenBullets = 0.15f; // 총의 발사 속도
    public float range = 100f; // 총알의 이동거리


    float timer = 0f; // 모든 것의 동기화 유지, 따라서 적절할 때만 공격 가능.
    Ray shootRay; // 총을 발사했을 때 이 광선을 이용해 raycast를 수행하고 총알이 무엇을 맞혔는지 알아낸다.
    RaycastHit shootHit; // 우리가 맞힌 모든 것을 반환한다. 
    int shootableMask; // 쏠 수 있는 것들만 맞히도록 설정하는 용도.
    ParticleSystem gunParticles; // gunParticle 참조 용도
    LineRenderer gunLine; // 참조
    AudioSource gunAudio; // 참조
    Light gunLight; // 참조
    float effectsDisplayTime = 0.2f; // 이벤트가 사라지기 전까지 얼마나 시각적으로 유지하는가?

    /* 모든 참조 설정
     */
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable"); // shootable layer의 수를 반환할 것이다. (레벨, 장애물, 좀비 등)
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    /* 쏠 수 있는 시간인지의 여부를 컨트롤
     */
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        // 발사 후 충분한 시간이 지났다면 효과를 비활성
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    void Shoot()
    {
        timer = 0f; // 타이머 초기화 하고 

        gunAudio.Play(); // 사운드 들리고

        gunLight.enabled = true; // 조명 키고 

        gunParticles.Stop(); // 파티클이 진행중이면 중단하고 시작 할 수 있도록
        gunParticles.Play();

        /* 실제 총알의 시각적 요소인 renderer를 켠다. */
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position); // 0은 총의 끝, 총신. 이것을 transform.position으로 지정한다.

        shootRay.origin = transform.position; // 광선은 총구의 끝에서 시작되므로 transform.position
        shootRay.direction = transform.forward;  // 광선이 시작되어 어느 방향으로 가야하므로 지정해줘야 한다.

        /* 실제로 물리효과를 이용해 총을 발사함. 
         * 우리가 어떤 것을 맞히면 광선을 발사하고(true), 그렇지 않으면 정말 긴 라인을 긋도록 지정을 함(false).
         * Physics.Raycast parameter : shootRay : 우리가 지정한 광선을 파싱(저 방향이라고 짚어주는 것)
         *                             out shootHit : 우리가 맞힌 것에 대한 정보를 달라고 요구
         *                             range : 범위를 나타내며, 위에서 지정을 했다.
         *                             shootableMask : 우리가 원하는 것, 우리가 쏠 수 있는 것만 맞힐 수 있다는 의미.
         */
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>(); // 맞힌다면 적의 체력 스크립트를 가져온다. (shootHit는 내가 맞힌 것이 무엇이든 간에 알게된다.)
            if (enemyHealth != null) // 레고, 벽, 바닥 등은 체력이 없으므로 오로지 체력을 갖고있는 적만이 if문에 들어올 수 있다.
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point); // shootHit를 이용하면 맞힌 위치까지 쉽게 파악 가능하다.
            }
            gunLine.SetPosition(1, shootHit.point); // 우리가 맞힌 것이 벽이든 적이든 맞췄다면 두 번째 점을 생성해야 하기 때문에 설정.(총구에서 시작해 사물까지 이어지는 라인이 생성됨)
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range); // 만약 목표를 맞추지 못 하였따면, 두 번째 점을 사정거리 만큼 지정하여 쭉 선을 잇는다.
        }
    }
}
