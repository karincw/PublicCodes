using Karin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentDetecter : MonoBehaviour
{
    IDetecterUser _agent;

    public CircleCollider2D _collider;
    private List<GameObject> detectedObject = new List<GameObject>();

    [SerializeField] private string findObjectTag;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _agent = GetComponentInParent<IDetecterUser>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != findObjectTag)
        { return; }
        detectedObject.Add(collision.gameObject);
        detectedObject = detectedObject.OrderBy(t => Vector3.Distance(transform.position, collision.gameObject.transform.position)).ToList();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (detectedObject.Count > 0)
            _agent.DetectedObject = detectedObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObject.Remove(collision.gameObject);
        detectedObject = detectedObject.OrderBy(t => Vector3.Distance(transform.position, collision.gameObject.transform.position)).ToList();

    }
}
