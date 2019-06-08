using UnityEngine;
using System.Collections;

public class RotateItem : MonoBehaviour {

	public float Xspeed,Yspeed = -100f,Zspeed;
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Time.deltaTime*Xspeed, Time.deltaTime*Yspeed, Time.deltaTime*Zspeed, Space.World);
	}
}
