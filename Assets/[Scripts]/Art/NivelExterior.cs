using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelExterior : MonoBehaviour
{
    //Arrays de objetos que el jugador va a coleccionar
    public List<GameObject> Objetos = new List<GameObject>();
    public List<GameObject> ObjetosJugador = new List<GameObject>();



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
            Collected();
            //other.GetComponent<>().MeshRenderer
        }
    }

    private void Collected()
    {
        print("Agarrado");
    }
}
