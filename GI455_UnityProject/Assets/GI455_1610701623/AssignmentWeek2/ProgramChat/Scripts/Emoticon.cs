using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emoticon : MonoBehaviour
{
    public Animator animator;
    public Text inputField;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChatEmote()
    {
       if(inputField.text == "Hello")
            {
                animator.SetBool("isHello", true);
                animator.SetBool("isHello", false);
            }
    }
}
