using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace CW
{

    public class Crow : MonoBehaviour
    {
        private Vector2 currentPosition;
        [SerializeField] private GameObject destroyCropEffect;
        private GameObject _trail;
        public bool canMove = true;
        private bool isThrow = false;

        Coroutine coroutine = null;

        private void Awake()
        {
            _trail = transform.Find("Trail").gameObject;
            canMove = true;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) < .5f)
            {
                CrowManager.Instance.PlaySound();
                isThrow = true;
                Catch();
            }

            if(isThrow)
            {
                if(coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
        }

        public void Catch()
        {
            DOTween.Kill(this);
            StopAllCoroutines();
            MoveExit();
        }

        public void MoveTile()
        {
            isThrow = false;
            canMove = false;
            List<string> strList = new List<string>();

            Vector2 startPos = CrowManager.Instance.crowStartPos;
            startPos.y += Random.Range(-1, 3);
            transform.position = startPos;
            _trail.SetActive(true);

            Vector3Int pos = CropManager.Instance.GetRandomCropPos();
            bool isNull = false;
            Crop crop = CropManager.Instance.GetPosToCrop(pos, ref isNull);
            if (!isNull)
            {
                if (crop.currentCard.cardType == CardType.Building)
                    CrowManager.Instance.SummonCrow();
                Catch();
            }
            currentPosition = (Vector3)pos;

            transform.DOMove(pos, 2.5f).OnComplete(() => { coroutine = StartCoroutine("DestroyCropAndExitCoroutine"); });
        }

        public IEnumerator DestroyCropAndExitCoroutine()
        {
            Vector3Int currentIntPos = new Vector3Int((int)currentPosition.x, (int)currentPosition.y, 0);

            yield return new WaitForSeconds(1);

            CropManager.Instance.SetGroundTile(currentIntPos);
            Instantiate(destroyCropEffect, transform.position + new Vector3(0, -0.3f), Quaternion.identity);

            yield return new WaitForSeconds(1);

            MoveExit();

        }

        public void MoveExit()
        {
            Vector2 endPos = CrowManager.Instance.crowEndPos;
            endPos.y += Random.Range(-3, 3);

            transform.DOMove(endPos, 2f)
            .OnComplete(() =>
                {
                    _trail.SetActive(false);
                    canMove = true;
                });

        }

    }

}