using UnityEngine;
using TMPro;
using System.Collections.Generic;

// <YSA>
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
        // 시작 언어는 영어로 설정
        SetLanguage(englishAsset, fontEnglishPrimary, fontEnglishSecondary);
        // 드롭다운 옵션은 LanguageAsset에 등록된 "Language" 옵션을 그대로 사용
        UpdateDropdownOptions();
        // 시작 시 드롭다운의 선택은 현재 언어(영어)인 인덱스 0으로 설정
        languageDropdown.value = 0;
        languageDropdown.RefreshShownValue();
        // 드롭다운 값 변경 이벤트 등록
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    // 언어와 폰트 정보를 저장하고 UI 업데이트
    private void SetLanguage(LanguageAsset asset, TMP_FontAsset primaryFont, TMP_FontAsset secondaryFont)
    {
        currentLanguageAsset = asset;
        currentFontPrimary = primaryFont;
        currentFontSecondary = secondaryFont;
        ApplyLanguage();
    }

    // 모든 텍스트 오브젝트에 대해 번역, 폰트, 크기, 스타일 적용 및 드롭다운 업데이트
    private void ApplyLanguage()
    {
        for (int i = 0; i < textObjects.Count; i++)
        {
            if (textObjects[i] != null && i < keys.Count)
            {
                string key = keys[i];
                // 번역 적용
                textObjects[i].text = currentLanguageAsset.GetTranslation(key);
                // 보조 폰트 사용 여부에 따라 폰트 설정
                bool useSecondaryFont = currentLanguageAsset.UseSecondaryFont(key);
                textObjects[i].font = useSecondaryFont ? currentFontSecondary : currentFontPrimary;
                // 폰트 크기 설정 (설정된 경우)
                float? fontSize = currentLanguageAsset.GetFontSize(key);
                if (fontSize.HasValue)
                {
                    textObjects[i].fontSize = fontSize.Value;
                }
                // 볼드 적용
                textObjects[i].fontStyle = currentLanguageAsset.IsBold(key) ? FontStyles.Bold : FontStyles.Normal;
            }
        }

        // 드롭다운 옵션 및 폰트 업데이트 (LanguageAsset에 설정된 옵션 사용)
        UpdateDropdownOptions();
        UpdateDropdownFont();
    }

    // LanguageAsset에 등록된 드롭다운 옵션을 사용하여 드롭다운을 업데이트
    private void UpdateDropdownOptions()
    {
        if (currentLanguageAsset == null)
            return;

        languageDropdown.ClearOptions();
        foreach (var dropdown in currentLanguageAsset.dropdownTranslations)
        {
            if (dropdown.dropdownKey == "Language")
            {
                // 드롭다운 옵션을 그대로 추가 (예: 영어 상태면 ["English", "Korean"], 한국어 상태면 ["한국어", "영어"])
                languageDropdown.AddOptions(dropdown.options);
                break;
            }
        }
        languageDropdown.RefreshShownValue();
    }

    // 드롭다운의 폰트 및 폰트 크기 업데이트 (기존 코드 유지)
    private void UpdateDropdownFont()
    {
        if (currentLanguageAsset == null)
            return;

        float? labelFontSize = currentLanguageAsset.GetDropdownLabelFontSize();
        float? itemFontSize = currentLanguageAsset.GetDropdownItemFontSize();

        if (languageDropdown.captionText != null)
        {
            languageDropdown.captionText.font = currentFontSecondary;
            if (labelFontSize.HasValue)
            {
                languageDropdown.captionText.fontSize = labelFontSize.Value;
            }
        }

        if (languageDropdown.itemText != null)
        {
            languageDropdown.itemText.font = currentFontSecondary;
            if (itemFontSize.HasValue)
            {
                languageDropdown.itemText.fontSize = itemFontSize.Value;
            }
        }
    }

    // 드롭다운 값 변경 시 호출됨
    private void OnLanguageChanged(int index)
    {
        // index 0: 현재 언어(변경 없음) → 아무 작업도 하지 않음
        if (index == 0)
        {
            languageDropdown.value = 0;
            return;
        }
        // index 1: 반대 언어 선택
        if (index == 1)
        {
            // 현재 언어가 영어면 한국어로, 한국어면 영어로 전환
            if (currentLanguageAsset == englishAsset)
            {
                SetLanguage(koreanAsset, fontKoreanPrimary, fontKoreanSecondary);
            }
            else if (currentLanguageAsset == koreanAsset)
            {
                SetLanguage(englishAsset, fontEnglishPrimary, fontEnglishSecondary);
            }
            // 전환 후 드롭다운 옵션이 다시 업데이트되므로, 선택 값은 현재 언어(인덱스 0)로 고정
            languageDropdown.value = 0;
        }
    }
}
