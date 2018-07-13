using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.iOS;
using System.Collections;

namespace Tracking {
    public class TrackingManager : MonoBehaviour {
		public GameObject Stage;
        public TrackableWorld TrackableWorld;

        private bool _inPlace = false;
        private List<WallMarker> _allAnchors = new List<WallMarker>();

        private List<WallMarker> _currentAnchors = new List<WallMarker>();
        private WallMarker _mainAnchor = null;
		        
        private Coroutine _coroutineLocalization;

        public GameObject anchorHighlight;
        private GameObject _anchorHighlight;

        private void Start() {
			if (Stage == null) {
				Debug.Log("AnchorStage must be specified");
				return;
			}
			Stage.SetActive(false);

            foreach (var marker in TrackableWorld.Anchors)
            {
                _allAnchors.Add(marker);
                marker.detected += OnMarkerDetected;
                marker.update += OnMarkerUpdate;
                marker.detectionLost += OnMarkerLost;
            }
            // if(anchorHighlight != null)
            //     _anchorHighlight = GameObject.Insta

            //FloorMarker.SetActive(false);

			FindObjectOfType<PositioningBehaviour>().planeFound += OnPlaneFound;
            //			UnityARSessionNativeInterface.ARAnchorAddedEvent += OnPlaneFound;
        }

        private void OnDestroy()
        {
            foreach (var marker in _allAnchors)
            {
                marker.detected -= OnMarkerDetected;
                marker.update -= OnMarkerUpdate;
                marker.detectionLost -= OnMarkerLost;
            }
        }

        private void OnPlaneFound(GameObject anchor)
        {
            //Stage.transform.parent = anchor.transform;
            //Stage.transform.localPosition = Vector3.zero;
            //Stage.transform.localRotation = Quaternion.identity;
            Stage.SetActive(true);
            Debug.Log("ONPlaneFound");
        }

        /*public void StartLocalization()
        {
            if (_coroutineLocalization != null)
            {
                StopCoroutine(_coroutineLocalization);
            }
            _coroutineLocalization = StartCoroutine(Localization());
            return;
        }*/

        private void OnMarkerDetected(WallMarker marker)
        {
            if (Stage.activeSelf && !_inPlace)
            {
                //FindObjectOfType<GameManager>().PlayerReady();
                _inPlace = true;
            }

            _currentAnchors.Add(marker);
            // if (_currentAnchor == null) {
            TrackableWorld.CorrectWithAnchor(marker);
            _mainAnchor = marker;
            // }
        }

        private void OnMarkerUpdate(WallMarker marker)
        {
            TrackableWorld.CorrectWithAnchor(marker);
        }

        private void OnMarkerLost(WallMarker marker)
        {
            _currentAnchors.Remove(marker);
            if (marker == _mainAnchor)
            {
                if (_currentAnchors.Count > 0)
                {
                    // make another anchor main
                    _mainAnchor = _currentAnchors[_currentAnchors.Count - 1];
                }
            }
        }
    /*    #region correct world anchor
        public void TranslateForwardAnchor()
		{			
			anchor.transform.Translate (anchor.transform.forward * 0.1f);
		}
		public void TranslateBackwardAnchor()
		{			
			anchor.transform.Translate (anchor.transform.forward * -0.1f);			
		}
		public void TranslateLeftAnchor()
		{			
			anchor.transform.Translate (anchor.transform.right * -0.1f);
		}
		public void TranslateRightAnchor()
		{			
			anchor.transform.Translate (anchor.transform.right * 0.1f);		
		}
		public void TranslateUpAnchor()
		{			
			anchor.transform.Translate (anchor.transform.up * 0.1f);
		}
		public void TranslateDownAnchor()
		{			
			anchor.transform.Translate (anchor.transform.up * -0.1f);		
		}
		public void RotateRightAnchor()
		{
			anchor.transform.Rotate (Vector3.up, 3f);
		}

		public void RotateLeftAnchor()
		{
			anchor.transform.Rotate (Vector3.up, -3f);		
		}

		#endregion*/
       
    }
}