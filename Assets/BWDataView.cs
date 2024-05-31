using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BWDataView : MonoBehaviour
{
    [SerializeField] Text _blackCountText;
    [SerializeField] Text _whiteCountText;

    [SerializeField] Text _turnText;
    [SerializeField] Text _gameStateText;

    [SerializeField] Text _blackTimeText;
    [SerializeField] Text _whiteTimeText;


    public void SetBlackCount(int count) { _blackCountText.text = count.ToString(); }

    public void SetWhiteCount(int count) { _whiteCountText.text = count.ToString(); }

    public void SetTurn(string turn) { _turnText.text = turn; }

    public void SetGameState(string state) { _gameStateText.text = state; }

    public void SetBlackTime(float time) { _blackTimeText.text = time.ToString("F2"); }

    public void SetWhiteTime(float time) { _whiteTimeText.text = time.ToString("F2"); }
    void OnEnable()
    {
        OseroManager.Instance.OnBlackCountChange += SetBlackCount;
        OseroManager.Instance.OnWhiteCountChange += SetWhiteCount;
    }
    void OnDisable()
    {
        OseroManager.Instance.OnBlackCountChange -= SetBlackCount;
        OseroManager.Instance.OnWhiteCountChange -= SetWhiteCount;
    }
}