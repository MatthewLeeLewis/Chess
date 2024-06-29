using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// This script is attached tot he object the cinemachine camera follows, to control its movement.

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    private const float MIN_X = -13f;
    private const float MAX_X = 27f;
    private const float MIN_Z = -20f;
    private const float MAX_Z = 33f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsAITurn())
        {
            Vector3 rotationVector = new Vector3(0, 180f, 0);
            transform.eulerAngles += rotationVector;
            transform.Translate(0f, 0f, -12f);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

        if (transform.position.x < MIN_X)
        {
            transform.position = new Vector3(MIN_X, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > MAX_X)
        {
            transform.position = new Vector3(MAX_X, transform.position.y, transform.position.z);
        }
        if (transform.position.z < MIN_Z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, MIN_Z);
        }
        else if (transform.position.z > MAX_Z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, MAX_Z);
        }
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.EitherIsAI())
        {
            Vector3 rotationVector = new Vector3(0, 180f, 0);
            transform.eulerAngles += rotationVector;
            transform.Translate(0f, 0f, -12f);  
        }
    }
}
