using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMgr : SingletonMB<FaceMgr>
{
    [SerializeField]
    private FaceDetector faceDetector;

    public delegate void FaceDetectedHandler(Texture2D texture);
    // Face Detected Event
    [HideInInspector]
    public FaceDetectedHandler OnFaceDetected;

    protected override void Initialize()
    {
    }

    public void StopDetect()
    {
        Destroy(faceDetector.gameObject);
    }
}
