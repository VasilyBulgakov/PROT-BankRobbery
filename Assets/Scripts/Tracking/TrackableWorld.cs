using System.Collections.Generic;
using UnityEngine;
//using MultiplayerClient;

namespace Tracking
{
	[RequireComponent(typeof(ParentObject))]
    public class TrackableWorld : MonoBehaviour
    {
        public Transform Center;

        public WallMarker[] Anchors;


        private void Start()
        {
        }
        private void FixedUpdate() {
           #if UNITY_EDITOR 
            foreach(WallMarker anchor in Anchors)
            {
                CorrectWithAnchor(anchor);
            }
            #endif
        }

        public void CorrectWithAnchor(WallMarker marker)
        {
			Debug.Log ("CorrectWithAnchor");
			Debug.Log (marker.Anchor);
            if (marker != null)
            {

                var targetAnchor = marker.Anchor;
                var stageAnchor = marker.TargetAnchor;
                stageAnchor.SetParent(transform.parent);
                transform.SetParent(stageAnchor);

                stageAnchor.SetPositionAndRotation(targetAnchor.position, targetAnchor.rotation);

                transform.SetParent(stageAnchor.parent);
                stageAnchor.SetParent(transform);

				Debug.Log (Center.position);
				Debug.Log (Center.rotation);

                //
                //var player = FindObjectOfType<Controller>();

                // var corrPos = transform.InverseTransformPoint()
                //player.SetCorrection(-Center.position, Quaternion.Inverse(Center.rotation));
            }
        }

        private void OnDrawGizmos()
        {
           
        }

        private void DrawCube(Vector3 center)
        {
            var color = Color.magenta;
            // Debug.DrawLine(start, center, color, 5f);

            var boxSize = 0.01f;
            Debug.DrawLine(center + new Vector3(boxSize, boxSize, boxSize), center + new Vector3(-boxSize, boxSize, boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(boxSize, boxSize, boxSize), center + new Vector3(boxSize, -boxSize, boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(boxSize, boxSize, boxSize), center + new Vector3(boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(-boxSize, -boxSize, -boxSize), center + new Vector3(boxSize, -boxSize, -boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(-boxSize, -boxSize, -boxSize), center + new Vector3(-boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(-boxSize, -boxSize, -boxSize), center + new Vector3(-boxSize, -boxSize, boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(boxSize, boxSize, -boxSize), center + new Vector3(boxSize, -boxSize, -boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(boxSize, -boxSize, -boxSize), center + new Vector3(boxSize, -boxSize, boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(boxSize, -boxSize, boxSize), center + new Vector3(-boxSize, -boxSize, boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(-boxSize, -boxSize, boxSize), center + new Vector3(-boxSize, boxSize, boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(-boxSize, boxSize, boxSize), center + new Vector3(-boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(center + new Vector3(-boxSize, boxSize, -boxSize), center + new Vector3(boxSize, boxSize, -boxSize), color, 5f);

        }
    }
}
