using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfelerselfRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 450f;

    Vector3 _origin;

    private void Awake()
    {
        _origin = transform.eulerAngles;
    }

    private void Update()
    {
        //현재 회전값을 가져옴
        Vector3 rotation = transform.eulerAngles;

        //현재 회전값을 회전 속도를 더한 값으로 세팅
        rotation.y += rotationSpeed * Time.deltaTime;

        //회전값을 transform에 적용
        transform.localRotation = Quaternion.Euler(_origin.x, rotation.y, _origin.z);
    }
}
