using System.Collections;
using UnityEngine;

public class InGameUIManager : MonoSingleton<InGameUIManager>
{

    [SerializeField] private GameObject _UI;
    [SerializeField] private PlayerStateManager _PlayerStateManager;
    private Rigidbody2D rig2d;
    Camera _cam;
    public GameObject currentObj;
    public int ObjX;
    public int ObjY;

    private void Awake()
    {
        _cam = Camera.main;
        rig2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _UI.gameObject.SetActive(false);
        changeSelect();
    }

    [ContextMenu("ChangeGame")]
    public void changeGame()
    {
        _PlayerStateManager.StartPlayMode();
        _PlayerStateManager.clicked = false;
    }
    [ContextMenu("ChangeInstall")]
    public void changeSelect()
    {
        _PlayerStateManager.StartInstallMode();
        _UI.gameObject.SetActive(true);
    }

    [ContextMenu("NextTurn")]
    public void ChangeInstall()
    {
        StartCoroutine(ChangeGameCorutine());
    }
    private IEnumerator ChangeGameCorutine()
    {
        yield return new WaitForSeconds(1f);
        _UI.GetComponent<InstallationUI>().Exit();
        currentObj = PoolManager.Instance.Spawn($"{_PlayerStateManager.clickedItemName}");
        Installing(currentObj);
    }

    private void Update()
    {
        if (currentObj != null)
        {
            ObjX = (int)currentObj.transform.position.x;
            ObjY = (int)currentObj.transform.position.y;

        }
        Installing(currentObj);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            changeGame();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            changeSelect();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeInstall();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {

        }
#endif
    }

    public void Installing(GameObject obj)
    {
        if (obj == null) return;
        obj.transform.position = new Vector2(
            Mathf.Floor(_cam.ScreenToWorldPoint(Input.mousePosition).x),
            Mathf.Floor(_cam.ScreenToWorldPoint(Input.mousePosition).y)
            );
        if (Input.GetKeyDown(KeyCode.R))
        {

            //회전 만들어야함

        }
    }

    public void Uninstalling()
    {
        currentObj = null;
    }

}
