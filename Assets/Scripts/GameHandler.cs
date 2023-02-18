using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{

	public bool isBuildingPhase = true;
	public static GameHandler instance;
	[SerializeField] private  BuildingHandler buildHandler;
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;

	}
	public GameObject hoveredOverTile;
	public GameObject selectedTile;

	public void OnTileCliked(InputAction.CallbackContext context)
	{
		
		if (context.started) 
		{
		
			if (hoveredOverTile == null)
			{
				Debug.Log("No tile selected");
				return;
				
			}
			selectedTile = hoveredOverTile;
			if (isBuildingPhase)
			{
				buildHandler.PlayerPlaceShip();



			}
		}




	}
}
