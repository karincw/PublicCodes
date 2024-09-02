using DG.Tweening;
using HS;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CW
{

    public class HarvestManager : MonoSingleton<HarvestManager>
    {
        [SerializeField] private GameObject _moveObject;
        [SerializeField] private Transform _moverTrm;
        [SerializeField] private float[] times;
        [SerializeField] private float cropSpawnDealy;
        [SerializeField] private Transform _cropParents;

        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }


        public void Harvest(Vector3 pos, Crop crop, int count)
        {
            StartCoroutine(HarvestCoroutine(pos, crop, count));
        }

        public IEnumerator HarvestCoroutine(Vector3 pos, Crop crop, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var move = Instantiate(_moveObject, pos, Quaternion.identity, _moverTrm);
                var sr = move.GetComponent<SpriteRenderer>();
                sr.sprite = crop.sprite;


                Vector2 originPos = move.transform.position;
                Vector2 targetDir = originPos + new Vector2(Random.Range(-0.8f, 0.8f), 0);

                float fadeSpeed = Random.Range(0.05f, .2f);

                Sequence seq = DOTween.Sequence()
                    .Append(move.transform.DOMoveX(originPos.x + targetDir.x * .3f, times[0]))
                    .Join(move.transform.DOMoveY(originPos.y + 1.3f, times[0]))
                    .Append(move.transform.DOMoveY(originPos.y, times[0]))

                    .Append(move.transform.DOMoveX(originPos.x + targetDir.x * .5f, times[1]))
                    .Join(move.transform.DOMoveY(originPos.y + .3f, times[1]))
                    .Append(move.transform.DOMoveY(originPos.y, times[1]))
                    .Join(sr.DOFade(0, fadeSpeed))
                    .OnComplete(() =>
                    {
                        _cropParents.Find(crop.currentCard.curName).GetComponent<CropInven>().AddCount(1);
                        _audio.Play();
                        Destroy(move.gameObject);
                    });

                yield return new WaitForSeconds(cropSpawnDealy);
            }
        }


    }

}