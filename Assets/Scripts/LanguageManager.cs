using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;
    public List<TextMeshProUGUI> textObjects;
    public List<string> keys;

    public LanguageAsset englishAsset;
    public LanguageAsset koreanAsset;

    public TMP_FontAsset fontEnglishPrimary;
    public TMP_FontAsset fontEnglishSecondary;
    public TMP_FontAsset fontKoreanPrimary;
    public TMP_FontAsset fontKoreanSecondary;

    private LanguageAsset currentLanguageAsset;
    private TMP_FontAsset currentFontPrimary;
    private TMP_FontAsset currentFontSecondary;

    private void Start()
    {
        LoadLanguage();
        // 🔹 드롭다운 값이 변경될 때 이벤트 리스너 추가
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void LoadLanguage()
    {
        SystemLanguage systemLanguage = Application.systemLanguage;

        if (systemLanguage == SystemLanguage.Korean)
        {
            SetLanguage(koreanAsset, fontKoreanPrimary, fontKoreanSecondary);
            languageDropdown.value = 1;
        }
        else
        {
            SetLanguage(englishAsset, fontEnglishPrimary, fontEnglishSecondary);
            languageDropdown.value = 0;
        }
    }
    private void SetLanguage(LanguageAsset asset, TMP_FontAsset primaryFont, TMP_FontAsset secondaryFont)
    {
        currentLanguageAsset = asset;
        currentFontPrimary = primaryFont;
        currentFontSecondary = secondaryFont;
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        for (int i = 0; i < textObjects.Count; i++)
        {
            if (textObjects[i] != null && i < keys.Count)
            {
                string key = keys[i];

                // 🔹 번역 적용
                textObjects[i].text = currentLanguageAsset.GetTranslation(key);

                // 🔹 폰트 설정 (보조 폰트 사용 여부 확인)
                bool useSecondaryFont = currentLanguageAsset.UseSecondaryFont(key);
                textObjects[i].font = useSecondaryFont ? currentFontSecondary : currentFontPrimary;

                // 🔹 폰트 크기 설정 (설정된 경우에만 적용)
                float? fontSize = currentLanguageAsset.GetFontSize(key);
                if (fontSize.HasValue)
                {
                    textObjects[i].fontSize = fontSize.Value;
                }

                // 🔹 볼드 적용
                textObjects[i].fontStyle = currentLanguageAsset.IsBold(key) ? FontStyles.Bold : FontStyles.Normal;
            }
        }

        // 🔹 드롭다운 옵션 및 폰트 업데이트
        UpdateDropdownOptions();
        UpdateDropdownFont();  // 🔹 추가됨 (언어 변경 시 폰트 크기도 즉시 반영)
    }

    private void UpdateDropdownOptions()
    {
        if (currentLanguageAsset == null) return;

        foreach (var dropdown in currentLanguageAsset.dropdownTranslations)
        {
            if (dropdown.dropdownKey == "Language") // 🔹 드롭다운 키값 확인!
            {
                languageDropdown.ClearOptions();
                languageDropdown.AddOptions(dropdown.options);
            }
        }

        // 🔹 드롭다운의 폰트 및 폰트 크기 변경 적용
        UpdateDropdownFont();
    }

    private void UpdateDropdownFont()
    {
        if (currentLanguageAsset == null) return;

        float? labelFontSize = currentLanguageAsset.GetDropdownLabelFontSize();
        float? itemFontSize = currentLanguageAsset.GetDropdownItemFontSize();

        // 🔹 드롭다운 라벨 (선택된 항목) 폰트 및 크기 적용
        if (languageDropdown.captionText != null)
        {
            languageDropdown.captionText.font = currentFontSecondary;
            if (labelFontSize.HasValue)
            {
                languageDropdown.captionText.fontSize = labelFontSize.Value;
            }
        }

        // 🔹 드롭다운 리스트 항목 (펼쳤을 때) 폰트 및 크기 적용
        if (languageDropdown.itemText != null)
        {
            languageDropdown.itemText.font = currentFontSecondary;
            if (itemFontSize.HasValue)
            {
                languageDropdown.itemText.fontSize = itemFontSize.Value;
            }
        }
    }

    private void OnLanguageChanged(int index)
    {
        if (index == 0) // 영어 선택
        {
            SetLanguage(englishAsset, fontEnglishPrimary, fontEnglishSecondary);
        }
        else if (index == 1) // 한국어 선택
        {
            SetLanguage(koreanAsset, fontKoreanPrimary, fontKoreanSecondary);
        }
    }
}
