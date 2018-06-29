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

        private Vector3[] offsets;

        public Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            
            offsets = new Vector3[Anchors.Length];
            int i = 0;
            foreach(var anc in Anchors)
            {
                //offsets[i] = Anchors[i++] - ce
            }
            GUI.enabled = true; 
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
			Debug.Log (marker.AR_DetectedAnchorPos);
            if (marker != null)
            {                
                /*
                stage               stage                        stage
                    scene       ->      anchor      -> move ->      scene
                        anchor              scene                      anchor
                 */
                //set stage as child of ancchor
                var stageAnchor = marker.TargetAnchor;
                stageAnchor.SetParent(transform.parent);
                transform.SetParent(stageAnchor);
               
                var detectedPos = marker.AR_DetectedAnchorPos;
                //Vector3 deltaPos = detectedPos.position - stageAnchor.position;
                
                //move anchor to posion ddetected in AR, hence moving whole stage with it  
                stageAnchor.position += marker.deltaScenePos2RealPos;
                stageAnchor.rotation = detectedPos.rotation;
                
                //stageAnchor.SetPositionAndRotation(detectedPos.position, detectedPos.rotation);
                //revert back
                transform.SetParent(stageAnchor.parent);
                stageAnchor.SetParent(transform);
                
                //find delta               

				Debug.Log (Center.position);
				Debug.Log (Center.rotation);
            }
        }
        private void Update() {
            if(Input.touchCount > 0)
            {          

                //anim.SetBool("Visible", true);
            }
            else
            {
                //anim.SetBool("Visible", false);
            }  
            
            GUI.Box(new Rect(0,0, Screen.width, Screen.height / 4), "delta1" + Anchors[0].deltaScenePos2RealPos.magnitude);
            GUI.Box(new Rect(0,Screen.height / 4, Screen.width, Screen.height / 4), "delta2" + Anchors[1].deltaScenePos2RealPos.magnitude);          
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
