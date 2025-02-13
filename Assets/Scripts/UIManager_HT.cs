using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/*<YSA> ������ �� �����÷��̾�� ���� �ڵ������� ���
* Main (Start/Option/Quit ��ư)
* Start (Slot Empty => Ŭ�� �� Save �˾�) 
* -> save �˾� yes => �ε� ���� ���� (LoadScene ����)/ no => �˾� ����
* Option ([�����г�]BGM ���� �����̴�/ SFX ���� �����̴� / ��� ��Ӵٿ�(���� �Ϸ�)/
* ��Ʈ�� ��ư=> [��Ʈ�� �г�] Ű��)
* Quit (Quit �˾� yes => ���� ���� / no => �˾�����)
*/
public class UIManager_HT : MonoBehaviour
{
    // ---------- Video & Sound ----------
    [Header("Video & Sound")]
    public VideoPlayer videoPlayer;   // ���� �� �ڵ� ����� ���� �÷��̾�
    public AudioSource audioSource;   // ���� �� �ڵ� ����� ���� (BGM)

    // ---------- UI Panels & Buttons ----------
    [Header("Main Panel")]
    public GameObject mainPanel;      // ���� �г� (Start, Option, Quit ��ư ����)
    public Button startButton;        // Start ��ư
    public Button optionButton;       // Option ��ư
    public Button quitButton;         // Quit ��ư

    [Header("Save Popup")]
    public GameObject savePopup;      // Save �˾� (������ ��� ���� �� ǥ��)
    public Button saveYesButton;      // Save �˾��� Yes ��ư (�ε� ���� ����)
    public Button saveNoButton;       // Save �˾��� No ��ư (�˾� �ݱ�)

    [Header("Option Panel")]
    public GameObject optionPanel;    // Option �г� (BGM/SFX �����̴�, ��� ��Ӵٿ�, ��Ʈ�� ��ư ����)
    public Slider bgmSlider;          // BGM ���� �����̴�
    public Slider sfxSlider;          // SFX ���� �����̴�
    public Dropdown languageDropdown; // ��� ��Ӵٿ� (���� �Ϸ�)
    public Button controlButton;      // ��Ʈ�� ��ư (��Ʈ�� �г� ����)

    [Header("Control Panel")]
    public GameObject controlPanel;   // ��Ʈ�� �г�

    [Header("Quit Popup")]
    public GameObject quitPopup;      // Quit �˾�
    public Button quitYesButton;      // Quit �˾��� Yes ��ư (���� ����)
    public Button quitNoButton;       // Quit �˾��� No ��ư (�˾� �ݱ�)

    // ---------- ��Ÿ ���� ----------
    private bool isSaveSlotEmpty = true; // ���� ������ ��� �ִ��� ����

    private void Start()
    {
        InitializeUI();
        RegisterEvents();
        PlayMedia();
    }

    // UI �ʱ� ���� ����
    void InitializeUI()
    {
        if (mainPanel) mainPanel.SetActive(true);
        if (savePopup) savePopup.SetActive(false);
        if (optionPanel) optionPanel.SetActive(false);
        if (controlPanel) controlPanel.SetActive(false);
        if (quitPopup) quitPopup.SetActive(false);
    }

    // ��ư �� �����̴� �̺�Ʈ ���
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
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(value => Debug.Log("SFX ����: " + value));
    }

    // ������ ���� �ڵ� ���
    void PlayMedia()
    {
        if (videoPlayer) videoPlayer.Play();
        if (audioSource) audioSource.Play();
    }

    // Start ��ư Ŭ�� �̺�Ʈ
    void OnStartButtonClicked()
    {
        if (isSaveSlotEmpty)
        {
            if (savePopup) savePopup.SetActive(true);
        }
        else
        {
            // ���Կ� �����Ͱ� �ִٸ� �ٷ� �� �ε� ����
            LoadGameScene();
        }
    }

    // Save �˾� Yes ��ư Ŭ�� �̺�Ʈ
    void OnSaveYesButtonClicked()
    {
        if (savePopup) savePopup.SetActive(false);
        LoadGameScene();
    }

    // Quit �˾� Yes ��ư Ŭ�� �̺�Ʈ
    void OnQuitYesButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // �� �ε� ��û (SceneLoader ��ũ��Ʈ ���)
    void LoadGameScene()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadGameScene();
        }
        else
        {
            Debug.LogWarning("SceneLoader ��ũ��Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    // �޸� ������ ���� ���� �̺�Ʈ ����
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
