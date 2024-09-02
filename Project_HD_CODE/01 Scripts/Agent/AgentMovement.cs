using DG.Tweening;
using System;
using UnityEngine;

namespace Karin.AStar
{
    public class AgentMovement : MonoBehaviour
    {

        Sequence seq;
        [Header("Attribute")]
        [SerializeField] private float _moveTime = 1.0f;
        private AgentHex _agentHex;

        private Action _callbackEvent;

        private void Awake()
        {
            _agentHex = GetComponent<AgentHex>();
        }

        public void Movement(Vector3 dir, Action callbackAction = null)
        {
            seq = DOTween.Sequence();
            _callbackEvent = callbackAction;
            if (dir != Vector3.zero)
            {
                _agentHex.ChangeSprite(dir);
                seq.Append(transform.DOMove(transform.position + dir, _moveTime))
                    .AppendCallback(() =>
                    {
                        _callbackEvent?.Invoke();
                    });
            }
            else
            {
                _agentHex.SightDefine();
                _agentHex.DidItMove = true;
            }

        }



    }
}
