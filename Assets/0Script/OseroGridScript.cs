using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OseroGridScript : MonoBehaviour
{
    int _x;
    int _y;

    GridMode _gridMode = GridMode.Empty;

    [SerializeField] Animation     _animation;
    [SerializeField] AnimationClip _comaAnimationEmpty;
    [SerializeField] AnimationClip _comaAnimationPB;
    [SerializeField] AnimationClip _comaAnimationPW;
    [SerializeField] AnimationClip _comaAnimationCTB;
    [SerializeField] AnimationClip _comaAnimationCTW;


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

        if (_comaAnimationPB == null || _comaAnimationPW == null || _comaAnimationCTB == null || _comaAnimationCTW == null) { Debug.LogError("Animation Clips are not assigned"); }
    }

    void Start() { EmptyGridAnim(); }

    void GridChangeAnimation(GridMode gridModeOriginal, GridMode gridModeNew)
    {
        if (gridModeOriginal == GridMode.CanPut)
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
        _animation.clip = _comaAnimationEmpty;
        _animation.Play();
    }

    void PlacePieceBlack() { Debug.Log("Placing Black Piece"); }
    void PlacePieceWhite() { Debug.Log("Placing White Piece"); }

    void TurnToBlack() { Debug.Log("Turning to Black"); }
    void TurnToWhite() { Debug.Log("Turning to White"); }


    void OnMouseDown() { Debug.Log($"X: {_x}, Y: {_y}, GridProperties: {PGridMode}" ); }
}

public enum GridMode { Empty, White, Black, CanPut, }