﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using MultiplayerClient;





namespace Tracking
{
	[RequireComponent(typeof(ParentObject))]
    public class TrackableWorld : MonoBehaviour
    {
        public UnityEvent OnFirstCorrection;
        public UnityEvent OnCorrection;
        
        private bool firstCorrection = true;

        public Transform Center;

        public WallMarker[] Anchors;

        private Vector3[] offsets;

        public Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            if (OnCorrection == null)
                OnCorrection = new UnityEvent();
            if (OnFirstCorrection == null)
                OnFirstCorrection = new UnityEvent();

            
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
			
            if (marker != null)
            {     
                Debug.Log ("CorrectWithAnchor");
			    Debug.Log (marker.AR_DetectedAnchorPos);           
                /*
                stage               stage                        stage
                    scene       ->      anchor      -> move ->      scene
                        anchor              scene                      anchor
                 */
                //set stage as child of ancchor
                var stageAnchor = marker.TargetAnchor;
                stageAnchor.SetParent(transform.parent);
                transform.SetParent(stageAnchor);

                //move anchor to posion ddetected in AR, hence moving whole stage with it  
                stageAnchor.position += calcDeltaUsingAllAnchors(marker);
                stageAnchor.rotation = marker.AR_DetectedAnchorPos.rotation;
                
                //stageAnchor.SetPositionAndRotation(detectedPos.position, detectedPos.rotation);
                //revert back
                transform.SetParent(stageAnchor.parent);
                stageAnchor.SetParent(transform);
                
                if(firstCorrection){
                    firstCorrection = false; 
                    OnFirstCorrection.Invoke(); 
                }
                else {                           
                    OnCorrection.Invoke();  
                }
                
                foreach(var cmp in GetComponentsInChildren<BoxCollider>())
                {
                    cmp.center = cmp.center + new Vector3(0,0,0.001f);
                    cmp.center = cmp.center - new Vector3(0,0,0.001f);
                }

				// Debug.Log (Center.position);
				// Debug.Log (Center.rotation);
               
            }
            
        }

        private Vector3 calcDeltaUsingAllAnchors(WallMarker marker)
        {
            //TODO: implement           
            
            return marker.deltaScenePos2RealPos;
        }


        private void Update() 
        {
            
        }
        private void OnGUI() {
            // var style = new GUIStyle();
            // style.fontSize = 14;
            // GUI.Box(new Rect(0,0, Screen.width, 40), "delta1: " + Anchors[0].deltaScenePos2RealPos.magnitude);
            // GUI.Box(new Rect(0,100, Screen.width, 40), "delta2: " + Anchors[1].deltaScenePos2RealPos.magnitude);          
        
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
