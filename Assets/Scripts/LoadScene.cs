using UnityEngine;
using UnityEngine.UI;

// <YSA> chatGPT
public class LoadScene : MonoBehaviour
{
    //public Button startButton; // ✅ 버튼 오브젝트
    public GameObject save_Pop;
    public SceneLoader sceneLoader; // ✅ SceneLoader 스크립트 참조
    public UIManager_HT uiManager_HT; // ✅ UIManager 스크립트 참조

    //private void Start()
    //{
    //    // ✅ 버튼 클릭 이벤트를 코드에서 직접 등록 (OnClick 사용 X)
    //    startButton.onClick.AddListener(StartGame);
    //}

    public void StartGame()
    {
        //startButton.gameObject.SetActive(false); // ✅ 버튼 비활성화 (사라짐)
        save_Pop.SetActive(false);
        // ✅ UIManager에서 로딩 화면 실행 (비디오 변경 & UI 활성화)
        if (uiManager_HT != null)
        {
            uiManager_HT.PlayLoadingScreen();
        }

        sceneLoader.LoadGameScene(); // ✅ SceneLoader의 씬 로딩 실행
    }

    //private void OnDestroy()
    //{
        // ✅ 씬이 변경될 때 메모리 누수를 방지하기 위해 이벤트 제거
        //startButton.onClick.RemoveListener(StartGame);
    //}
}
