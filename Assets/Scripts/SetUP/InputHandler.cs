using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

	[SerializeField] GameInputs gameInputs;
	[SerializeField] BuildingHandler buildingHandler;
	[SerializeField] GameHandler gameHandler;
	[SerializeField] EnemyBuildSetUP enemyBuild;
	private void Awake()
	{
		gameInputs = new GameInputs();


	}
	
	private void OnEnable()
	{
		gameInputs.PlayerMap.Enable();
		gameInputs.PlayerMap.LeftClick.started += gameHandler.OnTileCliked;
		gameInputs.PlayerMap.RotateShip.started += buildingHandler.RotateShip;
		gameInputs.PlayerMap.Confirm.started += buildingHandler.ConfirmPlacment;
		//gameInputs.PlayerMap.GenerateEnemyShip.started += enemyBuild.SetUpEnemyShips;


	}
	private void OnDisable()
	{
		gameInputs.PlayerMap.Disable();
		if (buildingHandler != null) 
		{
			//gameInputs.PlayerMap.LeftClick.started -= SelectAndBuildOnTile;
		}
	
	}
	public void DisableBuildInputs() 
	{

		gameInputs.PlayerMap.RotateShip.started -= buildingHandler.RotateShip;
		gameInputs.PlayerMap.Confirm.started -= buildingHandler.ConfirmPlacment;


	}
	public void EnableCombatInputs()
	{
		gameInputs.PlayerMap.TogglePlayerShipVisibility.started += buildingHandler.SetPlayerShipVisibility;

	}
	public void EndGame() 
	{
		
		gameInputs.PlayerMap.TogglePlayerShipVisibility.started -= buildingHandler.SetPlayerShipVisibility;
		gameInputs.PlayerMap.LeftClick.started -= gameHandler.OnTileCliked;

		gameInputs.PlayerMap.Disable();
	}


}
