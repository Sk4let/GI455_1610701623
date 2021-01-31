using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FindItemScript : MonoBehaviour
{
    public InputField _searchtxt;
    public TextMeshProUGUI _text;

    // Item List { Sword , Bow , Hammer , Gun , Knife}

    public void sendResult()
    {
        if (_searchtxt.text == "Sword")
        {
             _text.text = " " + _searchtxt.text + " " + " is found.";
             _text.color = Color.green;
        }
        else if (_searchtxt.text == "Bow")
        {
            _text.text = " " + _searchtxt.text + " " + " is found.";
            _text.color = Color.green;
        }
        else if (_searchtxt.text == "Hammer")
        {
            _text.text = " " + _searchtxt.text + " " + " is found.";
            _text.color = Color.green;
        }
        else if (_searchtxt.text == "Gun")
        {
            _text.text = " " + _searchtxt.text + " " + " is found.";
            _text.color = Color.green;
        }
        else if (_searchtxt.text == "Knife")
        {
            _text.text = " " + _searchtxt.text + " " + " is found.";
            _text.color = Color.green;
        }
            // If blank
        else if ( string.IsNullOrEmpty(_searchtxt.text))
        {
            _text.text = " Please Enter Item Name.";
             _text.color = Color.red;
        }
        else 
        {
            // Not Found
            _text.text = " " + _searchtxt.text + " " + " is not found.";
            _text.color = Color.red;
        }


    }
}
