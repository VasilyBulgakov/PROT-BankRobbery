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
        public Transform AR_DetectedAnchorPos;

        public GameObject Marker;

        public Vector3 deltaScenePos2RealPos{
            get{
                return delta;
            }
        }

        private Vector3 delta;

        private GameObject _myMarker;  

        // Use this for initialization
        void Start()
        {
            UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
            Debug.Log("image anchor events added");
            AR_DetectedAnchorPos = transform;
        }

        void AddImageAnchor(ARImageAnchor arImageAnchor)
        {
            Debug.Log("image anchor added");
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
				AR_DetectedAnchorPos.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				AR_DetectedAnchorPos.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

                delta = AR_DetectedAnchorPos.position - TargetAnchor.position;

				if (Marker!=null)
                {                    
                    _myMarker = Instantiate(Marker, AR_DetectedAnchorPos.position, AR_DetectedAnchorPos.rotation);                   
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

                AR_DetectedAnchorPos.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                AR_DetectedAnchorPos.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
                delta = AR_DetectedAnchorPos.position - TargetAnchor.position;

				if (update != null ) 
				{
					update (this);
					if (Marker!=null) {                        
						_myMarker.transform.SetPositionAndRotation(AR_DetectedAnchorPos.position, AR_DetectedAnchorPos.rotation);
						//StartCoroutine (HideMarker ());
					}

				}
            }

        }
		private IEnumerator HideMarker()
		{
			float timer = 0;
			float time = _timeUpdateMarker * 0.5f;
			while (timer < time) 
			{
				timer += Time.deltaTime;
				yield return null;

			}
			if (_myMarker!=null)
			{
				Destroy(_myMarker);
				_myMarker = null; 
			}
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