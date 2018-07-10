using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GamesManagement
{
	public class GameScene : MonoBehaviour {

		[Header("Universal Inteface for GameEvents")]
		public UnityEvent onWin;
		public UnityEvent onLose;
		public UnityEvent onCancel;

		public Transform mSceneObject;

        protected GameState _state = new GameState();	
		public GameState state{
			get{
				return _state;
			}	
			private set{
				_state = value;
			}		
		}

		private void Start() {
			begin();			
		}

		public void win(){
			
			onWin.Invoke();
		}	
		public void lose(){
			onLose.Invoke();
		}
		public void cancel(){
			onCancel.Invoke();
		}
		public void begin(){
		}
	}
}