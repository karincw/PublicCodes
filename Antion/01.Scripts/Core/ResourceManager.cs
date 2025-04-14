using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Karin
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(this);
        }

        private void Start()
        {
            UpdateResource();
        }

        [SerializeField] private TextMeshProUGUI _BranchsCountTMP;
        [SerializeField] private TextMeshProUGUI _rocksCountTMP;
        [SerializeField] private TextMeshProUGUI _BonesCountTMP;

        [SerializeField] private int branchCount = 0;
        public int BranchCount
        {
            get => branchCount;
            set
            {
                branchCount = value;
                UpdateResource();
            }
        }
        [SerializeField] private int rockCount = 0;
        public int RockCount
        {
            get => rockCount;
            set
            {
                rockCount = value;
                UpdateResource();
            }
        }
        [SerializeField] private int boneCount = 0;
        public int BoneCount
        {
            get => boneCount;
            set
            {
                boneCount = value;
                UpdateResource();
            }
        }

        public void UpdateResource()
        {
            _BranchsCountTMP.text = BranchCount.ToString("D3");
            _rocksCountTMP.text = RockCount.ToString("D3");
            _BonesCountTMP.text = BoneCount.ToString("D3");
        }
    }
}