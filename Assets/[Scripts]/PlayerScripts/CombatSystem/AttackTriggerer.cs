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

    public void ChooseAnimation(List<int> input)
    {
        switch (input.Count)
        {
            case 6:
                savedCombo = new int[6];
                foreach (int comboInput in input)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (comboInput == 0) 
                        {
                            savedCombo[i] = 0;
                        }
                        else if (comboInput == 1)
                        {
                            savedCombo[i] += 1;
                        }
                        Debug.Log(savedCombo[i]);
                    }
                }
               
                break;
            case 5:
                Debug.Log("SON 5");
                break;
            case 4:
                Debug.Log("SON 4");
                break;
            
        }
    }
}
