using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
  
	public Vector2 tilePos = Vector2.zero;
	public bool isTakenByPlayer = false;
	[SerializeField] private Material offColor;
	
	private Color baseColor;
	private Renderer rend;
	// Start is called before the first frame update
	void Start()
    {
		rend = GetComponent<Renderer>();
		baseColor = rend.material.color;

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnMouseEnter()
	{
		GameHandler.instance.hoveredOverTile = gameObject;
		rend.material.color = new Color(rend.material.color.r + 1, rend.material.color.g + 1, rend.material.color.b + 1);
	}
	private void OnMouseExit()
	{
		GameHandler.instance.hoveredOverTile = null;
		rend.material.color = baseColor;
	}

	public void OffColor()
	{
		rend = GetComponent<Renderer>();
		rend.material = offColor;
		baseColor = rend.material.color;
	}
	public void SetTilePos(Vector2 newTilePos) 
	{
		tilePos = newTilePos;
	}
}
