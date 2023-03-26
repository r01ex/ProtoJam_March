using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraFollowSpeed = 3;
    [SerializeField] private Transform destination;
    [SerializeField] private float cameraSize = 5.5f;
    private Camera mainCamera;
    private DOTweenAnimation shakeAnim;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        shakeAnim = GetComponent<DOTweenAnimation>();
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, destination.position, Time.deltaTime * cameraFollowSpeed);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSize, Time.deltaTime * cameraFollowSpeed);
    }

    //카메라 좌표 지정
    public void SetCameraDestination(Transform _transform, float _cameraSize = 5.5f)
    {
        destination = _transform;
        cameraSize = _cameraSize;
    }

    //카메라 캐릭터 추적
    public void SetCameraAtCharacter(float _cameraSize = 5)
    {
        destination = playerScript.Instance.transform;
        cameraSize = _cameraSize;
    }

    public void Do_ShakeCamera()
    {
        shakeAnim.DOPlay();
    }
}
