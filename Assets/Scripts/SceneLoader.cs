using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// <YSA> chatGPT
public class SceneLoader : MonoBehaviour
{
    public CanvasGroup canvasGroup; // ✅ UI 페이드 효과
    public VideoPlayer videoPlayer; // ✅ 비디오 플레이어
    public RawImage videoRawImage; // ✅ 비디오가 출력될 `RawImage`
    public Text loadingText; // ✅ "Loading..." 텍스트

    private void Start()
    {
        canvasGroup.alpha = 1; // ✅ 시작할 때 로딩 UI는 숨김

        // ✅ 비디오 출력 이미지의 초기 투명도 설정
        if (videoRawImage != null)
        {
            Color color = videoRawImage.color;
            color.a = 1f; // ✅ 처음엔 완전 보이게 설정
            videoRawImage.color = color;
        }
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadGameSceneCoroutine());
    }

    private IEnumerator LoadGameSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Test1");
        operation.allowSceneActivation = false; // 씬 자동 전환 막기

        //float fadeSpeed = 30f; // 페이드 인 속도 설정
        float targetAlpha = 1f; // 페이드 인 목표 알파 값

        // 씬 로딩이 10% 정도 진행될 때까지 페이드 인을 완료
        //while (operation.progress < 0.1f)
        //{
        //    // 페이드 인 처리: canvasGroup.alpha가 0에서 1로 증가
        //    canvasGroup.alpha += Time.deltaTime * fadeSpeed;

        //    if (videoRawImage != null)
        //    {
        //        Color color = videoRawImage.color;
        //        color.a += Time.deltaTime; // 비디오도 함께 페이드 인
        //        videoRawImage.color = color;
        //    }

        //    yield return null;
        //}

        // 페이드 인 완료 후 영상 시작
        if (videoPlayer != null)
        {
            videoPlayer.Play(); // 비디오 플레이 시작
        }

        float minimumWaitTime = Random.Range(3f, 6f);
        float startTime = Time.time;
        // 페이드 인 완료 후 씬 로딩 진행
        canvasGroup.alpha = targetAlpha; // 알파 값 강제로 1로 설정
        videoRawImage.color = new Color(videoRawImage.color.r, videoRawImage.color.g, videoRawImage.color.b, 1f); // 비디오도 강제로 알파 1로 설정

        // 나머지 씬 로딩은 계속 진행
        while (!operation.isDone)
        {
            while (Time.time - startTime < minimumWaitTime)
            {
                yield return null; // 매 프레임 체크하여 부드럽게 진행
            }
            if (operation.progress >= 0.9f) // 씬 로딩이 거의 끝날 때
            {
                //yield return new WaitForSeconds(1f); // 페이드 아웃을 위해 잠시 대기
                operation.allowSceneActivation = true; // ✅ 씬 전환
                //StartCoroutine(FadeOutAndLoadScene(operation)); // 페이드 아웃 후 씬 전환
                yield break; // 종료
            }

            yield return null; // 씬 로딩 진행 상태를 체크
        }
    }



    //private IEnumerator FadeIn()
    //{
    //    // ✅ 페이드 인 처리: canvasGroup.alpha가 0에서 1로 증가하도록
    //    while (canvasGroup.alpha < 1)
    //    {
    //        canvasGroup.alpha += Time.deltaTime; // 시간에 따라 알파 값 증가

    //        if (videoRawImage != null)
    //        {
    //            Color color = videoRawImage.color;
    //            color.a += Time.deltaTime; // 비디오도 함께 페이드 인
    //            videoRawImage.color = color;
    //        }

    //        yield return null;
    //    }
    //}

    //private IEnumerator FadeOutAndLoadScene(AsyncOperation operation)
    //{
    //    float fadeSpeed = 0.5f;

    //    while (canvasGroup.alpha > 0)
    //    {
    //        canvasGroup.alpha -= Time.deltaTime * fadeSpeed; // ✅ UI 페이드 아웃

    //        if (videoRawImage != null)
    //        {
    //            Color color = videoRawImage.color;
    //            color.a -= Time.deltaTime * fadeSpeed; // ✅ 비디오도 함께 투명하게
    //            videoRawImage.color = color;
    //        }

    //        yield return null;
    //    }

    //    //operation.allowSceneActivation = true; // ✅ 씬 전환
    //}
}
