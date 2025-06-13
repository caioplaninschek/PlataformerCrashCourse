using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
    [Tooltip("Nome do parâmetro float a ser atualizado")]
    public string floatName;


    [Tooltip("Atualizar o parâmetro float ao entrar no estado?")]
    public bool updateOnStateEnter;

    [Tooltip("Atualizar o parâmetro float ao sair do estado?")]
    public bool updateOnStateExit;


    [Tooltip("Atualizar o parâmetro float ao entrar da state machine?")]
    public bool updateOnStateMachineEnter;

    [Tooltip("Atualizar o parâmetro float ao sair do state machine?")]
    public bool updateOnStateMachineExit;


    [Tooltip("Valor do parâmetro float ao entrar no estado ou da state machine")]
    public float valueOnEnter;

    [Tooltip("Valor do parâmetro float ao sair do estado ou da state machine")]
    public float valueOnExit;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }

    // OnStateMachineEnter is called when entering a state machine via its Entry node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }
}