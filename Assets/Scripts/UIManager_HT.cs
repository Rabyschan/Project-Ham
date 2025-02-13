using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/*<YSA> 시작할 때 비디오플레이어랑 사운드 자동적으로 재생
* Main (Start/Option/Quit 버튼)
* Start (Slot Empty => 클릭 시 Save 팝업) 
* -> save 팝업 yes => 로딩 비디오 띄우기 (LoadScene 참고)/ no => 팝업 끄기
* Option ([게임패널]BGM 조절 슬라이더/ SFX 조절 슬라이더 / 언어 드롭다운(구현 완료)/
* 컨트롤 버튼=> [컨트롤 패널] 키기)
* Quit (Quit 팝업 yes => 게임 종료 / no => 팝업끄기)
*/
public class UIManager_HT : MonoBehaviour
{
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
    }

}
