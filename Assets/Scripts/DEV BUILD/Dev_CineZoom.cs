using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Dev_CineZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineComponentBase componentBase;
    float cameraDistance;

    void Start()
    {
        
    }

    void ZoomTime()
    {
        if (componentBase == null)
        {
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }

        (componentBase as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
    }
}
