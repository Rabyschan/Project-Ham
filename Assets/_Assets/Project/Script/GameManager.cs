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

    // �̱��� ���ٿ� ������Ƽ
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; //�̱����� �Ҵ�� static ����

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
