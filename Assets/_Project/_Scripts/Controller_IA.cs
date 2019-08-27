using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Controller_IA : MonoBehaviour
{
    #region VARIABLES

    //Creates a Enum for defining id's for the states.
    public enum Estados
    {
        ESPERAR,
        PATRULHAR,
        PERSEGUIR,
        PROCURAR
    }

    private Estados estadoAtual;
    private Transform alvo;
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private AICharacterControl aiCharacterControl;

    //Estado: Esperar
    [Header("Estado: Esperar")]
    public float tempoEsperar = 2.0f;
    private float tempoEsperando = 0.0f;

    //Estado: Patrulhar
    [Header("Estado: Patrulhar")]
    public Transform waypoint1;
    public Transform waypoint2;
    private Transform waypointAtual;
    public float distanciaMinimaWaypointAtual = 1.0f;
    private float distanciaWaypointAtual;

    //Estado: Perseguir
    [Header("Estado: Perseguir")]
    float distanciaPlayer;
    float distanciaMinimaPlayer = 3.0f;

    //Estado: Procurar
    [Header("Estado: Procurar")]
    float tempoProcurar = 5.0f;
    private float tempoProcurando = 0.0f;

    #endregion VARIABLES

    private void Awake()
    {
        //Components
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    private void Start()
    {
        //gets player transform through gameObject
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //Makes sure that waypointAtual will never be null
        waypointAtual = waypoint1;
        //Makes sure that when the game starts, Esperar will be the default method;
        Esperar();
    }

    private void Update()
    {
        //Methods
        //Esperar();
        ChecarEstados();
    }

    private void ChecarEstados()
    {
        if (estadoAtual != Estados.PERSEGUIR && VisaoJogador())
        {
            Perseguir();
            return;
        }

        //using to define behaveour of the AI
        //enter + enter on adding estadoAtual to the switch adds the states
        switch (estadoAtual)
        {
            case Estados.ESPERAR:
                //Verifies if the playes has waited for the determined time, if not, he will, if he did, go into the Patrulhar state;
                if (EsperouTempoSuficiente())
                {
                    Patrulhar();
                }
                else
                {
                    alvo = transform;
                }
                break;
                //verifies if player is close to the waypoint. If he is, waits again and changes target. If not, he goes in the waypoint direction.
            case Estados.PATRULHAR:
                if (PertoWaypointAtual())
                {
                    //tell the AI to wait before changing waypoints
                    Esperar();
                    //change waypoints
                    AlternatWaypoint();
                }
                else
                {
                    alvo = waypointAtual;
                }
                break;
                //verifies if IA doesn't have vision of the player. If he has, he will chase him, if not, he will wait
            case Estados.PERSEGUIR:
                if (!VisaoJogador())
                {
                    Procurar();
                }
                else
                {
                    alvo = playerTransform;
                }
                break;
                //Looks for the player after he left the AI field of vision. If he looks for 5 seconds and don't see him, go to Esperar state.
            case Estados.PROCURAR:
                if (ProcurouTempoSuficiente())
                {
                    Esperar();
                }
                else
                {
                    alvo = null;
                }
                break;
        }

        //gets alvo from the AICharacterController in the componets
        if (aiCharacterControl)
        {
            aiCharacterControl.SetTarget(alvo);
        }
        //but if he doesn't have the script in its compenets, it will check for the alvo in itself
        if (alvo != null)
        {
            navMeshAgent.destination = alvo.position; 
        }
    }

    #region ESPERAR

    //Region defined for the ESPERAR method
    private void Esperar()
    {
        //declares the estadoAtual from enum ESTADOS as ESPERAR;
        estadoAtual = Estados.ESPERAR;
        //defines the initial time as the time the game is running
        tempoEsperando = Time.time;
    }

    private bool EsperouTempoSuficiente()
    {
        //makes the operation that validates if the player has been waiting for the wanted time
        return tempoEsperando + tempoEsperar <= Time.time;
    }

    #endregion ESPERAR

    #region PATRULHAR

    private void Patrulhar()
    {
        estadoAtual = Estados.PATRULHAR;
    }

    private bool PertoWaypointAtual()
    {
        //creates a oparations that obtains the distance between the IA distance relative to the waypoint that is it's target
        distanciaWaypointAtual = Vector3.Distance(transform.position, waypointAtual.position);
        //if the distance is greater than the minimal distance, it will be its target, if the distance is lesser than the minimal
        //waypoint will be changed to the next one on the list
        return distanciaWaypointAtual <= distanciaMinimaWaypointAtual;
    }

    private void AlternatWaypoint()
    {
        //if that states             (if)             (than)      (else)
        waypointAtual = waypointAtual == waypoint1 ? waypoint2 : waypoint1;
    }

    #endregion PATRULHAR

    #region PERSEGUIR
    private void Perseguir()
    {
        estadoAtual = Estados.PERSEGUIR;
    }

    private bool VisaoJogador()
    {
        distanciaPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanciaPlayer <= distanciaMinimaPlayer;
    }
    #endregion PERSEGUIR

    #region PROCURAR

    //state that defines the AI to look for the player after losing sight of him
    private void Procurar()
    {
        estadoAtual = Estados.PROCURAR;
        tempoProcurando = Time.time;
    }

    private bool ProcurouTempoSuficiente()
    {
        return tempoProcurando + tempoProcurar <= Time.time;
    }

    #endregion PROCURAR
}