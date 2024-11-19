using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character_Movement_Mayorga : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey("w")){
            animator.SetBool("Is_Walking", true); 
        }

        if(Input.GetKeyUp("w")){
            animator.SetBool("Is_Walking", false); 
        }

        if(Input.GetKey("w") && Input.GetKey("e")){
            animator.SetBool("Is_Running", true); 
        }

        if(Input.GetKeyUp("q")){
            animator.SetBool("Is_Running", false); 
        }

        if(Input.GetKey("m")){
            animator.SetBool("Attacking", true); 
        }
    }
}
