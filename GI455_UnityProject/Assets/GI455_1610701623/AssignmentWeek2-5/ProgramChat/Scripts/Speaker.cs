using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class Speaker : MonoBehaviour
{
    Toggle myToggle;
   AudioSource audioSource;
   // Start is called before the first frame update
   void Start()
   {
      audioSource = GetComponent<AudioSource>();
      myToggle = GetComponent<Toggle>();
      if(AudioListener.volume == 0)
      {
          myToggle.isOn = false;
      }
   }
   public void SpeakerToggle(bool isOn)
   {
       if(isOn)
       {
           AudioListener.volume = 1;
       }
       else 
       {
           AudioListener.volume = 0;
       }
   }
}
