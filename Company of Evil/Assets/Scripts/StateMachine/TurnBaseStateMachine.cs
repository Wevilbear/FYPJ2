using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseStateMachine : MonoBehaviour {

    public enum BattleStates
    {
        START,
        PLAYERCHOICE,
        ENEMYCHOICE,
        LOSE,
        WIN
    }

    private BattleStates currentState;


	// Use this for initialization
	void Start () {
        currentState = BattleStates.START;
		
	}
	
	// Update is called once per frame
	void Update () {
        switch(currentState)
        {
            case (BattleStates.START):
                //SETUP BATTLE FUNCTION
                break;
            case (BattleStates.PLAYERCHOICE):
                break;
            case (BattleStates.ENEMYCHOICE):
                break;
        }
		
	}
}
