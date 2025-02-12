using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameManager : MonoBehaviour
{
    private AudioSource systemAudioPlayer;
    public AudioClip eating;

    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; //싱글톤이 할당될 static 변수

    private int score = 0;

    private void Awake()
    {
        systemAudioPlayer = GetComponent<AudioSource>();

        if(instance != this)
        {
            Destroy(this.gameObject);
        } 
    }

    public void CollectSeed(int newScore)
    {
        score += newScore;
        UIManager.instance.UpdateScoreText(score);
    }
}
