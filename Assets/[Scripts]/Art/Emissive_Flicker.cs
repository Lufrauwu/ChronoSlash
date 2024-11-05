using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emissive_Flicker : MonoBehaviour
{
	public AnimationCurve EmissiveCurve;
	public float CurveSpeed = 1f;

	public Material Emmaterial;
    //private MeshRenderer Emission;
    private float Intensity;




    private void Awake()
    {
        //Emission = GetComponent<MeshRenderer>();
        Intensity = Emmaterial.GetFloat("EmissionIntensity");

    }
	void Update()
	{
        //Obtener Emisivo de un material
        //No esta detectando el Material de los Objetos
        //Emission.GetComponent<Material>().SetFloat("Emission Intensity", (Intensity * EmissiveCurve.Evaluate(Time.time * CurveSpeed)));
        //No esta Moviendo el valor que se le pide
        //Intensity = Intensity*EmissiveCurve.Evaluate(Time.time*CurveSpeed);
        print(Intensity);
        //Emmaterial.SetFloat(Intensity, (Intensity * EmissiveCurve.Evaluate(Time.time * CurveSpeed)));
    }
}
