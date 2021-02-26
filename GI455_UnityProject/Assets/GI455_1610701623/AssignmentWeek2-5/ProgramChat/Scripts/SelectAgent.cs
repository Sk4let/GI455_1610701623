using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAgent : MonoBehaviour
{

    public GameObject agent1;
    public GameObject agent2;
    public GameObject agent3;
    public GameObject agent4;
    public int number;

    void Start() 
    {
        number = Random.Range(0,4);
        
        if(number == 0)
        {
            agent1.gameObject.SetActive(true);
        }
        else if ( number == 1)
        {
            agent2.gameObject.SetActive(true);
        }
         else if ( number == 2)
        {
            agent3.gameObject.SetActive(true);
        }
        else if ( number == 3)
        {
            agent4.gameObject.SetActive(true);
        }
        else 
        {
            agent1.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.F1))
        {
            agent1.gameObject.SetActive(true);
            agent2.gameObject.SetActive(false);
            agent3.gameObject.SetActive(false);
            agent4.gameObject.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.F2))
        {
            agent1.gameObject.SetActive(false);
            agent2.gameObject.SetActive(true);
            agent3.gameObject.SetActive(false);
            agent4.gameObject.SetActive(false);

        }
        else if (Input.GetKey(KeyCode.F3))
        {
            agent1.gameObject.SetActive(false);
            agent2.gameObject.SetActive(false);
            agent3.gameObject.SetActive(true);
            agent4.gameObject.SetActive(false);
            
        }
        else if (Input.GetKey(KeyCode.F4))
        {
            agent1.gameObject.SetActive(false);
            agent2.gameObject.SetActive(false);
            agent3.gameObject.SetActive(false);
            agent4.gameObject.SetActive(true);
            
        }

        
    }
}
