using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    OseroGridScript[]                       _oseroGridScripts = new OseroGridScript[64];

    Tern _ternEnum = Tern.Empty;

    public Tern TernEnum => _ternEnum;

    [SerializeField] int   _maxSquareSf  = 1;
    [SerializeField] float _oserorangeSf = 1;

    int        _selectPoint = 0;

    public int SelectPoint
    {
        get { return _selectPoint; }
        set
        {
            _selectPoint = value;
            SelectPrefubPosition();
        }
    }

    void SelectPrefubPosition() { _selectPrefab.transform.position = new Vector3(SelectPoint % 8 * _oserorangeSf, 0.2f, SelectPoint / 8 * _oserorangeSf); }

    int   _tern      = 0;
    float _time      = 0;
    float _blackTime = 0;
    float _whiteTime = 0;

    void Start()
    {
        SetUpGrid();
        BlackTurn();
    }
    void SetUpGrid()
    {
        for (int i = 0; i < 64; i++)
        {
            GameObject go = Instantiate(_oseroGridPrefabSf, new Vector3(i % 8 * _oserorangeSf, 0, i / 8 * _oserorangeSf), Quaternion.identity);
            _oseroGridScripts[i]         = go.GetComponent<OseroGridScript>();
            _oseroGridScripts[i]._number = i;
        }

        _oseroGridScripts[27].PGridMode = GridMode.Black;
        _oseroGridScripts[28].PGridMode = GridMode.White;
        _oseroGridScripts[35].PGridMode = GridMode.White;
        _oseroGridScripts[36].PGridMode = GridMode.Black;
    }
    List<OseroGridScript> _changeList = new List<OseroGridScript>();

    void BlackTurn()
    {
        ResetCanput();
        _ternEnum = Tern.Black;

        var blaclist = _oseroGridScripts.Where( x => x.PGridMode == GridMode.Black);
        foreach (var b in blaclist)
        {
            BlackAsterisk(1,  0 , b._number % 8, b._number / 8, false);
            BlackAsterisk(1,  1 , b._number % 8, b._number / 8, false);
            BlackAsterisk( 0, 1 , b._number % 8, b._number / 8, false);
            BlackAsterisk(-1, 1 , b._number % 8, b._number / 8, false);
            BlackAsterisk(-1, 0 , b._number % 8, b._number / 8, false);
            BlackAsterisk(-1, -1, b._number % 8, b._number / 8, false);
            BlackAsterisk( 0, -1, b._number % 8, b._number / 8, false);
            BlackAsterisk( 1, -1, b._number % 8, b._number / 8, false);
        }
    }
    void BlackAsterisk(int x , int y, int ox, int oy , bool haveEnemy)
    {
        int newx        = x    + ox;
        int newy        = y    + oy;
        int newposition = newx + newy * 8;

        if (newx < 0 || newx > 7 || newy < 0 || newy > 7) { return; }

        if (_oseroGridScripts[newposition].PGridMode == GridMode.Black) { return; }

        if (_oseroGridScripts[newposition].PGridMode == GridMode.White)
        {
            BlackAsterisk( x, y , newx, newy , true);
            return;
        }

        if ( _oseroGridScripts[newposition].PGridMode == GridMode.Empty && haveEnemy) { _oseroGridScripts[newposition].PGridMode = GridMode.CanPut; }
    }
    void BlackAttack()
    {
        BlackAttackAsterisk(1,  0 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk(1,  1 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk( 0, 1 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk(-1, 1 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk(-1, 0 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk(-1, -1, _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk( 0, -1, _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        BlackAttackAsterisk( 1, -1, _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
    }
    void BlackAttackAsterisk(int x , int y, int ox, int oy , bool haveEnemy)
    {
        int newx        = x    + ox;
        int newy        = y    + oy;
        int newposition = newx + newy * 8;

        if (newx < 0 || newx > 7 || newy < 0 || newy > 7) { return; }

        if ( _oseroGridScripts[newposition].PGridMode == GridMode.Empty ) { return; }

        if (haveEnemy && _oseroGridScripts[newposition].PGridMode == GridMode.Black)
        {
            foreach (var c in _changeList) { c.PGridMode = GridMode.Black; }
            return;
        }

        if (_oseroGridScripts[newposition].PGridMode == GridMode.White)
        {
            _changeList.Add(_oseroGridScripts[newposition]);
            BlackAttackAsterisk( x, y , newx, newy , true);
        }
    }
    
    void WhiteTurn()
    {
        ResetCanput();

        _ternEnum = Tern.White;

        var whiteList = _oseroGridScripts.Where( x => x.PGridMode == GridMode.White);
        foreach (var b in whiteList)
        {
            WhiteAsterisk(1,  0 , b._number % 8, b._number / 8, false);
            WhiteAsterisk(1,  1 , b._number % 8, b._number / 8, false);
            WhiteAsterisk( 0, 1 , b._number % 8, b._number / 8, false);
            WhiteAsterisk(-1, 1 , b._number % 8, b._number / 8, false);
            WhiteAsterisk(-1, 0 , b._number % 8, b._number / 8, false);
            WhiteAsterisk(-1, -1, b._number % 8, b._number / 8, false);
            WhiteAsterisk( 0, -1, b._number % 8, b._number / 8, false);
            WhiteAsterisk( 1, -1, b._number % 8, b._number / 8, false);
        }
    }
    void WhiteAsterisk(int x , int y, int ox, int oy , bool haveEnemy)
    {
        int newx        = x    + ox;
        int newy        = y    + oy;
        int newposition = newx + newy * 8;

        if (newx < 0 || newx > 7 || newy < 0 || newy > 7) { return; }

        if (_oseroGridScripts[newposition].PGridMode == GridMode.White) { return; }

        if (_oseroGridScripts[newposition].PGridMode == GridMode.Black)
        {
            WhiteAsterisk( x, y , newx, newy , true);
            return;
        }

        if ( _oseroGridScripts[newposition].PGridMode == GridMode.Empty && haveEnemy) { _oseroGridScripts[newposition].PGridMode = GridMode.CanPut; }
    }
    void WhiteAttack()
    {
        WhiteAttackAsterisk(1,  0 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk(1,  1 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk( 0, 1 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk(-1, 1 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk(-1, 0 , _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk(-1, -1, _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk( 0, -1, _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
        WhiteAttackAsterisk( 1, -1, _selectPoint % 8, _selectPoint / 8, false);
        _changeList.Clear();
    }
    void WhiteAttackAsterisk(int x , int y, int ox, int oy , bool haveEnemy)
    {
        int newx        = x    + ox;
        int newy        = y    + oy;
        int newposition = newx + newy * 8;

        if (newx < 0 || newx > 7 || newy < 0 || newy > 7) { return; }

        if ( _oseroGridScripts[newposition].PGridMode == GridMode.Empty ) { return; }

        if (haveEnemy && _oseroGridScripts[newposition].PGridMode == GridMode.White)
        {
            foreach (var c in _changeList) { c.PGridMode = GridMode.White; }
            return;
        }

        if (_oseroGridScripts[newposition].PGridMode == GridMode.Black)
        {
            _changeList.Add(_oseroGridScripts[newposition]);
            WhiteAttackAsterisk( x, y , newx, newy , true);
        }
    }
    void ResetCanput()
    {
        foreach (var b in _oseroGridScripts)
        {
            if (b.PGridMode == GridMode.CanPut) { b.PGridMode = GridMode.Empty; }
        }
    }


    void Update()
    {
        //wasd move
        if (Input.GetKeyDown(KeyCode.W) && SelectPoint < 63 - 7) { SelectPoint += +8; }

        if (Input.GetKeyDown(KeyCode.S) && SelectPoint > 0 + 7 ) { SelectPoint += -8; }

        if (Input.GetKeyDown(KeyCode.A) && SelectPoint > 0) { SelectPoint += -1; }

        if (Input.GetKeyDown(KeyCode.D) && SelectPoint < 63) { SelectPoint += +1; }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_oseroGridScripts[_selectPoint].PGridMode == GridMode.CanPut)
            {
                if ( _ternEnum == Tern.Black)
                {
                    _oseroGridScripts[_selectPoint].PGridMode = GridMode.Black;
                    BlackAttack();
                    WhiteTurn();
                }
                else if (_ternEnum == Tern.White)
                {
                    _oseroGridScripts[_selectPoint].PGridMode = GridMode.White;
                    WhiteAttack();
                    BlackTurn();
                }
            }
        }
    }


    public enum Tern { Empty, Black, White, Result }
}