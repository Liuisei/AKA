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
        if (gridModeOriginal == GridMode.CanPut || gridModeOriginal == GridMode.Empty)
        {
            if (gridModeNew == GridMode.Black ) { PlacePieceBlack(); }
            else { PlacePieceWhite(); }
        }
        else
        {
            if (gridModeNew == GridMode.Black)
            {
                if (gridModeOriginal == GridMode.Black) { TurnToBlack(); }
                else { TurnToWhite(); }
            }
        }
    }
    //placeBlackPiece

    void EmptyGridAnim()
    {
   
    }

    void PlacePieceBlack()
    {
        Debug.Log("1");
        _animation.SetInteger("State",2);
    }
    void PlacePieceWhite()
    {
        _animation.SetInteger("State",1);
    }

    void TurnToBlack() { Debug.Log("Turning to Black"); }
    void TurnToWhite() { Debug.Log("Turning to White"); }


    void OnMouseDown() { Debug.Log($"X: {_x}, Y: {_y}, GridProperties: {PGridMode}" ); }
}

public enum GridMode { Empty, White, Black, CanPut, }