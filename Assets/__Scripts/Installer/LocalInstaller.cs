﻿using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LocalInstaller : MonoBehaviour
{
    public CameraManager cameraManager;
    public MapManager    mapManager;
    public Image         blackBoard;
    public RawImage[]    transitionTexture;
    public Image         shieldBlackBoard;
    public VolumeProfile volumeProfile;
    public ObjectPoolManager objectPoolManager;
    
    private void Awake()
    {
        var _container = new DIContainer();
        DIContainer.Local = _container;

        _container.Regist(cameraManager);
        _container.Regist(mapManager);
        _container.Regist(blackBoard, "BlackBoard");
        _container.Regist(transitionTexture);
        _container.Regist(shieldBlackBoard, "ShieldBlackBoard");
        _container.Regist(volumeProfile);
        _container.Regist(objectPoolManager);
    }
}