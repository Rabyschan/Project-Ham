using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] CameraType camType;
    [SerializeField] GameObject hamster;
    [SerializeField] SkinnedMeshRenderer hamsterMesh;
    [SerializeField] CinemachineDollyCart cart;
    [SerializeField] Transform hamsterPosition;

    private void OnTriggerEnter(Collider other)
    {
        switch(camType)
        {
            case CameraType.HAMSTER:
                // 햄스터 메쉬렌더러 활성화
                // 햄스터 위치를 도착지점으로 옮기기
                // 돌리 카트 속도 0

                hamster.SetActive(true);
                hamster.transform.position = hamsterPosition.position;
                cart.m_Speed = 0;


                break;

            case CameraType.AIR_PLANE:
                // 햄스터 메쉬렌더러 비활성화
                // 돌리 카트 속도 올리기
                hamster.SetActive(false);
                cart.m_Speed = 0.5f;
                break;
        }    

        CameraSet.Instance.ChangeCamera(camType);
    }
}
