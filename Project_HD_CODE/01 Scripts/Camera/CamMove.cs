using System;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;

    private Rigidbody _rigidbody;

    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private int ScrollSpeed = 1;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputReader.CameraMovementEvent += CameraMovement;
        _inputReader.CameraScrollEvent += CameraZoom;
    }
    private void OnDisable()
    {
        _inputReader.CameraMovementEvent -= CameraMovement;
        _inputReader.CameraScrollEvent -= CameraZoom;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 2.2f, 4f), transform.position.z);
    }

    private void CameraZoom(Vector2 vector)
    {
        float value = vector.y;
        if ((transform.position.y >= 3.95f && value < 0) || (transform.position.y <= 2.35f && value > 0)) return;
        _rigidbody.velocity = new Vector3(0, value * -1.5f, value).normalized * ScrollSpeed;
    }

    private void CameraMovement(Vector2 dir)
    {
        _rigidbody.velocity = new Vector3(dir.x, 0, dir.y).normalized * movementSpeed;
    }
}
