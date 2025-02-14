//using TMPro;
//using UnityEngine;

//public class LanguageUIText : MonoBehaviour
//{
//    public string translationKey;  // 번역 키
//    private TextMeshProUGUI textComponent;

//    private void Awake()
//    {
//        textComponent = GetComponent<TextMeshProUGUI>();

//        if (LanguageManager.Instance != null)
//        {
//            LanguageManager.Instance.RegisterTextObject(this);
//        }
//    }

//    private void OnEnable()
//    {
//        UpdateText();  // UI 활성화 시 번역 적용
//    }

//    public void UpdateText()
//    {
//        if (textComponent != null && LanguageManager.Instance != null)
//        {
//            // 번역된 텍스트 적용
//            textComponent.text = LanguageManager.Instance.currentLanguageAsset.GetTranslation(translationKey);

//            // 폰트 설정 (기본 폰트 vs 보조 폰트)
//            bool useSecondaryFont = LanguageManager.Instance.currentLanguageAsset.UseSecondaryFont(translationKey);
//            textComponent.font = useSecondaryFont ? LanguageManager.Instance.currentFontSecondary : LanguageManager.Instance.currentFontPrimary;

//            // 폰트 크기 적용
//            float? fontSize = LanguageManager.Instance.currentLanguageAsset.GetFontSize(translationKey);
//            if (fontSize.HasValue)
//            {
//                textComponent.fontSize = fontSize.Value;
//            }

//            // 볼드 적용
//            textComponent.fontStyle = LanguageManager.Instance.currentLanguageAsset.IsBold(translationKey) ? FontStyles.Bold : FontStyles.Normal;
//        }
//    }
//}
