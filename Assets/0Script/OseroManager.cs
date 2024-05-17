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

    [SerializeField] int _maxSquareSf = 1;

    [SerializeField] float _oserorangeSf = 1;

    int        SelectPoint = 0;

    public int SelectPoint1
    {
        get
        {
            return SelectPoint;
        }
        set
        {
            SelectPoint = value;
            SelectPrefubPosition();
        }
    }

    void SelectPrefubPosition() { _selectPrefab.transform.position = new Vector3(SelectPoint % 8, 0.2f, SelectPoint * SelectPoint / 8); }

    int   _tern      = 0;
    float _time      = 0;
    float _blackTime = 0;
    float _whiteTime = 0;

    void Start() { SetUpGrid(); }
    void SetUpGrid()
    {
        for (int i = 0; i < 64; i++)
        {
            GameObject go = Instantiate(_oseroGridPrefabSf, new Vector3(i % 8 * _oserorangeSf, 0, i / 8 * _oserorangeSf), Quaternion.identity);
            _oseroGridScripts[i] = go.GetComponent<OseroGridScript>();
        }
    }


    void Update()
    {
        //wasd move
        if (Input.GetKeyDown(KeyCode.W) && SelectPoint < 63 - 8) { SelectPoint = +8; }

        if (Input.GetKeyDown(KeyCode.S) && SelectPoint > 0 + 8 ) { SelectPoint = -8; }

        if (Input.GetKeyDown(KeyCode.A) && SelectPoint > 0) { SelectPoint = -1; }

        if (Input.GetKeyDown(KeyCode.D) && SelectPoint < 63) { SelectPoint = +1; }
    }


    public enum Tern { Empty, Black, White, Result }
}