using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelExterior : MonoBehaviour
{
    //Arrays de objetos que el jugador va a coleccionar
    public List<GameObject> Objetos = new List<GameObject>();
    public List<GameObject> ObjetosJugador = new List<GameObject>();

    public bool NivelTerminado = false;
    [SerializeField] private GameObject Nivelganaste;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Objetos.Count == ObjetosJugador.Count)
        {
            NivelTerminado = true;
            print (NivelTerminado);
        }

        if (NivelTerminado)
        {
            Nivelganaste.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Collectible")
        {
            ObjetosJugador.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

}
