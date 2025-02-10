using UnityEngine;
using Cinemachine;

public enum CameraType
{
    HAMSTER,
    AIR_PLANE
}

public class CameraSet : MonoBehaviour
{
    // ΩÃ±€≈Ê
    private static CameraSet _instance;
    public static CameraSet Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CameraSet>();
            return _instance;
        }
    }

    [SerializeField] private CinemachineVirtualCamera hamsterCam;
    [SerializeField] private CinemachineVirtualCamera airPlaneCam;
    [SerializeField] private CinemachineBrain brain;

    public void ChangeCamera(CameraType type)
    {
        switch (type)
        {
            case CameraType.HAMSTER:
                hamsterCam.Priority = 1;
                airPlaneCam.Priority = 0;
                break;

            case CameraType.AIR_PLANE:
                hamsterCam.Priority = 0;
                airPlaneCam.Priority = 1;
                break;
        }
    }
}
