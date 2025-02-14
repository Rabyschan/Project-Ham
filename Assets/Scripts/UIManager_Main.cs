using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/*<YSA> 시작할 때 비디오플레이어랑 사운드 자동적으로 재생
* Main (Start/Option/Quit 버튼)
* Start (Slot Empty => 클릭 시 Save 팝업) 
* -> save 팝업 yes => 로딩 비디오 띄우기 (LoadScene 참고)/ no => 팝업 끄기
* Option ([게임패널]BGM 조절 슬라이더/ SFX 조절 슬라이더 / 언어 드롭다운(구현 완료)/
* 컨트롤 버튼=> [컨트롤 패널] 키기)
* Quit (Quit 팝업 yes => 게임 종료 / no => 팝업끄기)
*/

/* <YSA> 
 * 초기 UI 설정
 * 시작할 때 메인 비디오(타겟이미지)/오디오 재생 => 메인 씬이 켜질 때마다
 * Quit 팝업의 Yes버튼(게임 종료) => 함수로
 * 로딩할 때 로딩 비디오 재생/오디오 멈추기 (비디오 플레이어 클립/타겟이미지 바꾸기)
 */
public class UIManage_Main : MonoBehaviour
{
    /* 전체 스크립트로 제어하기
    // ---------- Video & Sound ----------
    [Header("Video & Sound")]
    public VideoPlayer videoPlayer;   // 시작 시 자동 재생할 비디오 플레이어
    public AudioSource audioSource;   // 시작 시 자동 재생할 사운드 (BGM)

    // ---------- UI Panels & Buttons ----------
    [Header("Main Panel")]
    public GameObject mainPanel;      // 메인 패널 (Start, Option, Quit 버튼 포함)
    public Button startButton;        // Start 버튼
    public Button optionButton;       // Option 버튼
    public Button quitButton;         // Quit 버튼

    [Header("Save Popup")]
    public GameObject savePopup;      // Save 팝업 (슬롯이 비어 있을 때 표시)
    public Button saveYesButton;      // Save 팝업의 Yes 버튼 (로딩 비디오 실행)
    public Button saveNoButton;       // Save 팝업의 No 버튼 (팝업 닫기)

    [Header("Option Panel")]
    public GameObject optionPanel;    // Option 패널 (BGM/SFX 슬라이더, 언어 드롭다운, 컨트롤 버튼 포함)
    public Slider bgmSlider;          // BGM 조절 슬라이더
    public Slider sfxSlider;          // SFX 조절 슬라이더
    public Dropdown languageDropdown; // 언어 드롭다운 (구현 완료)
    public Button controlButton;      // 컨트롤 버튼 (컨트롤 패널 열기)

    [Header("Control Panel")]
    public GameObject controlPanel;   // 컨트롤 패널

    [Header("Quit Popup")]
    public GameObject quitPopup;      // Quit 팝업
    public Button quitYesButton;      // Quit 팝업의 Yes 버튼 (게임 종료)
    public Button quitNoButton;       // Quit 팝업의 No 버튼 (팝업 닫기)

    // ---------- 기타 변수 ----------
    private bool isSaveSlotEmpty = true; // 저장 슬롯이 비어 있는지 여부

    private void Start()
    {
        InitializeUI();
        RegisterEvents();
        PlayMedia();
    }

    // UI 초기 상태 설정
    void InitializeUI()
    {
        if (mainPanel) mainPanel.SetActive(true);
        if (savePopup) savePopup.SetActive(false);
        if (optionPanel) optionPanel.SetActive(false);
        if (controlPanel) controlPanel.SetActive(false);
        if (quitPopup) quitPopup.SetActive(false);
    }

    // 버튼 및 슬라이더 이벤트 등록
    void RegisterEvents()
    {
        if (startButton) startButton.onClick.AddListener(OnStartButtonClicked);
        if (optionButton) optionButton.onClick.AddListener(() => optionPanel.SetActive(true));
        if (quitButton) quitButton.onClick.AddListener(() => quitPopup.SetActive(true));

        if (saveYesButton) saveYesButton.onClick.AddListener(OnSaveYesButtonClicked);
        if (saveNoButton) saveNoButton.onClick.AddListener(() => savePopup.SetActive(false));
        if (controlButton) controlButton.onClick.AddListener(() => controlPanel.SetActive(true));
        if (quitYesButton) quitYesButton.onClick.AddListener(OnQuitYesButtonClicked);
        if (quitNoButton) quitNoButton.onClick.AddListener(() => quitPopup.SetActive(false));

        if (bgmSlider) bgmSlider.onValueChanged.AddListener(value => { if (audioSource) audioSource.volume = value; });
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(value => Debug.Log("SFX 볼륨: " + value));
    }

    // 비디오와 사운드 자동 재생
    void PlayMedia()
    {
        if (videoPlayer) videoPlayer.Play();
        if (audioSource) audioSource.Play();
    }

    // Start 버튼 클릭 이벤트
    void OnStartButtonClicked()
    {
        if (isSaveSlotEmpty)
        {
            if (savePopup) savePopup.SetActive(true);
        }
        else
        {
            // 슬롯에 데이터가 있다면 바로 씬 로딩 가능
            LoadGameScene();
        }
    }

    // Save 팝업 Yes 버튼 클릭 이벤트
    void OnSaveYesButtonClicked()
    {
        if (savePopup) savePopup.SetActive(false);
        LoadGameScene();
    }

    // Quit 팝업 Yes 버튼 클릭 이벤트
    void OnQuitYesButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // 씬 로딩 요청 (SceneLoader 스크립트 사용)
    void LoadGameScene()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadGameScene();
        }
        else
        {
            Debug.LogWarning("SceneLoader 스크립트를 찾을 수 없습니다!");
        }
    }

    // 메모리 누수를 막기 위한 이벤트 해제
    private void OnDestroy()
    {
        if (startButton) startButton.onClick.RemoveListener(OnStartButtonClicked);
        if (optionButton) optionButton.onClick.RemoveAllListeners();
        if (quitButton) quitButton.onClick.RemoveAllListeners();
        if (saveYesButton) saveYesButton.onClick.RemoveListener(OnSaveYesButtonClicked);
        if (saveNoButton) saveNoButton.onClick.RemoveAllListeners();
        if (controlButton) controlButton.onClick.RemoveAllListeners();
        if (quitYesButton) quitYesButton.onClick.RemoveListener(OnQuitYesButtonClicked);
        if (quitNoButton) quitNoButton.onClick.RemoveAllListeners();
    }*/

