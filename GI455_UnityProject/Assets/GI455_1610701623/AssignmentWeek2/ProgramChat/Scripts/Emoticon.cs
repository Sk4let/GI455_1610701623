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

   public void EmotionIsHello()
   {
      animator.SetTrigger("Hello");
   }
   public void EmotionIsAngry()
   {
      animator.SetTrigger("Angry");
   }
   public void EmotionIsDance()
   {
      animator.SetTrigger("Dance");
   }
}
