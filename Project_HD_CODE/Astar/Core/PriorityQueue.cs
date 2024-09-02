using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public class PriorityQueue<T> where T : IComparable<T>
{
    public List<T> _heap = new List<T>();
    public int Count
    {
        get { return _heap.Count; }
    }

    public T Contains(T t)
    {
        int idx = _heap.IndexOf(t);
        if (idx < 0) return default(T);
        return _heap[idx];
    }

    public void Push(T data)
    {
        //���� �� ���� ���ο� �����͸� �����Ѵ�.
        _heap.Add(data);
        int now = _heap.Count - 1;
        while (now > 0)
        {
            //�ڱ⺸�� �켱������ �и��� ���� �ִ��� ã�´�.
            int next = (now - 1) / 2;
            if (_heap[now].CompareTo(_heap[next]) < 0)
            {
                break;
            }
            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            now = next;
        }
    } //���� ������ �켱������ ���� ���� �༮�� �����ִ�.

    public T Pop()
    {
        T ret = _heap[0];

        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex); //������ ���Ҹ� �� ���� ������� �������� ����
        lastIndex--;

        //���� �������鼭 �ڱ� �ڸ� ã�ư��� (push�� �ݴ�)
        int now = 0;
        while (true)
        {
            int left = 2 * now + 1;
            int right = 2 * now + 2;

            int next = now;
            //���ʰ��� ���� ������ ũ�ٸ� �������� �������� ���ʰŸ� ���ڸ��� �ø�
            if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
            {
                next = left;
            }

            //�������� �׷��ٸ� (���� ������ ������ Ÿ���� �Ǿ�� �̰��� ���������� Ÿ���� ����ȴ�.
            if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
            {
                next = right;
            }

            //���� �������̳� ������ ������ �� �۴ٴ� ���� �Ǵϱ� �̷��� �� ����
            if (next == now)
                break;

            //�׷��� �ʴٸ� ��ü�ؼ� ����������
            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            now = next;
        }
        return ret;
    }

    public T Peek()
    {
        //TŸ���� �⺻���� �����ϰ� �׷��� �ʴٸ� 0��°�� �����ϰ�
        return _heap.Count == 0 ? default(T) : _heap[0];
    }

    public void Clear()
    {
        _heap.Clear();
    }
}

