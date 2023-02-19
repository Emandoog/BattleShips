using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{

	public bool isBuildingPhase = true;
	public static GameHandler instance;
	public int playerShipsLeft = 5;
	public int enemyShpipsLeft = 5;

	[SerializeField] private  BuildingHandler buildHandler;
	[SerializeField] private  InputHandler inputHandler;
	[SerializeField] private  GameUIHandler gameUIHandler;
	[SerializeField] private  EnemyBuildSetUP enemyBuildSetUP;
	[SerializeField] private  CombatHandler combatHandler;
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

	

	
	/// <summary>
	/// Does things based on the game phase, in building phase places ship on a tile, in combat phase, attacks a tile 
	/// </summary>
	/// <param name="context"></param>
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
			else
			{
				combatHandler.PlayerHitSelectedTile();
			}
			
		}




	}
	/// <summary>
	/// Stops the building phase and starts combat phase
	/// </summary>
	public void StopBuildingPhase()
	{
		isBuildingPhase= false;
		inputHandler.DisableBuildInputs();
		gameUIHandler.EndBuildPhaseUI();
		SetUpEnemyShips();
		inputHandler.EnableCombatInputs();
	}

	public void SetUpEnemyShips()
	{
		enemyBuildSetUP.SetUpEnemyShips();
	}

	public void PlayerWon()
	{
		inputHandler.EndGame();
		gameUIHandler.GetComponent<GameUIHandler>().OnGameEnd("You Won");

	}

	public void PlayerLost()
	{
		inputHandler.EndGame();
		gameUIHandler.GetComponent<GameUIHandler>().OnGameEnd("You Lost");

	}


}
