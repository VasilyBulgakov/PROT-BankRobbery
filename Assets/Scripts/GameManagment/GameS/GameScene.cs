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

		private void FixedUpdate() {
			// if( Vector3.SqrMagnitude(Camera.main.transform.position - _state.tr.position) > 25f )
			// 	cancel();
		}


		private void Start() {			
			begin();			
		}

		public virtual void SetState(GameState st) {
            if (st != null) {                
                _state.UpdateState(st as GameState);

                mSceneObject.transform.SetPositionAndRotation(_state.tr.position, _state.tr.rotation);                
                mSceneObject.transform.localScale = _state.tr.localScale;
            }
        }

		public void win(){
			_state.gameState = GameStates.finished;
			onWin.Invoke();
		}	
		public void lose(){
			_state.gameState = GameStates.failed;
			onLose.Invoke();
		}
		public void cancel(){
			_state.gameState = GameStates.canceled;			
			onCancel.Invoke();
		}
		public void begin(){
			_state.gameState = GameStates.inProgress;
		}
	}
}