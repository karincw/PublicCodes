using CW;
using System.Collections.Generic;
using UnityEngine;

public class CrowManager : MonoSingleton<CrowManager>
{
    [SerializeField] private Transform _crowStartTrm;
    [SerializeField] private Transform _crowEndTrm;
    public Vector2 crowStartPos => _crowStartTrm.position;
    public Vector2 crowEndPos => _crowEndTrm.position;

    [SerializeField] private List<Crow> _crows = new List<Crow>();

    [SerializeField] private float _crowSpawnCooltimebase = 5f;
    [SerializeField] private float _crowSpawnCooltimeoffset = 2f;
    private float _crowSpawnCooldown;
    private float Cooltime => _crowSpawnCooltimebase + Random.Range(-_crowSpawnCooltimeoffset, _crowSpawnCooltimeoffset);

    private bool canSpawn = true;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _crowSpawnCooldown = Cooltime;
    }
    private void Update()
    {

        if (_crowSpawnCooldown < 0)
        {
            SummonCrow();

            _crowSpawnCooldown = Cooltime;
        }

        _crowSpawnCooldown -= Time.deltaTime;

    }

    public void PlaySound()
    {
        _audio.Play();
    }

    public void SummonCrow()
    {
        if (!canSpawn) return;

        Crow currentCrow = null;
        foreach (var crow in _crows)
        {
            if (crow.canMove == false) continue;

            currentCrow = crow;
        }
        if (currentCrow == null)
            Debug.LogError("CurrentCrow Is NULL _crows haven't activeCrow");

        currentCrow.MoveTile();
    }
    
    public void StopSummonCrow()
    {
        canSpawn = false;
    }

    public void StartSummonCrow()
    {
        canSpawn = true;
    }


}
