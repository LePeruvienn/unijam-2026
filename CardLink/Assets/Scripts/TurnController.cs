using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public enum TurnPhase { ChooseAction, Play, ChooseAttacker, ChooseDefender, Resolution, GameOver }

public class TurnController : MonoBehaviour
{
    [SerializeField] private GameObject panelAction;
    [SerializeField] private GameObject PlayResolve;
    [SerializeField] private GameObject AttackResolve;
    [SerializeField] private GameObject DefendResolve;
    [SerializeField] private GameObject ResolutionResolve;
    [SerializeField] private Transform CamP1;
    [SerializeField] private Transform CamP2;

    private Camera _cam;
    public TurnPhase Phase { get; private  set; }
    
    public PlayerId CurrentPlayer { get; private  set; }
    
    public event Action<TurnPhase, PlayerId> PhaseChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = Camera.main;
        SetPhase(TurnPhase.ChooseAction, PlayerId.P1);
    }

    void SetPhase(TurnPhase turnPhase, PlayerId decider)
    {
        CurrentPlayer = decider;
        Phase =  turnPhase;
        
        panelAction.SetActive(Phase == TurnPhase.ChooseAction);
        PlayResolve.SetActive(Phase == TurnPhase.Play);
        AttackResolve.SetActive(Phase == TurnPhase.ChooseAttacker);
        DefendResolve.SetActive(Phase == TurnPhase.ChooseDefender);
        ResolutionResolve.SetActive(Phase == TurnPhase.Resolution);
        
        Debug.Log(decider);
        var anchor = decider ==  PlayerId.P1 ? CamP1 : CamP2;
        Debug.Log(anchor);
        _cam.transform.SetPositionAndRotation(anchor.position, anchor.rotation);
        
        PhaseChanged?.Invoke(turnPhase, decider);
    }

    public void Play()
    {
        SetPhase(TurnPhase.Play, CurrentPlayer);
    }

    public void Attack()
    {
        SetPhase(TurnPhase.ChooseAttacker, CurrentPlayer);
    }

    public void Next()
    {
        var other = CurrentPlayer.OtherPlayer();
        switch (Phase)
        {
            case TurnPhase.Play:
                SetPhase(TurnPhase.Resolution, CurrentPlayer);
                break;
            case TurnPhase.ChooseAttacker:
                SetPhase(TurnPhase.ChooseDefender, other);
                break;
            case TurnPhase.ChooseDefender:
                SetPhase(TurnPhase.Resolution, other);
                break;
            case TurnPhase.Resolution:
                SetPhase(TurnPhase.ChooseAction, other);
                break;
        }
    }

}
