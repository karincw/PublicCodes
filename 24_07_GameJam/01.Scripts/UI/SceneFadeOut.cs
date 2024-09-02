using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeOut : MonoBehaviour
{
    public RectTransform _boardRect;

    public void BoardDown()
    {
        Sequence seq = DOTween.Sequence()
            .Append(_boardRect.DOAnchorPos(new Vector2(0, 0), 1f))
            .Join(_boardRect.GetComponent<Image>().DOFade(1, 1f));

        Invoke("SceneChange", 1.5f);
    }

    private void SceneChange()
    {
        SceneManager.LoadScene(1);
    }

}
