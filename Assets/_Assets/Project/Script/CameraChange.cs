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
                // �ܽ��� �޽������� Ȱ��ȭ
                // �ܽ��� ��ġ�� ������������ �ű��
                // ���� īƮ �ӵ� 0

                hamster.SetActive(true);
                hamster.transform.position = hamsterPosition.position;
                cart.m_Speed = 0;


                break;

            case CameraType.AIR_PLANE:
                // �ܽ��� �޽������� ��Ȱ��ȭ
                // ���� īƮ �ӵ� �ø���
                hamster.SetActive(false);
                cart.m_Speed = 0.5f;
                break;
        }    

        CameraSet.Instance.ChangeCamera(camType);
    }
}
