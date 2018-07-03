using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.iOS;
public class MoveToMarker : MonoBehaviour {

	[SerializeField]
	private ARReferenceImage referenceImage;


	 void Start()
        {
            UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
            
        }

        void AddImageAnchor(ARImageAnchor arImageAnchor)
        {
            Debug.Log("image anchor added");
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
				transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
                
            }
        }

        void UpdateImageAnchor(ARImageAnchor arImageAnchor)
        {            
			if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
                transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);                
            }
        }

        void RemoveImageAnchor(ARImageAnchor arImageAnchor)
        {            
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
				
            }
        }

        void OnDestroy()
        {
            UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

        }
    

}
