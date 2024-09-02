using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Karin.AStar
{
    [RequireComponent(typeof(LineRenderer))]
    public class NavAgent : MonoBehaviour
    {

        public UnityEvent<Vector3, Action> MovementEvent;

        private PriorityQueue<AstarNode> _openList; //���� �����ִ� �͵��� ��Ƴ�����
        private List<AstarNode> _closeList; // �ѹ��̶� �湮�� ��带 ��Ƴ�����
        private List<Vector3Int> _routePath; //���� ������ ��θ� Ÿ������������ ������ �ִ°�

        private int _moveIdx = 0;

        private Vector3Int _currentPosition; //���� Ÿ�� ������
        private Vector3Int _destPosition; //��ǥ ������
        public Vector3 _nextPosition;
        public Vector3Int Destination
        {
            get => _destPosition;
            set
            {
                if (Destination == value) return; //�������� �Ⱥ���
                SetCurrentPosition();
                _destPosition = value;
                if (CalculatePath())
                {
                    GetNextPath();
                }
            }
        }

        public void GetNextPath()
        {
            if (_moveIdx >= _routePath.Count)
            {
                MovementEvent?.Invoke(Vector3.zero, null);
                _nextPosition += Vector3.up;
                return;
            }
            _currentPosition = _routePath[_moveIdx];
            _nextPosition = HexCoordinates.ConvertOffsetToPosition(_currentPosition);

            Vector3 targetDir = _nextPosition - transform.position;
            if (_currentPosition.z % 2 == 1)
            {
                targetDir += new Vector3(-0.5f, 0, 0);
            }
            MovementEvent?.Invoke(targetDir, () => GetNextPath());

            _moveIdx++;
        }

        private void Awake()
        {
            _closeList = new List<AstarNode>();
            _routePath = new List<Vector3Int>();
            _openList = new PriorityQueue<AstarNode>();
        }

        private void Start()
        {
            SetCurrentPosition();
            transform.position = HexCoordinates.ConvertOffsetToPosition(_currentPosition);
            if (_currentPosition.z % 2 == 1)
            {
                transform.position += new Vector3(-0.5f, 0, 0);
            }
            //���� ��ǥ�� Ÿ����ǥ �߽����� �̵�
        }

        public void StartAstar(Vector3Int coord)
        {
            Destination = coord;
        }
        public void StartAstar(Vector3 world)
        {
            Destination = HexCoordinates.ConvertPositionToOffset(world);
        }

        private void SetCurrentPosition()
        {
            _currentPosition = HexCoordinates.ConvertPositionToOffset(transform.position);
        }

        #region Astar�� ��ǻ� ����

        private bool CalculatePath()
        {
            _openList.Clear();
            _closeList.Clear();

            _openList.Push(new AstarNode()
            {
                Position = _currentPosition,
                Parent = null,
                G = 0,
                F = CalculateH(_currentPosition)
            });

            bool result = false;

            int cnt = 0;
            while (_openList.Count > 0) //�����Ұ� ����
            {
                AstarNode node = _openList.Pop();
                FindOpenList(node);
                _closeList.Add(node); //�湮�ѳ��ϱ� closeList�� ��

                if (_destPosition == node.Position)//�湮�� ��尡 ���������ٸ� �����ѰŴϱ� �����
                {
                    result = true;
                    break;
                }


                cnt++;
                if (cnt > 10000)
                {
                    Debug.Log("����");
                    break;
                }
            }

            if (result == true)
            {
                _routePath.Clear();
                AstarNode node = _closeList.Last();
                while (node.Parent != null)
                {
                    _routePath.Add(node.Position);
                    node = node.Parent; //�θ� ã�Ƽ� �ö�
                }
                _routePath.Reverse(); //�������� �����

                _moveIdx = 0; //0������ �̵� ����
            }

            return result;// true�� ���� ã��
        }

        private float CalculateH(Vector3Int pos)
        {
            return (_destPosition - pos).magnitude;
        }

        private void FindOpenList(AstarNode node)
        {
            foreach (var dir in Direction.GetDirectionList(node.Position.z))
            {

                Vector3Int nextPos = node.Position + dir;
                //�˻��� ��ǥ�� NestPos �� ����

                AstarNode temp = _closeList.Find(node => node.Position == nextPos);
                if (temp != null) continue; //�̹� �������� ����


                if (HexGrid.Instance.CanMove(nextPos) == false) continue;

                //������� �´ٸ� �����ִ� ���ϱ� ����ؼ� ���¸���Ʈ�� ����

                if (HexGrid.Instance.CanMove(nextPos))
                {
                    float g = (node.Position - nextPos).magnitude + node.G;
                    AstarNode nextOpenNode = new AstarNode()
                    {
                        Position = nextPos,
                        Parent = node,
                        G = g,
                        F = g + CalculateH(nextPos)
                    };
                    // Astar == F == G + H

                    AstarNode exist = _openList.Contains(nextOpenNode); //���� ���� ������ �ϴ°� �̹� �ֳ�?

                    if (exist != null) // ���� ����
                    {
                        if (nextOpenNode.G < exist.G) //���ƿ°� �ȵ��ƿ°ź��� ����
                        {
                            exist.G = nextOpenNode.G;
                            exist.F = nextOpenNode.F;
                            exist.Parent = nextOpenNode.Parent;

                            //_openList.ReCalculation(exist); //�ٽð���ؼ� �����갡 ���� ������ �����
                        }
                    }
                    else
                    {
                        _openList.Push(nextOpenNode);
                    }
                }
            }

        }

        #endregion

    }
}
