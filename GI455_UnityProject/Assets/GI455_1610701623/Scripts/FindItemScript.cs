using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FindItemScript : MonoBehaviour
{
    public InputField _searchtxt;
    public TextMeshProUGUI _text;
    public GameObject _item1;

    public void sendResult()
    {
        _text.text = _searchtxt.text;
    }
}
