using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int score; // static은 클래스의 인스턴스에 속하는 것이 아니라 클래스 자체에 속한다. 

    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0; // 죽어서 다시 실행이 될 때에는 score가 0이므로
    }


    void Update ()
    {
        text.text = "Score: " + score;
    }
}
