using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor; // �⺻ Ŀ�� �̹���
    public Texture2D clickCursor;  // Ŭ�� �� Ŀ�� �̹���

    private void Start()
    {
        // ���� ���� �� �⺻ Ŀ�� ����
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))  // ���� Ŭ��
        {
            // Ŭ�� �� Ŀ���� clickCursor�� ����
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))  // Ŭ�� ����
        {
            // Ŭ���� ���� �ٽ� normalCursor�� ����
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
