using System;
using UnityEngine;
using UnityEngine.XR.iOS;
using System.Collections.Generic;
using System.Collections;

namespace Tracking 
{
	//[RequireComponent(typeof(MultiplayerClient.UI.Localization))]
	public class PositioningBehaviour : MonoBehaviour 
	{
		public delegate void PositioningEvent(GameObject go);
		public event PositioningEvent planeFound;

		private GameObject _previousAnchor;

		private LinkedList<ARPlaneAnchorGameObject> _planes = new LinkedList<ARPlaneAnchorGameObject>();
		private UnityARAnchorManager _unityARAnchorManager;

		private Coroutine _localizationCoroutine;

		private void Start()
		{
			_unityARAnchorManager = new UnityARAnchorManager();
			StartLocalization();
		}

		public void StartLocalization()
		{
			_localizationCoroutine = StartCoroutine (Localization ());
		}

		public void StopLocalization()
		{
			if (_localizationCoroutine != null) 
			{
				StopCoroutine (_localizationCoroutine);
				_isFounding = false;
				_isFindingPlane = false;
			}
		}

		public bool IsFounding { get { return _isFounding; } }
		private bool _isFounding = false;

		public bool IsFindingPlane 
		{ 
			get { return _isFindingPlane; } 
		}
		private bool _isFindingPlane = false;

		public int CountPlane
		{
			get
			{
				if (_planes != null)
				{
					return _planes.Count;
				}
				else
				{
					return 0;
				}
			}
		}

		private IEnumerator Localization()
		{
			Debug.Log("<color=cyan>Start localization</color>");
			_isFounding = true;

			while (!FindPlane())
			{			
				yield return null;
			}
			_isFounding = false;

		}
		private bool FindPlane()
		{
			Debug.Log("<color=cyan>FindPlane</color>");
			_planes = _unityARAnchorManager.GetCurrentPlaneAnchors();
			if (_planes.Count > 0)
			{
				Debug.Log("<color=cyan>FindPlane Count = " + _planes.Count + "</color>");
				// prioritize reults types
				ARHitTestResultType resultType = ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane;//ARHitTestResultTypeHorizontalPlane;
				ARPoint point = new ARPoint
				{
					x = Screen.width / 2,
					y = Screen.height / 2
				};
				List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultType);
				if (hitResults.Count > 0 && Input.touchCount > 0)
				{
					OnPlaneFound(hitResults[0]);
					return true;
				}
			}

			return false;
		}

		private void OnPlaneFound(ARHitTestResult hitResult) 
		{
			Debug.Log("<color=cyan>OnPlaneFound</color>");

			GameObject anchor = new GameObject ("game world");

			anchor.transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
			anchor.transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);

			planeFound (anchor);

			if (_previousAnchor != null)
				Destroy(_previousAnchor);
			
			_previousAnchor = anchor;
		}
	
	}
}
