using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class OseroManager : MonoBehaviour
{
    static  OseroManager _instance;
    public OseroManager Instance => _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [SerializeField]
    GameObject _oseroGridPrefabSF;

    [SerializeField]
    GameObject _oseroPiecesPrefabSF;

    List<OseroGridScript> _oseroGridScripts = new List<OseroGridScript>();
    
    Tern ternEnum = Tern.Empty;

    public Tern TernEnum => ternEnum;

    [SerializeField]
    int _maxSquareSF = 1;

    [SerializeField]
    float _oserorangeSF = 1;


    int   _tern      = 0;
    float _time      = 0;
    float _blackTime = 0;
    float _whiteTime = 0;

    void Start()
    {
        SetGrid();
    }


    void SetKoma(int x, int y, GridProperties gridProperties)
    {
        OseroGridScript oserogurid                        = _oseroGridScripts.FirstOrDefault(n => n.X == x && n.Y == y);
        if (oserogurid != null) oserogurid.PGridProperties = gridProperties;
        Debug.LogError("a is null");
    }
    void SetGrid()
    {
        for (int i = 0; i < _maxSquareSF; i++)
        {
            for (int k = 0; k < _maxSquareSF; k++)
            {
                var oseroGridPrefub = Instantiate(_oseroGridPrefabSF,
                    new Vector3(i * _oserorangeSF, 0, k * _oserorangeSF),
                    Quaternion.identity);
                oseroGridPrefub.transform.SetParent(transform);
                oseroGridPrefub.name = "OseroGrid" + i + "_" + k;
                OseroGridScript newGridScript = oseroGridPrefub.GetComponent<OseroGridScript>();
                newGridScript.X = i;
                newGridScript.Y = k;
                _oseroGridScripts.Add(newGridScript);
            }
        }
    }
}
public enum Tern
{
    Empty,
    Black,
    White,
    Result
}
