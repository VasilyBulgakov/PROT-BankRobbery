using System.Collections.Generic;
using UnityEngine;


namespace Tracking
{
	[RequireComponent(typeof(ParentObject))]
    public class TrackableWorld : MonoBehaviour
    {
        public Transform Center;

        public WallMarker[] Anchors;

 

        private void Start()
        {
            // #if UNITY_EDITOR
            // StartCoroutine(correctInEditor());
            // #endif
        } 

        System.Collections.IEnumerator correctInEditor(){
            int index  = 0;
            while(true)
            {
                index++;
                if(index >= Anchors.Length) index = 0;
                var a = Anchors[index];
                if(a)
                    CorrectWithAnchor(a);

                yield return new WaitForSeconds(.1f);
            }            
        }

        public void CorrectWithAnchor(WallMarker marker)
        {
			Debug.Log ("CorrectWithAnchor");
			Debug.Log (marker.Anchor);
            if (marker != null)
            {

                var targetAnchor = marker.getMarkerTr();
                var stageAnchor = marker.TargetAnchor;               
                stageAnchor.SetParent(transform.parent);
                transform.SetParent(stageAnchor);
                //why rotates on even corrections??????????????????????
                
                stageAnchor.SetPositionAndRotation(targetAnchor.position, targetAnchor.rotation);


                transform.SetParent(stageAnchor.parent);
                stageAnchor.SetParent(transform);

				Debug.Log (Center.position);
				Debug.Log (Center.rotation);

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
