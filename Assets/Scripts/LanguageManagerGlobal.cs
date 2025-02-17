using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Rendering;

[System.Serializable]
public class LanguageOption
{
    public string languageName;         // 예: "English", "Korean"
    public LanguageAsset languageAsset;
    public TMP_FontAsset primaryFont;
    public TMP_FontAsset secondaryFont;
}

public class LanguageManagerGlobal : MonoBehaviour
{
    public static LanguageManagerGlobal Instance;

    public List<LanguageOption> languageOptions;

    //public LanguageAsset englishAsset;
    //public LanguageAsset koreanAsset;
    //public TMP_FontAsset fontEnglishPrimary;
    //public TMP_FontAsset fontEnglishSecondary;
    //public TMP_FontAsset fontKoreanPrimary;
    //public TMP_FontAsset fontKoreanSecondary;

    private TMP_Dropdown languageDropdown; // 드롭다운 참조 (씬에 있을 수도 있고 없을 수도 있으므로 null 체크)

    private LanguageAsset currentLanguageAsset;
    private TMP_FontAsset currentFontPrimary;
    private TMP_FontAsset currentFontSecondary;
    private string selectedLanguage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("HasLaunchedBefore"))
            {
                PlayerPrefs.SetInt("HasLaunchedBefore", 1);
                PlayerPrefs.DeleteKey("SelectedLanguage"); // 첫 실행 때만 초기화
                PlayerPrefs.Save();
            }

            LoadLanguageSetting();
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드될 때 UI 자동 등록
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void LoadLanguageSetting()
    {
        selectedLanguage = PlayerPrefs.GetString("SelectedLanguage", languageOptions[0].languageName);
        ApplySavedLanguage();
    }

    private void ApplySavedLanguage()
    {
        LanguageOption option = languageOptions.Find(x => x.languageName == selectedLanguage);
        if (option != null)
        {
            SetLanguage(option.languageAsset, option.primaryFont, option.secondaryFont);
        }
        else
        {
            // 기본값 설정: 첫 번째 옵션
            SetLanguage(languageOptions[0].languageAsset, languageOptions[0].primaryFont, languageOptions[0].secondaryFont);
        }
    }

    private void SetLanguage(LanguageAsset asset, TMP_FontAsset primaryFont, TMP_FontAsset secondaryFont)
    {
        currentLanguageAsset = asset;
        currentFontPrimary = primaryFont;
        currentFontSecondary = secondaryFont;
        RegisterAllUIObjects(); // UI 자동 등록
        UpdateAllText();
    }

    public void ChangeLanguage(int index)
    {
        if (index < 0 || index >= languageOptions.Count) return;

        LanguageOption option = languageOptions[index];
        selectedLanguage = option.languageName;
        PlayerPrefs.SetString("SelectedLanguage", selectedLanguage);
        PlayerPrefs.Save();
        SetLanguage(option.languageAsset, option.primaryFont, option.secondaryFont);
    }

    private void UpdateAllText()
    {
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var textObj in texts)
        {
            TranslationKey keyComponent = textObj.GetComponent<TranslationKey>();

            // ✅ TranslationKey가 존재하면 key 사용, 없으면 스킵
            if (keyComponent != null && !string.IsNullOrEmpty(keyComponent.translationKey))
            {
                string key = keyComponent.translationKey;

                if (currentLanguageAsset.translationDict.ContainsKey(key))
                {
                    textObj.text = currentLanguageAsset.GetTranslation(key);

                    // ✅ 폰트 설정
                    bool useSecondaryFont = currentLanguageAsset.UseSecondaryFont(key);
                    textObj.font = useSecondaryFont ? currentFontSecondary : currentFontPrimary;

                    // ✅ 폰트 크기 설정 (있으면 적용)
                    float? fontSize = currentLanguageAsset.GetFontSize(key);
                    if (fontSize.HasValue) textObj.fontSize = fontSize.Value;

                    // ✅ 볼드 처리
                    textObj.fontStyle = currentLanguageAsset.IsBold(key) ? FontStyles.Bold : FontStyles.Normal;
                }
                else
                {
                    Debug.LogWarning($"⚠️ 번역 Key 없음: {key}");
                }
            }
        }
        // 드롭다운 업데이트 (씬에 드롭다운이 있다면)
        if (languageDropdown != null)
        {
            UpdateDropdownOptions();
            UpdateDropdownFont();
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterAllUIObjects(); // 씬이 바뀔 때마다 UI 다시 찾기
        UpdateAllText();
        SetupDropdown();
    }
    private void RegisterAllUIObjects()
    {
        currentLanguageAsset.ClearUIObjects(); // 기존 UI 목록 초기화
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var textObj in texts)
        {
            string key = textObj.gameObject.name;
            currentLanguageAsset.RegisterUIObject(key, textObj);
        }
    }

    // 드롭다운 초기 설정
    private void SetupDropdown()
    {
        languageDropdown = FindObjectOfType<TMP_Dropdown>(true);
        if (languageDropdown != null)
        {
            List<string> options = new List<string>();
            foreach (var opt in languageOptions)
            {
                options.Add(opt.languageName);
            }

            languageDropdown.AddOptions(options);

            // 현재 저장된 언어에 맞게 드롭다운의 인덱스 설정
            int currentIndex = languageOptions.FindIndex(x => x.languageName == selectedLanguage);
            if (currentIndex == -1)
            {
                Debug.LogError($"❌ 언어 '{selectedLanguage}'를 찾을 수 없음! 기본값(0)으로 설정");
                currentIndex = 0;  // 기본값으로 설정
            }

            languageDropdown.onValueChanged.RemoveAllListeners();
            languageDropdown.onValueChanged.AddListener(ChangeLanguage);
            languageDropdown.value = currentIndex >= 0 ? currentIndex : 0;
            languageDropdown.RefreshShownValue();
        }
    }

    private void UpdateDropdownOptions()
    {
        if (languageDropdown == null) return;

        languageDropdown.ClearOptions();
        foreach (var dropdown in currentLanguageAsset.dropdownTranslations)
        {
            if (dropdown.dropdownKey == "Language")
            {
                int selectedindex = languageOptions.FindIndex(x => x.languageName == selectedLanguage);
                if (selectedindex > 0)
                {
                    //선택된 언어를 리스트 맨 앞으로 이동
                    LanguageOption selecteOption = languageOptions[selectedindex];
                    languageOptions.RemoveAt(selectedindex);
                    languageOptions.Insert(0, selecteOption);
                }
                // 드롭다운 옵션을 그대로 추가 (예: 영어 상태면 ["English", "Korean"], 한국어 상태면 ["한국어", "영어"])
                languageDropdown.AddOptions(dropdown.options);
                break;
            }
        }
        languageDropdown.RefreshShownValue();
    }

    private void UpdateDropdownFont()
    {
        if (languageDropdown == null) return;

        float? labelFontSize = currentLanguageAsset.GetDropdownLabelFontSize();
        float? itemFontSize = currentLanguageAsset.GetDropdownItemFontSize();

        if (languageDropdown.captionText != null)
        {
            languageDropdown.captionText.font = currentFontSecondary;
            if (labelFontSize.HasValue)
                languageDropdown.captionText.fontSize = labelFontSize.Value;
        }

        if (languageDropdown.itemText != null)
        {
            languageDropdown.itemText.font = currentFontSecondary;
            if (itemFontSize.HasValue)
                languageDropdown.itemText.fontSize = itemFontSize.Value;
        }
    }
}
