using UnityEngine;
using System.Collections.Generic;

// <YSA>
[System.Serializable]
public class LanguageData
{
    public string key;
    public string value;
    public float fontSize;
    public bool useSecondaryFont;
    public bool isBold; // 🔹 볼드 여부 추가
}

[System.Serializable]
public class DropdownData
{
    public string dropdownKey;
    public List<string> options;
}

[CreateAssetMenu(fileName = "LanguageAsset", menuName = "Localization/LanguageAsset")]
public class LanguageAsset : ScriptableObject
{
    public List<LanguageData> translations;
    public List<DropdownData> dropdownTranslations;

    public float dropdownLabelFontSize; // 🔹 선택된 항목의 폰트 크기
    public float dropdownItemFontSize;  // 🔹 옵션 목록의 폰트 크기

    private Dictionary<string, LanguageData> translationDict;
    private Dictionary<string, List<string>> dropdownDict;

    private void OnEnable()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        translationDict = new Dictionary<string, LanguageData>();
        dropdownDict = new Dictionary<string, List<string>>();

        if (translations != null)
        {
            foreach (var item in translations)
            {
                translationDict[item.key] = item;
            }
        }

        if (dropdownTranslations != null)
        {
            foreach (var dropdown in dropdownTranslations)
            {
                dropdownDict[dropdown.dropdownKey] = dropdown.options;
            }
        }
    }

    public string GetTranslation(string key)
    {
        if (translationDict.TryGetValue(key, out LanguageData data))
        {
            return data.value.Replace("\\n","\n");
        }
        return key;
    }

    public float? GetFontSize(string key)
    {
        if (translationDict.TryGetValue(key, out LanguageData data))
        {
            return data.fontSize > 0 ? data.fontSize : (float?)null;
        }
        return null;
    }

    public bool UseSecondaryFont(string key)
    {
        if (translationDict.TryGetValue(key, out LanguageData data))
        {
            return data.useSecondaryFont;
        }
        return false;
    }

    public bool IsBold(string key)
    {
        if (translationDict.TryGetValue(key, out LanguageData data))
        {
            return data.isBold;
        }
        return false;
    }

    public string[] GetDropdownOptions(string dropdownKey)
    {
        if (dropdownDict.TryGetValue(dropdownKey, out List<string> options))
        {
            return options.ToArray();
        }
        return new string[0];
    }

    public float? GetDropdownLabelFontSize()
    {
        return dropdownLabelFontSize > 0 ? dropdownLabelFontSize : (float?)null;
    }

    public float? GetDropdownItemFontSize()
    {
        return dropdownItemFontSize > 0 ? dropdownItemFontSize : (float?)null;
    }
}
