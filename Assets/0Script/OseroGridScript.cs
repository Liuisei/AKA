using UnityEngine;


public class OseroGridScript : MonoBehaviour
{
    int _x;
    int _y;

    GridMode _gridMode = GridMode.Empty;

    [SerializeField] Animator     _animation;


    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }

    public GridMode PGridMode
    {
        get { return _gridMode; }
        set
        {
            Debug.Log("GridMode changed to " + value);
            GridChangeAnimation(_gridMode, value);
            _gridMode = value;
        }
    }

    void Awake()
    {
        if (_animation == null) { Debug.LogError("Animation is not assigned"); }
    }

    void Start() { EmptyGridAnim(); }

    void GridChangeAnimation(GridMode gridModeOriginal, GridMode gridModeNew)
    {
        if (gridModeOriginal == GridMode.CanPut )
        {
            if (gridModeNew == GridMode.Empty)
            {
                GetComponent<Renderer>().material = OseroManager.Instance._normalMaterial;
                return;
            }

            if (gridModeNew == GridMode.Black )
            {
                GetComponent<Renderer>().material = OseroManager.Instance._normalMaterial;
                PlacePieceBlack();
            }

            if (gridModeNew == GridMode.White)
            {
                GetComponent<Renderer>().material = OseroManager.Instance._normalMaterial;
                PlacePieceWhite();
            }
        }
        else if (gridModeOriginal == GridMode.Empty)
        {
            if (gridModeNew == GridMode.CanPut)
            {
                var material = GetComponent<Renderer>().material;
                GetComponent<Renderer>().material = OseroManager.Instance._canPutMaterial;
                return;
            }

            if (gridModeNew == GridMode.Black ) { PlacePieceBlack(); }

            if (gridModeNew == GridMode.White) { PlacePieceWhite(); }
        }
        else if (gridModeOriginal == GridMode.Black || gridModeOriginal == GridMode.White)
        {
            if (gridModeNew == GridMode.Black) { TurnToBlack(); }
            if (gridModeNew == GridMode.White ) { TurnToWhite(); }
        }
    }
    //placeBlackPiece

    void EmptyGridAnim() { }

    void PlacePieceBlack() { _animation.SetInteger("State", 2); }
    void PlacePieceWhite() { _animation.SetInteger("State", 1); }

    void TurnToBlack() { _animation.SetInteger("State", 3); }
    void TurnToWhite() { _animation.SetInteger("State", 4); }


    void OnMouseDown() { Debug.Log($"X: {_x}, Y: {_y}, GridProperties: {PGridMode}" ); }
}

public enum GridMode { Empty, White, Black, CanPut, }