using UnityEngine;
using System.Collections;

public class CanonController : MonoBehaviour {

	// Use this for initialization
	public GameObject canonLeft;
	public float opacida;

	void Start () {
		opacida = 0.7f;
		//canonLeft = GameObject.FindWithTag ("CanonLeft");
		GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1,opacida);
		//Color tmp_color = new Color ();
		//tmp_color.a = 0.5f;

		//canonLeft.GetComponent<Material> ().color = tmp_color;
		//canonLeft.GetComponent<Renderer>()
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
