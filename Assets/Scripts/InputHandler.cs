using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

	[SerializeField] GameInputs gameInputs;
	[SerializeField] BuildingHandler buildingHandler;
	[SerializeField] GameHandler gameHandler;
	private void Awake()
	{
		gameInputs = new GameInputs();


	}
	private void Start()
	{
		
	}
	private void OnEnable()
	{
		gameInputs.PlayerMap.Enable();
		gameInputs.PlayerMap.LeftClick.started += gameHandler.GetComponent<GameHandler>().OnTileCliked;
		gameInputs.PlayerMap.RotateShip.started += buildingHandler.RotateShip;
		gameInputs.PlayerMap.Confirm.started += buildingHandler.ConfirmPlacment;


	}
	private void OnDisable()
	{
		gameInputs.PlayerMap.Disable();
		if (buildingHandler != null) 
		{
			//gameInputs.PlayerMap.LeftClick.started -= SelectAndBuildOnTile;
		}
	
	}

	private void SelectAndBuildOnTile(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			var temp = GameHandler.instance.GetComponent<GameHandler>().hoveredOverTile;
			if (GameHandler.instance.GetComponent<GameHandler>().hoveredOverTile == null )
			{
				Debug.Log("Mouse if not hovering over any tile");
				return;
			}

			GameHandler.instance.GetComponent<GameHandler>().selectedTile = temp;

		


		}
		
	
	
	
	}
}
