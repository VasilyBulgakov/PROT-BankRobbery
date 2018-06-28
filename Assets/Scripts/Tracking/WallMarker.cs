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
            Debug.Log("image anchor events added");
            Anchor = transform;
        }

        void AddImageAnchor(ARImageAnchor arImageAnchor)
        {
            Debug.Log("image anchor added");
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
				Anchor.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				Anchor.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

                 Debug.Log("ROT: x:" + Anchor.eulerAngles.x + "y:" + Anchor.eulerAngles.y + "x:" + Anchor.eulerAngles.z);
				if (Marker!=null)
                {                    
                    _myMarker = Instantiate(Marker, Anchor.position, Anchor.rotation);
                    _myMarker.transform.Rotate(90,0,0);
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
					if (Marker!=null) {
                        _myMarker.transform.Rotate(90,0,0);
						_myMarker.transform.SetPositionAndRotation(Anchor.position, Anchor.rotation);
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