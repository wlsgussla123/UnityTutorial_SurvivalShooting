using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100; // 게임을 시작할 때 플레이어의 체력이 얼마인지 지정하는 public 변수
    public int currentHealth; // 게임의 특정 시점에서의 체력
    public Slider healthSlider; // 슬라이더 오브젝트 (앞서 생성한 slider의 참조), using UnityEngine.UI 필요 
    public Image damageImage; // 생성한 데미지 이미지의 참조
    public AudioClip deathClip; // the audio clip to play when the player dies.
    public float flashSpeed = 5f; // 데미지 이미지 표시되는 속도
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // 붉은색 설정  


    Animator anim; // 애니메이션 참조 
    AudioSource playerAudio; // 오디오 컴포넌트 참조 
    PlayerMovement playerMovement; // 이미 생성한 또 다른 스크립트 참조, 플레이어가 죽으면 움직임을 조절해야 하므로 참조.
    //PlayerShooting playerShooting; 
    bool isDead;
    bool damaged;


    /* 컴포넌트를 확보하여 참조 */
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> (); // 스크립트 이름을 이용하면 참조를 할 수 있다.
        //playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth; // 현재 체력은 만땅으로 
    }


    /* 플레이어가 데미지를 입으면 이 이미지를 [flashColour]로 설정하는 것 (flashColour의 색은 초기화 했음) */
    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime); // Vector3.Lerp와 마찬가지로 자연스럽게 이동하는 것. 즉, 색상이 자연스럽게 이동
        }
        damaged = false; // 필수, 데미지를 입은 후 false 로 바꿔줘야 함.
    }

    /* public 함수이기 때문에 다른 스크립트나 컴포넌트가 호출할 수 있다. (다른 스터프가 이 함수를 호출 할 것) 
     
     * parameter : 플레이어가 입은 데미지의 양
     */
    public void TakeDamage (int amount)
    {
        damaged = true; // 화면 깜빡거리게

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

        anim.SetTrigger ("Die"); // 애니메이션에 파라미터 추가를 할 때 Die Trigger를 추가했던 것을 재생.

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false; // 참조한 스크립트에 접근
        //playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
