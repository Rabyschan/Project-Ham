
using UnityEngine;

// <CAY> //<YSA>

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름

    // 키보드 감지 결과를 저장하는 변수
    // 변수의 읽기는 바깥에서도 자유롭게 할 수 있으나,
    // 변수의 값 수정은 이 클래스 내에서만 가능하다.
    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        //move, rotate, fire, reload 입력 여부를 실시간으로 체크
        // => GameOver일 때에는 실행되면 안 된다. => 예외 처리 넣기
        // --------------------------------------------------

        //앞뒤 입력값 감지해서 move에 넣기. (-1이면 후진, 1이면 전진)
        move = Input.GetAxis("Vertical");

        //좌우 입력값 감지해서 rotate에 넣기
        rotate = Input.GetAxis(rotateAxisName);
    }

    //마우스 스크롤 하면 카메라가 가까워짐
}
