using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetDestination : MonoBehaviour
{
    //componets
    public Transform alvo;
    public NavMeshAgent navMeshAgent;

    private void Start()
    {
    }

    private void Update()
    {
        Movimentar();
    }

    private void Movimentar()
    {
        //you could use the navMeshAgent.SetDestination, but we are using the .destinantion-
        //navMeshAgent.destination = alvo.position;
    }
}