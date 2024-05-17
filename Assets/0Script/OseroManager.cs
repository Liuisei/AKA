using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OseroManager : MonoBehaviour
{
    static        OseroManager _instance;
    public static OseroManager Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance == null) { _instance = this; }
        else { Destroy(gameObject); }
    }
    [SerializeField] GameObject _selectPrefab;

    [Header("Materials")] public Material   _canPutMaterial;
    [SerializeField]      public Material   _normalMaterial;
    [SerializeField]             GameObject _oseroGridPrefabSf;
    List<OseroGridScript>                   _oseroGridScripts = new List<OseroGridScript>();

    Tern _ternEnum = Tern.Empty;

    public Tern TernEnum => _ternEnum;

    [SerializeField] int _maxSquareSf = 1;

    [SerializeField] float _oserorangeSf = 1;

    public int selectx = 0;

    public int selecty = 0;

    public int Selectx
    {
        get { return selectx; }
        set
        {
            selectx = value;
            SelectPrefubPosition();
        }
    }

    public int Selecty
    {
        get { return selecty; }
        set
        {
            selecty = value;
            SelectPrefubPosition();
        }
    }

    void SelectPrefubPosition() { _selectPrefab.transform.position = new Vector3(Selectx * _oserorangeSf, 0.2f, Selecty * _oserorangeSf); }

    int   _tern      = 0;
    float _time      = 0;
    float _blackTime = 0;
    float _whiteTime = 0;

    void Start()
    {
        SetGrid();
        SetUPKoma();
        BlactTurn();
        SelectPrefubPosition();
    }

    void SetKoma(int x, int y, GridMode gridMode , bool forced)
    {
        OseroGridScript targetOseroGridScript  = _oseroGridScripts.FirstOrDefault(n => n.X == x && n.Y == y);
        if (targetOseroGridScript == null)
        {
            Debug.LogError("targetOseroGridScript is null");
            return;
        }

        if (forced) { targetOseroGridScript.PGridMode = gridMode; }
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
        SetKoma(3, 3, GridMode.White, true);
        SetKoma(3, 4, GridMode.Black, true);
        SetKoma(4, 3, GridMode.Black, true);
        SetKoma(4, 4, GridMode.White, true);
    }


    void BlactTurn()
    {
        _ternEnum = Tern.Black;
        var block = _oseroGridScripts.Where(e => e.PGridMode == GridMode.Black);
        foreach (var b in block)
        {
            TergetVBlock(1,  0,  b.X, b.Y,  false );
            TergetVBlock(-1, 0,  b.X, b.Y , false );
            TergetVBlock(0,  1,  b.X, b.Y , false );
            TergetVBlock(0,  -1, b.X, b.Y , false );
            TergetVBlock(1,  1,  b.X, b.Y , false );
            TergetVBlock(1,  -1, b.X, b.Y , false );
            TergetVBlock(-1, 1,  b.X, b.Y , false );
            TergetVBlock(-1, -1, b.X, b.Y , false );
        }
    }

    void BlackAtk( )
    {
        TergetVBlockATK(1,  0,  selectx, selecty,  false );
        _atkTarget.Clear();
        TergetVBlockATK(-1, 0,  selectx, selecty , false );
        _atkTarget.Clear();
        TergetVBlockATK(0,  1,  selectx, selecty , false );
        _atkTarget.Clear();
        TergetVBlockATK(0,  -1, selectx, selecty , false );
        _atkTarget.Clear();
        TergetVBlockATK(1,  1,  selectx, selecty , false );
        _atkTarget.Clear();
        TergetVBlockATK(1,  -1, selectx, selecty , false );
        _atkTarget.Clear();
        TergetVBlockATK(-1, 1,  selectx, selecty , false );
        _atkTarget.Clear();
        TergetVBlockATK(-1, -1, selectx, selecty , false );
        _atkTarget.Clear();
    }


    void TergetVBlock(int x, int y,  int ox, int oy , bool haveenemy)
    {
        int             nowx                  = ox + x;
        int             nowy                  = oy + y;
        OseroGridScript targetOseroGridScript = _oseroGridScripts.FirstOrDefault(n => n.X == nowx  && n.Y == nowy);
        if (targetOseroGridScript == null) { return; }

        if (targetOseroGridScript.PGridMode == GridMode.Black) { return; }

        if (targetOseroGridScript.PGridMode == GridMode.White)
        {
            TergetVBlock( x,  y,  nowx,  nowx , true);
            return;
        }

        if (haveenemy &&  targetOseroGridScript.PGridMode == GridMode.Empty) { targetOseroGridScript.PGridMode = GridMode.CanPut; }
    }
    void TergetVWhite(int x, int y,  int ox, int oy)
    {
        int             nowx                  = ox + x;
        int             nowy                  = oy + y;
        OseroGridScript targetOseroGridScript = _oseroGridScripts.FirstOrDefault(n => n.X == nowx + x && n.Y == nowy);
        if (targetOseroGridScript == null) { return; }

        if (targetOseroGridScript.PGridMode == GridMode.White) { return; }

        if (targetOseroGridScript.PGridMode == GridMode.Black)
        {
            TergetVBlock( x,  y,  nowx,  nowx , true);
            return;
        }

        if (targetOseroGridScript.PGridMode == GridMode.Empty) { targetOseroGridScript.PGridMode = GridMode.CanPut; }
    }

    List<OseroGridScript> _atkTarget = new List<OseroGridScript>();
    void TergetVBlockATK(int x, int y,  int ox, int oy , bool haveenemy)
    {
        Debug.Log("a");
        int             nowx                  = ox + x;
        int             nowy                  = oy + y;
        OseroGridScript targetOseroGridScript = _oseroGridScripts.FirstOrDefault(n => n.X == nowx  && n.Y == nowy);
        if (targetOseroGridScript == null) { return; }

        if (haveenemy &&  targetOseroGridScript.PGridMode == GridMode.Empty)
        {
            _atkTarget.Clear();
            return;
        }

        if (targetOseroGridScript.PGridMode == GridMode.White)
        {
            _atkTarget.Add(targetOseroGridScript);
            TergetVBlockATK( x,  y,  nowx,  nowx , true);
            return;
        }

        if (targetOseroGridScript.PGridMode == GridMode.Black && haveenemy)
        {
            
            foreach (var t in _atkTarget) { t.PGridMode = GridMode.Black; }
            Debug.Log("b");
        }
    }

    public void Update()
    {
        // WASD  KEY DOWN
        if (Input.GetKeyDown(KeyCode.W) && Selecty < _maxSquareSf - 1) { Selecty += 1; }

        if (Input.GetKeyDown(KeyCode.A) && Selectx > 0) { Selectx -= 1; }

        if (Input.GetKeyDown(KeyCode.S) && Selecty > 0) { Selecty -= 1; }

        if (Input.GetKeyDown(KeyCode.D) && Selectx < _maxSquareSf - 1) { Selectx += 1; }

        //key down space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var targetOseroGridScript = _oseroGridScripts.FirstOrDefault(n => n.X == Selectx && n.Y == Selecty);
            if (targetOseroGridScript == null) { return; }

            if (targetOseroGridScript.PGridMode == GridMode.CanPut)
            {
                if (_ternEnum == Tern.Black)
                {
                    targetOseroGridScript.PGridMode = GridMode.Black;
                    BlackAtk();
                }
            }
        }
    }
}

public enum Tern { Empty, Black, White, Result }