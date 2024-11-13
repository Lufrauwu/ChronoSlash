using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelExterior : MonoBehaviour
{
    //Arrays de objetos que el jugador va a coleccionar
    public GameObject[] Objetos;
    public GameObject[] ObjetosJugador;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Collectible")
        {
            print("Agarrado");
            //other.GetComponent<>().MeshRenderer
        }
    }

}
