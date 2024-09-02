using System;
using UnityEngine;

namespace Karin.AStar
{
    public class AstarNode : IComparable<AstarNode>
    {
        public Vector3Int Position; // Ÿ�ϸ� ������
        public AstarNode Parent;
        public float G;
        public float F;
        //H �� ���� ��� //F == �����Ÿ�
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

            return Position == other.Position; // �������� �����ʴٸ� False ������ True
        }

        public override bool Equals(object other) => this.Equals((AstarNode)other);

        public override int GetHashCode()
        {
            return Position.GetHashCode();
            //�������� Hash�ڵ� �̱�
        }
        #region ������ ������

        //������ ������ �ݵ�� Static����
        public static bool operator ==(AstarNode lhs, AstarNode rhs)
        {
            if (lhs is null)
            {
                return rhs is null ? true : false; // �Ѵ� Null�̸� True�ƴϸ�False
            }//Null üũ

            return lhs.Equals(rhs);
        }

        public static bool operator !=(AstarNode lhs, AstarNode rhs)
        {
            return !(lhs == rhs); //�̹� [ == ] �� ���� ������ �Ѱſ� Not�� ���̸��
        }
        #endregion

    }
}
