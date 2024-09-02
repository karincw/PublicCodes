using System;
using UnityEngine;

namespace Karin.AStar
{
    public class AstarNode : IComparable<AstarNode>
    {
        public Vector3Int Position; // 타일맵 포지션
        public AstarNode Parent;
        public float G;
        public float F;
        //H 는 직접 계산 //F == 직선거리
        //F = G + H

        public int CompareTo(AstarNode other)
        {
            if (other.F == this.F) return 0;
            return other.F < this.F ? -1 : 1;
        }

        public bool Equals(AstarNode other)
        {
            if (other == null || this.GetType() != other.GetType())
            {
                return false;
            }

            return Position == other.Position; // 포지션이 같지않다면 False 같으면 True
        }

        public override bool Equals(object other) => this.Equals((AstarNode)other);

        public override int GetHashCode()
        {
            return Position.GetHashCode();
            //포지션의 Hash코드 뽑기
        }
        #region 연산자 재정의

        //연산자 재정의 반드시 Static으로
        public static bool operator ==(AstarNode lhs, AstarNode rhs)
        {
            if (lhs is null)
            {
                return rhs is null ? true : false; // 둘다 Null이면 True아니면False
            }//Null 체크

            return lhs.Equals(rhs);
        }

        public static bool operator !=(AstarNode lhs, AstarNode rhs)
        {
            return !(lhs == rhs); //이미 [ == ] 을 정의 했으니 한거에 Not을 붙이면됨
        }
        #endregion

    }
}
