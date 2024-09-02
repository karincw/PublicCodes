using UnityEngine;
using DG.Tweening;

public enum AttackType : ushort
{
    Melee,
    Range
}

public class AgentAttack : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    AgentHex _agentHex;

    [SerializeField] AttackType attackType;
    [SerializeField] GameObject RangeBullet;

    private void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        _agentHex = GetComponentInParent<AgentHex>();
        _spriteRenderer = visualTrm.GetComponent<SpriteRenderer>();
    }

    public void AttackFeedback(Vector3 AttackPos, AgentHex hitAgent)
    {

        var originPos = transform.position;
        if (attackType == AttackType.Melee)
        {
            Sequence seq = DOTween.Sequence().Append(transform.DOMove(AttackPos, 0.3f))
                .AppendCallback(() => { hitAgent.Hit(_agentHex.atk); })
                .Append(transform.DOMove(originPos, 0.3f));
        }
        else if (attackType == AttackType.Range)
        {
            var rebound = (transform.position - AttackPos).normalized;
            var reboundPos = transform.position + rebound * 0.5f;
            int dis = Mathf.RoundToInt(Vector3.Distance(originPos, AttackPos));
            Sequence seq = DOTween.Sequence()
                .Append(transform.DOMove(reboundPos, 0.1f))
                .AppendCallback(() =>
                {
                    var bullet = Instantiate(RangeBullet, new Vector3(reboundPos.x, 0.8f, reboundPos.z), Quaternion.Euler(-reboundPos));
                    Sequence seq2 = DOTween.Sequence()
                    .Append(bullet.transform.DOMove(new Vector3(AttackPos.x, 0.8f, AttackPos.z), dis * 0.1f))
                    .AppendCallback(() =>
                    {
                        hitAgent.Hit(_agentHex.atk);
                        Destroy(bullet);
                    });
                })
                .Append(transform.DOMove(originPos, 0.2f));

        }
        _agentHex.DidItAttack = true;
        _agentHex.DidItMove = true;
    }

    public void HitFeedBack()
    {
        Color originColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;
        _spriteRenderer.DOColor(originColor, 0.2f);
    }

}
