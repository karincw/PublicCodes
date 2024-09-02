using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BadEnding : MonoBehaviour
{
    [SerializeField] private Transform view;
    [SerializeField] private Image sign;
    [SerializeField] private CanvasGroup panel;
    private void Start()
    {
        view.position = new Vector3(view.transform.position.x, 540, -10);
        sign.fillAmount = 1;
        panel.interactable = false;
        StartCoroutine("StroyCoroutine");
    }

    IEnumerator StroyCoroutine()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        view.DOMove(new Vector3(view.transform.position.x, -540, -10), 4f);
        yield return new WaitForSeconds(4f);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        sign.DOFillAmount(0, 1.5f);
        yield return new WaitForSeconds(1.5f);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        panel.DOFade(1, 2f)
            .OnComplete(() =>
            {
                panel.interactable = true;
            });
    }

}
