using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevador : MonoBehaviour
{
    private int LocationsIndex = 0;
    public GameObject[] locations;
    private void OnTriggerEnter(Collider other)
    {
     if(other.gameObject.tag == "Player")
        {
            //transform.position = Vector3.MoveTowards(transform.position, locations[1].position, 2 * Time.deltaTime);
            print("PlayerOn");
        }
        else
        {
            //transform.position = Vector3.MoveTowards(transform.position, locations[0].position, 2 * Time.deltaTime);
            print("PlayerOff");
        }
    }


}
