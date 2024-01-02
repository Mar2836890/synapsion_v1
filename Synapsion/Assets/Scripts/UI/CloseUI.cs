using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
public void CloseObj(GameObject _objToDisable)
    {
        _objToDisable.SetActive(false);
    }
}
