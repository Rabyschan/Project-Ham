
using UnityEngine;

// <CAY> //<YSA>

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�

    // Ű���� ���� ����� �����ϴ� ����
    // ������ �б�� �ٱ������� �����Ӱ� �� �� ������,
    // ������ �� ������ �� Ŭ���� �������� �����ϴ�.
    public float move { get; private set; } // ������ ������ �Է°�
    public float rotate { get; private set; } // ������ ȸ�� �Է°�

    // �������� ����� �Է��� ����
    private void Update()
    {
        //move, rotate, fire, reload �Է� ���θ� �ǽð����� üũ
        // => GameOver�� ������ ����Ǹ� �� �ȴ�. => ���� ó�� �ֱ�
        // --------------------------------------------------

        //�յ� �Է°� �����ؼ� move�� �ֱ�. (-1�̸� ����, 1�̸� ����)
        move = Input.GetAxis("Vertical");

        //�¿� �Է°� �����ؼ� rotate�� �ֱ�
        rotate = Input.GetAxis(rotateAxisName);
    }

    //���콺 ��ũ�� �ϸ� ī�޶� �������
}
