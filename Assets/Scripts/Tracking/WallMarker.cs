using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

namespace Tracking
{
    public class WallMarker : MonoBehaviour
    {
        public delegate void TrackingEvent(WallMarker self);
        public event TrackingEvent detected;
        public event TrackingEvent update;
        public event TrackingEvent detectionLost;

        [SerializeField]
        private ARReferenceImage referenceImage;

		[SerializeField]
		private float _timeUpdateMarker = 3f;

		private float _lastTimeUpdate;

        public Transform TargetAnchor;

        [HideInInspector]
        public Transform Anchor;

        public GameObject Marker;

        private GameObject _myMarker;  

        // Use this for initialization
        void Start()
        {
            UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
            
            Anchor = transform;
        }

        private void FixedUpdate() {
            // #if UNITY_EDITOR
            // Anchor.position = transform.position;
            // Anchor.rotation = transform.rotation;
            // #endif
        }

        public Transform getMarkerTr()
        {
            if(_myMarker)
                return _myMarker.transform;
            else
                return null;
        }

        void AddImageAnchor(ARImageAnchor arImageAnchor)
        {
            Debug.Log("image anchor added");
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
				transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);  

                            

				if (Marker!=null)
                {
                    _myMarker = Instantiate(Marker, Anchor.position, Anchor.rotation);
                    _myMarker.GetComponent<MarkerQuadScript>().setARImage(referenceImage);
                    StartCoroutine (HideMarker ());
                    
                }
                if (detected != null)
					detected(this);
				_lastTimeUpdate = Time.time;
            }
        }

        void UpdateImageAnchor(ARImageAnchor arImageAnchor)
        {
            Debug.Log("image anchor updated");
			if (arImageAnchor.referenceImageName == referenceImage.imageName && Time.time - _lastTimeUpdate > _timeUpdateMarker)
            {
				_lastTimeUpdate = Time.time;

                Anchor.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                Anchor.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

				if (update != null ) 
				{
					update (this);
					if (_myMarker!=null) {
						_myMarker.transform.SetPositionAndRotation(Anchor.position, Anchor.rotation);
						StartCoroutine (HideMarker ());
					}             

				}
            }

        }
		private IEnumerator HideMarker()
		{
            var m = _myMarker.GetComponentInChildren<Renderer>().material;
            m.SetColor("_Color", Color.white);
			float timer = 0;
			float time = _timeUpdateMarker;
			while (timer < time) 
			{
				timer += Time.deltaTime;

                m.SetColor("_Color", new Color( 1f,1f - timer/time,1f - timer/time, 0.7f ));
                
				yield return new WaitForSeconds(0.1f);
			}
			// if (_myMarker!=null)
			// {
			// 	Destroy(_myMarker);
			// 	_myMarker = null; 
			// }
		}

        void RemoveImageAnchor(ARImageAnchor arImageAnchor)
        {

            Debug.Log("image anchor removed");
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
				if (_myMarker!=null)
                {
                    Destroy(_myMarker);
                    _myMarker = null; 
                }

                if (detectionLost != null)
				    detectionLost(this);
            }
        }

        void OnDestroy()
        {
            UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

        }
    }
}