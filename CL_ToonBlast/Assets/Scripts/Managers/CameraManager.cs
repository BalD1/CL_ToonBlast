using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera cam;

    private void Reset()
    {
        cam = this.GetComponent<Camera>();
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
