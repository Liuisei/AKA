using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OseroManager : MonoBehaviour
{
    static OseroManager _instance;
    public OseroManager Instance => _instance;
    void Awake()
    {
        if (_instance == null) { _instance = this; }
        else { Destroy(gameObject); }
    }

    [ SerializeField] GameObject _oseroGridPrefabSf;

    List<OseroGridScript> _oseroGridScripts = new List<OseroGridScript>();

    Tern _ternEnum = Tern.Empty;

    public Tern TernEnum => _ternEnum;

    [SerializeField] int _maxSquareSf = 1;

    [SerializeField] float _oserorangeSf = 1;


    int   _tern      = 0;
    float _time      = 0;
    float _blackTime = 0;
    float _whiteTime = 0;

    void Start()
    {
        SetGrid();
        SetUPKoma();
    }


    void SetKoma(int x, int y, GridMode gridMode , bool forced)
    {
        OseroGridScript targetOseroGridScript                         = _oseroGridScripts.FirstOrDefault(n => n.X == x && n.Y == y);
        if (targetOseroGridScript == null)
        {
            Debug.LogError("targetOseroGridScript is null");
            return;
        }
        if (forced)
        {
            targetOseroGridScript.PGridMode = gridMode;
        }
        else
        {
            if (targetOseroGridScript.PGridMode == GridMode.CanPut) { targetOseroGridScript.PGridMode = gridMode; }
        }
    }
    void SetGrid()
    {
        for (int i = 0; i < _maxSquareSf; i++)
        {
            for (int k = 0; k < _maxSquareSf; k++)
            {
                var oseroGridPrefub = Instantiate(_oseroGridPrefabSf, new Vector3(i * _oserorangeSf, 0, k * _oserorangeSf), Quaternion.identity);
                oseroGridPrefub.transform.SetParent(transform);
                oseroGridPrefub.name = "OseroGrid" + i + "_" + k;
                OseroGridScript newGridScript = oseroGridPrefub.GetComponent<OseroGridScript>();
                newGridScript.X = i;
                newGridScript.Y = k;
                _oseroGridScripts.Add(newGridScript);
            }
        }
    }

    void SetUPKoma()
    {
        SetKoma(3, 3, GridMode.White,true);
        SetKoma(3, 4, GridMode.Black,true);
        SetKoma(4, 3, GridMode.Black,true);
        SetKoma(4, 4, GridMode.White,true);
    }
}

public enum Tern { Empty, Black, White, Result }