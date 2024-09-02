using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Charactor : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string _attackEnemyName;
    [SerializeField] private string _attackEnemyTag;
    [SerializeField] private Transform _enemyParent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SoundPlay(int tone)
    {
        if (_animator.runtimeAnimatorController != null)
            _animator.SetTrigger("Attack");

        Attack();
    }

    public void Attack()
    {
        try
        {
            List<GameObject> atkTargets = GameObject.FindGameObjectsWithTag(_attackEnemyTag).ToList();
            atkTargets.OrderBy(t => t.transform.position.x).ToList();
            if (atkTargets.Count <= 0)
            {
                GameManager.Instance.CastleHealth.DecreaseHealth(5);
                return;
            }
            GameObject atkTarget = atkTargets.First();

            atkTarget.GetComponent<Enemy>().Release();

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
            return;
        }


    }
}
