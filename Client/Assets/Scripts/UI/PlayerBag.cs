using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBag : MonoBehaviour
{
    public ScrollRect scrollRect;

    public GameObject grid;

    public GameObject cell;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        var capacity = 6;
        for (int i = 0; i < capacity; i++)
        {
            Transform uiTran = (GameObject.Instantiate(cell) as GameObject).transform;
            uiTran.gameObject.SetActive(true);
            uiTran.parent = grid.transform;
            uiTran.localScale = Vector3.one;
            uiTran.localPosition = Vector3.zero;
        }
    }
}
