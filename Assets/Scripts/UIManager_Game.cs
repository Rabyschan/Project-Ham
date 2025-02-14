using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* <YSA> 
 * Pause 창
 */
public class UIManage_Game : MonoBehaviour
{

    #region 버튼의 onclick()사용/ 일부 스크립트 제어
    [Header("Panel")]
    public GameObject quitPop;


    [Header("Audio")]
    public AudioSource sfxAudioSource;
    public Slider sfxSlider;

    private static UIManage_Game instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject); // ✅ 중복 생성 방지
        }
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        quitPop?.SetActive(false);
    }

    // ✅ 메인 패널이 켜질 때마다 비디오 재생

    // ✅ Quit 팝업에서 "Yes" 버튼 클릭 시 게임 종료
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion
}