    #region 버튼의 onclick()사용/ 일부 스크립트 제어
    [Header("Panel")]
    public GameObject mainPanel;
    public GameObject startPanel;
    public GameObject optionPanel;
    public GameObject quitPop;
    public GameObject savePop;
    public GameObject loadingCanvas;

    [Header("Main & Loading")]
    public VideoPlayer videoplayer;
    public RawImage mainVideoTarget;
    public RawImage loadingVideoTarget;
    public VideoClip[] videoClips;

    [Header("Audio")]
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private static UIManage_Main instance;

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
        LoadVolumeSettings(); // ✅ 저장된 볼륨 값 불러오기
        MainVideo();
    }

    private void InitializeUI()
    {
        // ✅ 모든 패널 비활성화
        startPanel?.SetActive(false);
        optionPanel?.SetActive(false);
        quitPop?.SetActive(false);
        savePop?.SetActive(false);
        loadingCanvas?.SetActive(false);

        // ✅ 메인 패널만 활성화
        if (mainPanel != null)
        {
            mainPanel.SetActive(true);
        }

        // ✅ 오디오 재생
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Play();
        }

        // ✅ 슬라이더 값 변경 시 볼륨 조절
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // ✅ 메인 패널이 켜질 때마다 비디오 재생
    private void OnEnable()
    {
        if (mainPanel != null && mainPanel.activeSelf)
        {
            MainVideo();
        }
    }

    // ✅ 메인 씬 비디오 재생
    void MainVideo()
    {
        if (videoplayer != null && videoClips.Length > 0)
        {
            videoplayer.clip = videoClips[0]; // ✅ 메인 비디오 클립 설정
            if (mainVideoTarget != null)
            {
                videoplayer.targetTexture = mainVideoTarget.texture as RenderTexture;
            }
            videoplayer.isLooping = true; // ✅ 메인 비디오는 반복
            videoplayer.Play();
        }
    }

    // ✅ Quit 팝업에서 "Yes" 버튼 클릭 시 게임 종료
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ✅ 로딩 화면 비디오 재생 (씬 전환 전)
    public void PlayLoadingScreen()
    {
        // ✅ UI 변경
        if (loadingVideoTarget != null)
        {
            loadingVideoTarget.gameObject.SetActive(true); // ✅ 로딩 UI 활성화
        }

        // ✅ 비디오 클립 변경 및 반복 재생
        if (videoplayer != null && videoClips.Length > 1)
        {
            if (loadingCanvas != null)
            {
                loadingCanvas.SetActive(true);
            }

            videoplayer.clip = videoClips[1]; // ✅ 로딩 비디오 클립 설정
            if (loadingVideoTarget != null)
            {
                videoplayer.targetTexture = loadingVideoTarget.texture as RenderTexture;
            }
            videoplayer.isLooping = true; // ✅ 로딩 비디오는 무한 반복
            videoplayer.Play();
        }

        // ✅ 오디오 정지 (로딩 화면에서는 음악 X)
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
        }
        if(sfxAudioSource != null)
        {
            sfxAudioSource.Stop();
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
            PlayerPrefs.SetFloat("BGMVolume", volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);

            // 효과음 미리 듣기: 이미 재생 중이면 중단하고, 미리 듣기 코루틴 실행
            sfxAudioSource.Stop();
            StartCoroutine(PreviewSFX());
        }
    }

    private IEnumerator PreviewSFX()
    {
        // 미리 듣기를 위한 클립이 sfxAudioSource에 이미 할당되어 있어야 합니다.
        if (sfxAudioSource.clip != null)
        {
            sfxAudioSource.time = 0f;
            sfxAudioSource.Play();
            yield return new WaitForSeconds(2f); // 2초 동안 재생
            sfxAudioSource.Stop();
        }
    }

    private void LoadVolumeSettings()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = bgmVolume;
        }
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = sfxVolume;
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = bgmVolume;
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
        }
    }

    // 옵션창을 닫을 때 호출되는 함수
    public void CloseOptionPanel()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);

            // 옵션창이 닫힐 때 효과음 중간 재생이 있다면 정지
            if (sfxAudioSource != null && sfxAudioSource.isPlaying)
            {
                sfxAudioSource.Stop();
            }
        }
    }
    #endregion
}
