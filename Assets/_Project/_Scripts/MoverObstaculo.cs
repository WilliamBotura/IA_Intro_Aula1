using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObstaculo : MonoBehaviour
{
    #region Variables
    //Variables
    public Vector3 sentidoMovimento = Vector3.left;
    public float vel = 2.0f;
    private float delayInverter = 2.0f;
    private int direcao = 1;
    #endregion Variables

    #region DefaultMethods
    //Default Methods
    private void Awake()
    {
        //nameof is used to avoid typing errors in the method declaration
        InvokeRepeating(nameof(InverterDirecao), delayInverter, delayInverter);
    }

    private void Update()
    {
        transform.Translate(sentidoMovimento * direcao * vel * Time.deltaTime, Space.World);
    }
    #endregion DefaultMethods

    #region Methods
    //Methods
    private void InverterDirecao()
    {
        direcao *= -1;
    }
    #endregion Methods
}