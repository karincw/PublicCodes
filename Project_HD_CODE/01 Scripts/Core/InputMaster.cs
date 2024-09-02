using Karin.AStar;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputMaster : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _selectionMask;

    private AgentHex selectAgent = null;

    List<Vector3Int> neighbours = new List<Vector3Int>();
    List<Vector3Int> agents = new List<Vector3Int>();

    [SerializeField] private bool IsAgentSelected = false;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.SelectEvent += CharactorSelect;
        _inputReader.UnSelectEvent += SelectCancel;
    }

    private void OnDisable()
    {
        _inputReader.SelectEvent -= CharactorSelect;
        _inputReader.UnSelectEvent -= SelectCancel;
    }

    private void CharactorSelect(bool value)
    {
        if (value == false) //���� ������
        {
            GameObject result = null;
            Vector3 mousePos = Input.mousePosition;

            ScreenToRayFind(mousePos, out result);

            if (IsAgentSelected == false)
            {
                //SelectionClick
                SelectClick(result);
            }
            else
            {
                //Interection Click
                InterectionClick(result);
            }
        }
    }

    private void SelectClick(GameObject result)
    {

        if (result == null) return;
        if (result.TryGetComponent<Hex>(out Hex selectedHex))
        {
            foreach (Vector3Int neighbour in neighbours) // ���̶���Ʈ ����
            {
                HexGrid.Instance.GetTileAt(neighbour).DisableMovementHighlight();
            }
            foreach (Vector3Int agent in agents)
            {
                HexGrid.Instance.GetTileAt(agent).DisableAttackHighlight();
            }
            if (selectAgent != null)
            {
                selectAgent.GlowDisable();
                selectAgent = null;
            }

            if (selectedHex is AgentHex) // Agent���
            {

                neighbours.Clear();
                agents.Clear();

                neighbours = HexGrid.Instance.GetNeighboursFor(selectedHex.HexCoords);
                selectAgent = selectedHex as AgentHex;
                //GameManager.Instance.mainScreen.LeftPanelOpen();
                IsAgentSelected = true;

                if (selectAgent.DidItMove == true && selectAgent.DidItAttack == true)
                {
                    IsAgentSelected = false;
                    selectAgent = null;
                    //GameManager.Instance.mainScreen.LeftPanelClose();
                    return;
                }

                #region Attackǥ��
                HashSet<Vector3Int> agentHash = neighbours.ToHashSet();
                if (selectAgent.DidItAttack == false)
                {
                    //����Ʈ ã�ºκ�
                    for (int i = 1; i < selectAgent.atkRange; ++i)
                    {
                        HashSet<Vector3Int> nei = new HashSet<Vector3Int>();
                        foreach (var item in agentHash)
                        {
                            var addItems = HexGrid.Instance.GetNeighboursFor(HexGrid.Instance.GetTileAt(item).HexCoords);

                            nei.UnionWith(addItems);
                        }
                        agentHash.UnionWith(nei);

                    }
                    //����Ʈ Ű�ºκ�
                    HexGrid hexgrid = HexGrid.Instance;
                    foreach (var add in agentHash)
                    {
                        if (hexgrid.GetAgentsCoordinate().Contains(add) == false || add == selectedHex.HexCoords)
                            continue;

                        //���⼭ �������� ���ݰ���ǥ�� �ȳ����� �ٲ����

                            hexgrid.GetTileAt(add).EnableAttackHighlight();
                        agents.Add(add);
                    }
                }
                #endregion

                #region Movementǥ��
                if (selectAgent.DidItMove == false)
                {
                    //����Ʈ ã�ºκ�
                    HashSet<Vector3Int> neighboursHash = new();
                    foreach (var add in neighbours)
                    {
                        if (HexGrid.Instance.GetAgentsCoordinate().Contains(add))
                            continue;
                        neighboursHash.Add(add);
                    }

                    for (int i = 1; i < selectAgent.moveRange; ++i)
                    {
                        HashSet<Vector3Int> nei = new HashSet<Vector3Int>();
                        foreach (var item in neighboursHash)
                        {
                            var addItems = HexGrid.Instance.GetNeighboursFor(HexGrid.Instance.GetTileAt(item).HexCoords);
                            HashSet<Vector3Int> settingedItems = new();

                            foreach (var add in addItems)
                            {
                                if (HexGrid.Instance.GetAgentsCoordinate().Contains(add))
                                    continue;
                                settingedItems.Add(add);
                            }

                            nei.UnionWith(settingedItems);
                        }
                        neighboursHash.UnionWith(nei);
                    }
                    //����Ʈ Ű�ºκ�
                    neighbours.Clear();
                    foreach (var item in neighboursHash)
                    {
                        if (HexGrid.Instance.GetAgentsCoordinate().Contains(item) == false)
                        {
                            neighbours.Add(item);
                            HexGrid.Instance.GetTileAt(item).EnableMovementHighlight();
                        }
                    }
                }
            }
            else
            {
                IsAgentSelected = false;
                selectAgent = null;
                //GameManager.Instance.mainScreen.LeftPanelClose();
            }
            #endregion
        }
    }

    private void InterectionClick(GameObject result)
    {
        if (result == null) return;
        if (result.TryGetComponent<Hex>(out Hex selectedHex))
        {
            if (selectedHex is AgentHex)
            {
                AgentAttack(selectedHex as AgentHex);
            }
            else if (selectedHex.IsGlow == true)
            {
                AgentMovement(selectedHex);
            }
        }
    }

    private void AgentMovement(Hex selectedHex)
    {
        if (HexGrid.Instance.GetAgentsCoordinate().Contains(selectedHex.HexCoords) == true)
        {
            return;
        }

        selectAgent.GetComponent<NavAgent>().StartAstar(selectedHex.HexCoords);

        SelectCancel(false);
    }
    private void AgentAttack(AgentHex selectedAgent)
    {
        if (HexGrid.Instance.GetAgentsCoordinate().Contains(selectedAgent.HexCoords) == false
            || agents.Contains(selectedAgent.HexCoords) == false)
        {
            return;
        }

        if (selectAgent.team.IsMyTeam(selectedAgent.team))
        {
            Debug.Log("�������̶� ���ݸ���");
            return;
        }

        selectAgent.Attack(selectedAgent.HexCoords, selectedAgent);

        SelectCancel(false);
    }

    public void SelectCancel(bool state)
    {
        if (state == false)
        {
            foreach (Vector3Int neighbour in neighbours) // ���̶���Ʈ ����
            {
                HexGrid.Instance.GetTileAt(neighbour).DisableMovementHighlight();
            }
            foreach (Vector3Int agent in agents)
            {
                HexGrid.Instance.GetTileAt(agent).DisableAttackHighlight();
            }
            if (selectAgent != null)
            {
                selectAgent.GlowDisable();
                selectAgent = null;
            }
            IsAgentSelected = false;
        }
    }

    private bool ScreenToRayFind(Vector3 mousePos, out GameObject result)
    {
        Ray ray = _camera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, _selectionMask))
        {
            //Debug.Log($"Hit {hit.collider.gameObject.name}");
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
