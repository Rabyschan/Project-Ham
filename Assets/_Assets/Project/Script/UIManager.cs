using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            //���� �ν��Ͻ��� ã�� Ÿ�ֿ̹� �̱����� ���ٸ�
            if (m_instance == null)
            {
                //�� ������ UI Manager Ŭ������ ã�Ƽ� �̱������� ����Ѵ�.
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    public TMP_Text scoreText;
    public TMP_Text achievementText;

    public GameObject pauseUI;
    public GameObject optionUI;
    public GameObject clothUI;

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = " X " + newScore;
    }

    public void UpdateAchieveText(string newText)
    {

    }
}
