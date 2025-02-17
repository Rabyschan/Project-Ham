using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager_HT : MonoBehaviour
{
    private static UIManager_HT instance;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject startPanel;
    public GameObject loadingCanvas;
    public GameObject gameUICanvas;
    public GameObject pausePanel;
    public GameObject saveLoadPanel;
    public GameObject optionPanel;
    public GameObject quitPopup;
    public GameObject savePopup;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public RawImage mainVideoTarget;
    public RawImage loadingVideoTarget;
    public VideoClip[] videoClips;

    [Header("Audio")]
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private bool isPaused = false;
    public static GameObject FindInActiveObjectByName(string name)
    {
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objs)
        {
            if (obj.name == name)
                return obj;
        }
        return null;
    }
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
        LoadVolumeSettings();
        SceneSetup();
    }

    private void Update()
    {
        // ESC 입력 감지는 게임 씬에서만 실행
        if (SceneManager.GetActiveScene().name != "MainMenu" && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    // ✅ 씬이 변경될 때마다 실행되는 함수
    private void SceneSetup()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // 🔹 씬이 바뀌었으므로, UI 요소를 다시 찾음
        FindUIElements();

        if (currentScene == "MainMenu")
        {
            SetupMainMenu();
        }
        else
        {
            SetupGameScene();
        }
    }

    // ✅ 씬이 변경될 때 새 UI 요소 자동으로 찾기
    private void FindUIElements()
    {
        mainPanel = FindInActiveObjectByName("Main");
        startPanel = FindInActiveObjectByName("Start");
        loadingCanvas = FindInActiveObjectByName("LoadingCanvas");
        gameUICanvas = FindInActiveObjectByName("GameUICanvas");
        pausePanel = FindInActiveObjectByName("Pause");
        saveLoadPanel = FindInActiveObjectByName("SaveLoad");
        optionPanel = GameObject.Find("Option");
        quitPopup = FindInActiveObjectByName("Quit");
        savePopup = FindInActiveObjectByName("Save");

        videoPlayer = FindObjectOfType<VideoPlayer>();
        mainVideoTarget = FindInActiveObjectByName("Main Video Target")?.GetComponent<RawImage>();
        loadingVideoTarget = FindInActiveObjectByName("Loading Video Target")?.GetComponent<RawImage>();

        bgmAudioSource = FindInActiveObjectByName("BGM Audio Source")?.GetComponent<AudioSource>();
        sfxAudioSource = FindInActiveObjectByName("SFX Audio Source")?.GetComponent<AudioSource>();
        bgmSlider = FindInActiveObjectByName("BGM_Slider")?.GetComponent<Slider>();
        sfxSlider = FindInActiveObjectByName("SFX_Slider")?.GetComponent<Slider>();

        // 🔹 슬라이더 이벤트 리스너를 다시 등록하여 볼륨 조절 가능하도록 설정
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveAllListeners(); // 기존 이벤트 제거
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f); // 저장된 볼륨 값 적용
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners(); // 기존 이벤트 제거
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f); // 저장된 볼륨 값 적용
        }

        Debug.Log("🔄 UI 요소 및 오디오 설정 자동 연결 완료!");
    }

    // ✅ 메인 메뉴 전용 설정
    private void SetupMainMenu()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (startPanel != null) startPanel.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
        if (gameUICanvas != null) gameUICanvas.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (saveLoadPanel != null) saveLoadPanel.SetActive(false);
        if (optionPanel != null) optionPanel.SetActive(false);
        if (quitPopup != null) quitPopup.SetActive(false);
        if (savePopup != null) savePopup.SetActive(false);

        PlayMainVideo();
    }

    // ✅ 게임 씬 전용 설정
    private void SetupGameScene()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (startPanel != null) startPanel.SetActive(false);
        if (loadingCanvas != null) loadingCanvas.SetActive(false);
        if (gameUICanvas != null) gameUICanvas.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (saveLoadPanel != null) saveLoadPanel.SetActive(false);
        if (optionPanel != null) optionPanel.SetActive(false);
        if (quitPopup != null) quitPopup.SetActive(false);
        if (savePopup != null) savePopup.SetActive(false);
    }

    // ✅ 메인 메뉴 비디오 실행
    private void PlayMainVideo()
    {
        if (videoPlayer != null && videoClips.Length > 0)
        {
            videoPlayer.clip = videoClips[0]; // ✅ 메인 비디오 클립 설정
            if (mainVideoTarget != null)
            {
                videoPlayer.targetTexture = mainVideoTarget.texture as RenderTexture;
            }
            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }
    }

    // ✅ 로딩 화면 실행 (씬 전환 전에 호출)
    public void PlayLoadingScreen()
    {
        if (loadingVideoTarget != null)
        {
            loadingVideoTarget.gameObject.SetActive(true);
        }

        if (videoPlayer != null && videoClips.Length > 1)
        {
            loadingCanvas?.SetActive(true);
            videoPlayer.clip = videoClips[1]; // ✅ 로딩 비디오 클립 설정
            if (loadingVideoTarget != null)
            {
                videoPlayer.targetTexture = loadingVideoTarget.texture as RenderTexture;
            }

            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }

        if (bgmAudioSource != null) bgmAudioSource.Stop();
        if (sfxAudioSource != null) sfxAudioSource.Stop();
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

        if (bgmAudioSource != null) bgmAudioSource.volume = bgmVolume;
        if (sfxAudioSource != null) sfxAudioSource.volume = sfxVolume;

        if (bgmSlider != null)
        {
            bgmSlider.value = bgmVolume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume); // ✅ 슬라이더 이벤트 추가
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume); // ✅ 슬라이더 이벤트 추가
        }
    }

    // ✅ ESC 입력 시 Pause 창 열기/닫기
    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (gameUICanvas != null) gameUICanvas.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = isPaused ? 0 : 1; // ✅ 게임 멈춤/재개
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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

    // ✅ 씬이 변경될 때마다 실행되도록 설정
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneSetup();
    }
}
