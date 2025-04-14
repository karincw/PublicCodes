using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Karin.AStar
{
    public class NavAgent : MonoBehaviour
    {

        public UnityEvent<Vector2> MovementEvent;

        private PriorityQueue<AstarNode> _openList; //내가 갈수있는 것들을 모아놓은것
        private List<AstarNode> _closeList; // 한번이라도 방문한 노드를 모아놓은곳
        private List<Vector3Int> _routePath; //내가 가야할 경로를 타일포지션으로 가지고 있는것

        [SerializeField] private bool _cornerCheck = false; //코너링돌때 체크 여부;

        public event Action NotFoundAction;

        private int _moveIdx = 0;

        private Camera mainCam;
        [SerializeField] private TileBase tile;
        private Vector3Int beforeTilePos;

        private Vector3Int _currentPosition; //현재 타일 포지션
        private Vector3Int _destPosition; //목표 포지션
        public Vector3 _nextPosition;
        public Vector3Int Destination
        {
            get => _destPosition;
            set
            {
                try
                {
                    if (Destination == value) return;
                    SetCurrentPosition();
                    _destPosition = value;
                    if (CalculatePath())
                    {
                        //라우팅 경로 출력 -> 디버그
                        GetNextPath();

                    }
                    else
                    {
                        NotFoundAction?.Invoke();
                    }
                }
                catch { }
            }
        }

        public void GetNextPath()
        {
            if (_moveIdx >= _routePath.Count)
            {
                MovementEvent?.Invoke(Vector2.zero);
                SetCurrentPosition();
                return;
            }
            _currentPosition = _routePath[_moveIdx];
            _nextPosition = MapManager.Instance.GetWorldPos(_currentPosition);

            Vector2 targetDir = _nextPosition - transform.position;
            MovementEvent?.Invoke(targetDir);

            _moveIdx++;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tilePos = MapManager.Instance.GetTilePos(transform.position);
            Destination = tilePos;
            transform.position = MapManager.Instance.GetWorldPos(tilePos);
        }

        public bool NearPosition() //Move가 실행될때마다 호출하면됨 True면 NextPos를 GetNextPath로 받아오기
        {
            if (Vector3.Distance(_nextPosition, transform.position) <= 0.15f)
            { //목표치 까지 도달

                if (beforeTilePos != null)
                {
                    MapManager.Instance.collisionMap.SetTile(beforeTilePos, null);
                }
                var tilePos = MapManager.Instance.GetTilePos(transform.position);
                MapManager.Instance.collisionMap.SetTile(tilePos, tile);
                beforeTilePos = tilePos;

                transform.position = _nextPosition;
                return true;
            }
            else
            {
                return false;
            }

            //목표치에 다다른다면 True를 아닐경우 False를
        }

        private void Awake()
        {
            mainCam = Camera.main;
            _closeList = new List<AstarNode>();
            _routePath = new List<Vector3Int>();
            _openList = new PriorityQueue<AstarNode>();
        }

        private void Start()
        {
            SetCurrentPosition();
            transform.position = MapManager.Instance.GetWorldPos(_currentPosition);
            //계산된 좌표의 타일좌표 중심으로 이동
        }

        private void Update()
        {
            if (NearPosition())
            {
                GetNextPath();
            }
        }

        private void OnDestroy()
        {
            try
            {

                if (beforeTilePos != null)
                {
                    MapManager.Instance.collisionMap.SetTile(beforeTilePos, null);
                }
            }
            catch { }
        }

        private void SetCurrentPosition()
        {
            try

            {

                if (beforeTilePos != null)
                {
                    MapManager.Instance.collisionMap.SetTile(beforeTilePos, null);
                }
                var tilePos = MapManager.Instance.GetTilePos(transform.position);
                MapManager.Instance.collisionMap.SetTile(tilePos, tile);
                beforeTilePos = tilePos;
                _currentPosition = tilePos;
            }
            catch { }
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
                if (cnt > 500)
                {
                    return false;
                    //Debug.Log("못가긴하는데 일단 찾은데 까지는 가보자");
                    //_routePath.Clear();
                    //AstarNode anode = _closeList.Last();
                    //while (anode.Parent != null)
                    //{
                    //    _routePath.Add(anode.Position);
                    //    anode = anode.Parent; //부모 찾아서 올라감
                    //}
                    //_routePath.Reverse(); //역순으로 만들고

                    //_moveIdx = 0; //0번부토 이동 ㄱㄱ
                    //return true;
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
            for (int y = -1; y <= 1; ++y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    if (x == 0 && x == y)
                    {
                        continue;
                        //현재위치니까 무시
                    }

                    Vector3Int nextPos = node.Position + new Vector3Int(x, y, 0);
                    //검사할 좌표를 NestPos 로 변경

                    AstarNode temp = _closeList.Find(node => node.Position == nextPos);
                    if (temp != null) continue; //이미 지나갔던 곳임


                    if (MapManager.Instance.CanMove(nextPos, _currentPosition) == false) continue;
                    if (_cornerCheck == true && MapManager.Instance.CanMove(new Vector3Int(nextPos.x, node.Position.y, 0), _currentPosition) == false
                        || MapManager.Instance.CanMove(new Vector3Int(node.Position.x, nextPos.y, 0), _currentPosition) == false)
                        continue;

                    //여기까지 온다면 갈수있는 노드니까 계산해서 오픈리스트에 넣음

                    if (MapManager.Instance.CanMove(nextPos, _currentPosition))
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
        }

        #endregion

    }
}
