using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerer : MonoBehaviour
{
    [SerializeField] private int[] savedCombo;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseAnimation(string combo)
    {
        switch (combo)
        {
            case "000000":
               Debug.Log("hOLA");
                break;
            case "001100":
                Debug.Log("SON 5");
                break;
            case "000111":
                Debug.Log("SON 4");
                break;
            
        }
    }
}
