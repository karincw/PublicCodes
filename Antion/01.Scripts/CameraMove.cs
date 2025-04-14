using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private InputReaderSO inputreader;
        private Vector2 direction;
        [SerializeField] private float speed;

        private void OnEnable()
        {
            inputreader.CameraMovementEvent += Movement;
        }

        private void OnDisable()
        {
            inputreader.CameraMovementEvent -= Movement;
        }

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        private void Movement(Vector2 dir)
        {
            direction = dir.normalized;
        }
    }
}
