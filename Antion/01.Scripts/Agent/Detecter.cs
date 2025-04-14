using Karin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Karin
{

    public class Detecter : MonoBehaviour
    {
        IDetecterUser _myBrain;

        public CircleCollider2D _collider;


        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _myBrain = GetComponentInParent<IDetecterUser>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _myBrain.DetectedObject.Add(collision.gameObject);
            _myBrain.DetectedObject = _myBrain.DetectedObject.OrderBy(t => Vector3.Distance(transform.position, collision.gameObject.transform.position)).ToList();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _myBrain.DetectedObject.Remove(collision.gameObject);
            _myBrain.DetectedObject = _myBrain.DetectedObject.OrderBy(t => Vector3.Distance(transform.position, collision.gameObject.transform.position)).ToList();

        }
    }
}