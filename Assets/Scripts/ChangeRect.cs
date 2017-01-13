using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRect : MonoBehaviour {
	private GameObject gridLayout;
	private RectTransform rt;
	private int oldNumberOfItems;
	private bool didFirstIteration;

	void Start () {
		oldNumberOfItems = 0;
		didFirstIteration = false;

		gridLayout = GameObject.Find("Grid Layout");
		rt = gridLayout.GetComponent<RectTransform>();
	}
	
	void Update () {
		if (didFirstIteration) {
			CheckGridLayoutUpdate();
		} else {
			didFirstIteration = true;
			oldNumberOfItems = rt.childCount;
		}
	}

	public void CheckGridLayoutUpdate () {
		int currentNumberOfItems = rt.childCount;

		if (oldNumberOfItems != currentNumberOfItems) {
			rt.offsetMin += new Vector2 (rt.offsetMin.x, -105f);
			oldNumberOfItems = currentNumberOfItems;
		}
	}
}
