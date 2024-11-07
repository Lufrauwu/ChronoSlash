using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerer : MonoBehaviour
{
    [SerializeField] private int[] savedCombo;
    [SerializeField] private Animator animator;
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
               animator.SetBool("Slash", true);
               combo = "";
                break;
            case "001100":
                Debug.Log("SON 5");
                break;
            case "000111":
                Debug.Log("SON 4");
                break;
            case "111000":
                break;
            case "110011":
                break;
            case "100010":
                break;
            case "111111":
                break;
            case "00000":
                break;
            case "00011":
                break;
            case "11000":
                break;
            case "11100":
                break;
            case "11111":
                break;
            case "0000":
                break;
            case "0001":
                break;
            case "0011":
                break;
            case "0101":
                break; 
            case "1000":
                break;  
            case "1100":
                Debug.Log("SI");
                break;
            case "1110":
                break;
            

            
        }
    }
}
