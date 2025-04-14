using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class Selecter : MonoBehaviour
    {
        //레이를 쏴서 움직일 캐릭터를 선택하고 선택이 됬으면 
        //클릭한 좌표로 캐릭터 움직이게 함

        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private LayerMask selectLayer;
        [SerializeField] private BottomBar _bottomBer;

        private ControlableCharacter selectedObject = null;

        private void OnEnable()
        {
            _inputReader.SelectEvent += AgentSelect;
        }
        private void OnDisable()
        {
            _inputReader.SelectEvent -= AgentSelect;
        }

        private void AgentSelect(Vector2 mousePos)
        {
            Collider2D hitCol = Physics2D.OverlapCircle(mousePos, 0.3f, selectLayer);
            if (hitCol != null)
            {
                GameObject selectObject = hitCol.gameObject;

                if (selectObject.TryGetComponent<IStructure>(out var structure))
                {
                    _bottomBer.Setting(structure.StructureData.buildingSprite, structure.StructureData.name, 0, 0, structure.CurrentHp, structure.StructureData.maxHealth, 0); ;
                }

                if (selectObject.TryGetComponent<ControlableCharacter>(out var controlableCharacter))
                {
                    _bottomBer.Setting(controlableCharacter._characterData.Image, controlableCharacter._characterData.name, controlableCharacter._characterData.atk, (controlableCharacter._characterData.atkspeed),
                       controlableCharacter.CurrentHP, controlableCharacter._characterData.maxHealth, controlableCharacter._characterData.range);


                    if (selectedObject != null)
                    {
                        selectedObject.DeSelect();
                    }
                    controlableCharacter.Select();
                    selectedObject = controlableCharacter;
                }
            }

        }



    }
}
