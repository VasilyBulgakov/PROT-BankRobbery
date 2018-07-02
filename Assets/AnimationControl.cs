using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour {

	public DoScan eventSource;

	public Material normal;
	public Material scanned;

	public Slider slider;
	public Button button;


	public bool isActive = false;
	public float scanTimeThreshold = 4.0f;
	public float curScanTime = 0;
	public float lastScanTime = 0;

	

	private void Start() {
		DoScan.scannedAllPOIs += OnAllScanned;
		GetComponent<Renderer>().material = normal;	
		button.onClick.AddListener(setActive);
		eventSource.gameObject.SetActive(false);

		slider.gameObject.SetActive(false);
	}

	private void setActive(){
		isActive = true;
		button.gameObject.SetActive(false);
		slider.gameObject.SetActive(true);
		eventSource.gameObject.SetActive(true);
		
	}


	private void OnAllScanned(GameObject POI_Parent)
	{
		if(!isActive) return;
		if(POI_Parent.name != transform.parent.name)
			return;
		
		if(curScanTime > scanTimeThreshold)	
		{
			enable();
		}		
		else
			curScanTime += Time.deltaTime;				

	}

	private void enable() {		
		GetComponent<Animator>().SetBool("Visible", true);	
		
		GetComponent<Renderer>().material = scanned;

	}
	private void disable() {		
		GetComponent<Animator>().SetBool("Visible", false);
		GetComponent<Renderer>().material = normal;			
	}
	private void FixedUpdate() {		
		slider.value = curScanTime/scanTimeThreshold;
	}

            
}
