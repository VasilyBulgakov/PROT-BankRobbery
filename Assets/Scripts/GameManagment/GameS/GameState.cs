using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GamesManagement
{
	public enum GameStates{
		finished,
		failed,
		canceled,
		inProgress
	}

	public class GameState{
		public GameStates gameState = GameStates.inProgress;
		public Transform tr;	
	
        public GameState() { }

        public GameState(Transform another) {
			tr.SetPositionAndRotation(another.position, another.rotation);            
            tr.localScale = another.localScale;
        }

        public void UpdateState(GameState another) {
          	tr.SetPositionAndRotation(another.tr.position, another.tr.rotation);            
          	tr.localScale = another.tr.localScale;
	    }
	}
}