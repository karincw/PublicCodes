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

        private PriorityQueue<AstarNode> _openList; //내가 갈수있는 것들을 모아놓은것
        private List<AstarNode> _closeList; // 한번이라도 방문한 노드를 모아놓은곳
        private List<Vector3Int> _routePath; //내가 가야할 경로를 타일포지션으로 가지고 있는것

        private int _moveIdx = 0;

        private Vector3Int _currentPosition; //현재 타일 포지션
        private Vector3Int _destPosition; //목표 포지션
        public Vector3 _nextPosition;
        public Vector3Int Destination
        {
            get => _destPosition;
            set
            {
                if (Destination == value) return; //목적지가 안변함
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
            //계산된 좌표의 타일좌표 중심으로 이동
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

        #region Astar의 사실상 전부

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
            while (_openList.Count > 0) //선택할게 있음
            {
                AstarNode node = _openList.Pop();
                FindOpenList(node);
                _closeList.Add(node); //방문한노드니까 closeList로 들어감

                if (_destPosition == node.Position)//방문한 노드가 목적지였다면 도착한거니까 멈춰라
                {
                    result = true;
                    break;
                }


                cnt++;
                if (cnt > 10000)
                {
                    Debug.Log("에러");
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
                    node = node.Parent; //부모 찾아서 올라감
                }
                _routePath.Reverse(); //역순으로 만들고

                _moveIdx = 0; //0번부토 이동 ㄱㄱ
            }

            return result;// true면 길을 찾음
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
                //검사할 좌표를 NestPos 로 변경

                AstarNode temp = _closeList.Find(node => node.Position == nextPos);
                if (temp != null) continue; //이미 지나갔던 곳임


                if (HexGrid.Instance.CanMove(nextPos) == false) continue;

                //여기까지 온다면 갈수있는 노드니까 계산해서 오픈리스트에 넣음

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

                    AstarNode exist = _openList.Contains(nextOpenNode); //지금 내가 가려고 하는게 이미 있냐?

                    if (exist != null) // ㅇㅇ 있음
                    {
                        if (nextOpenNode.G < exist.G) //돌아온게 안돌아온거보다 빠름
                        {
                            exist.G = nextOpenNode.G;
                            exist.F = nextOpenNode.F;
                            exist.Parent = nextOpenNode.Parent;

                            //_openList.ReCalculation(exist); //다시계산해서 작은얘가 위로 오도록 해줘라
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
