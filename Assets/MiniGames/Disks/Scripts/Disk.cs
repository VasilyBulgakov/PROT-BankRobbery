using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiskGame
{	
	[System.Serializable]
	public class ItemAligned : UnityEngine.Events.UnityEvent<GameObject>
	{

	}

	public class Disk : MonoBehaviour {

		
		Color colorAligned = Color.green;
		Color colorNotAligned = Color.red;  
		
		public ItemAligned onAlign;

		public GameObject item;		
		[Range(0, 360)]
		public float winAngle;


		private bool aligned = false;
		public bool isAligned{
			get{ return aligned; }
		}

		public void rotateItem(float angle, float precision)
		{			
			item.transform.Rotate(Vector3.up, angle);


			if( !aligned && checkAlign(precision) )
				onAlign.Invoke(item);
			else
				checkAlign(precision);
			
			if(aligned)
				transform.Find("Cylinder").GetComponent<Renderer>().material.SetColor("_Color", colorAligned);
			else
				transform.Find("Cylinder").GetComponent<Renderer>().material.SetColor("_Color", colorNotAligned);
		}

		private bool checkAlign(float precision)
		{				
			aligned = Mathf.Abs( item.transform.eulerAngles.y - winAngle ) < precision;
			return aligned;	
		}
	}
}
