using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevador : MonoBehaviour
{
    private int LocationsIndex = 0;
    public Vector3[] locations;
    private bool PlayerOn = false;

    private void Update()
    {
        if (PlayerOn == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, locations[1], 2 * Time.deltaTime);
        }
        if (PlayerOn == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, locations[0], 2 * Time.deltaTime);
        }
    }

    //No esta moviendose constantemente
    private void OnTriggerEnter(Collider other)
    {
     
        if(other.gameObject.tag == "Player")
        {
            //transform.position = Vector3.MoveTowards(transform.position, locations[1], 2 * Time.deltaTime);
            print("PlayerOn");
            PlayerOn = true;       
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //transform.position = Vector3.MoveTowards(transform.position, locations[1], 2 * Time.deltaTime);
            print("PlayerOff");
            PlayerOn = false;
        }
    }

}
