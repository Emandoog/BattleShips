using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BuildingHandler : MonoBehaviour
{

	[SerializeField] private List<GameObject> shipList = new List<GameObject>();
	[SerializeField] private List<GameObject> tilesTakenByPlayer = new List<GameObject>();
	[SerializeField] private int shipIndex = 0;
	[SerializeField] private int shipIndexEnemy = 0;
	[SerializeField] private GameObject gameUIHandler;
	public bool showEnemyPostion = false;
	public bool playerShipsHidden = false;



	/// <summary>
	/// Tries to  Player ship on selected tile
	/// </summary>
	public void PlayerPlaceShip() 
	{

		GameObject tileToPlaceShipOn = GameHandler.instance.selectedTile;
		Tile tile = tileToPlaceShipOn.GetComponent<Tile>();
		Ship shipToPlace = shipList[shipIndex].GetComponent<Ship>();


			if (tileToPlaceShipOn != null)
			{
				if (!tileToPlaceShipOn.GetComponent<Tile>().isTakenByPlayer == false)
				{
					SetUIText("Tile is already taken");
					Debug.Log("Tile is already taken");
					return;

				}
				if (!CheckIfPlacementIsValidPlayer(shipToPlace.shipSize, shipToPlace.rotation, tileToPlaceShipOn))
				{
					return;

				}

				switch (shipToPlace.rotation)
				{
					case 0:

						shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x + shipToPlace.offset, 1.2f, tileToPlaceShipOn.transform.position.z );
						break;
					case 1:
							
						shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x , 1.2f, tileToPlaceShipOn.transform.position.z - shipToPlace.offset);
						break;
					case 2:
	
						shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x - shipToPlace.offset, 1.2f, tileToPlaceShipOn.transform.position.z);
						break;
					case 3:

						shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x , 1.2f, tileToPlaceShipOn.transform.position.z + shipToPlace.offset);
						break;


				}

				SetUIText("Ship placed");
				Debug.Log("Ship placed");

			}
		
	}

	public bool EnemyPlaceShip( GameObject tileToPlaceShipOn, int shipRotation) 
	{
		Ship shipToPlace = shipList[shipIndexEnemy].GetComponent<Ship>();
		if (!CheckIfPlacementIsValidEnemy(tileToPlaceShipOn, shipToPlace.shipSize, shipRotation)) 
		{
			return false;
		
		}

		shipIndexEnemy++;
		return true;
	}

	/// <summary>
	/// Checks if ship placment is valid, returns true if valid 
	/// </summary>
	/// <returns></returns>
	private bool CheckIfPlacementIsValidPlayer(int shipSize, int shipRotation, GameObject tileToPlaceShipOn, bool confirmPlacement = false )
	{
		bool result = true;
		//GameObject tileToPlaceShipOn = GameHandler.instance.SelectedTile;
		
		Tile tile = tileToPlaceShipOn.GetComponent<Tile>();
		Ship shipToPlace = shipList[shipIndex].GetComponent<Ship>();
		
		List<GameObject> tilesToActivate = new List<GameObject>();

		//check tiles based on ship rotation
		switch (shipToPlace.rotation)
		{
			
			case 0:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x + i, tile.tilePos.y);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						SetUIText("Wrong placement");
						Debug.Log("Wrong Placement");
						result = false;
						return result; 
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						SetUIText("Other tiles are already taken");
						Debug.Log("Other tiles are already taken");
						result = false;
						return result;

					}

					//If we are confirming placement, add the tile to the list, 
					if (confirmPlacement)
					{
						tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));
					}
				}
				// if everyting went good and we were confirming placement set the tiles as teken by player;
				if (confirmPlacement && result == true)
				{
					//add the first tile to the list
					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(new Vector2(tile.tilePos.x  , tile.tilePos.y)));

					ConfirmPlayerShip(tilesToActivate);
					
				}
				break;
			case 1:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x, tile.tilePos.y - i);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						SetUIText("Wrong placement");
						Debug.Log("WrongPlacement");
						return false;
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						SetUIText("Other tiles are already taken");
						Debug.Log("Other tiles are already taken");
						return false;
					}
					//If we are confirming placement, add the tile to the list, 
					if (confirmPlacement)
					{
						tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));
					}
				}
				// if everyting went good and we were confirming placement set the tiles as teken by player;
				if (confirmPlacement && result == true)
				{
					//add the first tile to the list
					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(new Vector2(tile.tilePos.x, tile.tilePos.y)));

					ConfirmPlayerShip(tilesToActivate);

				}
				break;
			case 2:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x - i, tile.tilePos.y);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						SetUIText("Wrong placement");
						Debug.Log("Wrong placement");
						return false;
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						SetUIText("Other tiles are already taken");
						Debug.Log("Other tiles are already taken");
						return false;
					}
					//If we are confirming placement, add the tile to the list, 
					if (confirmPlacement)
					{
						tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));
					}
				}
				// if everyting went good and we were confirming placement set the tiles as teken by player;
				if (confirmPlacement && result == true)
				{
					//add the first tile to the list
					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(new Vector2(tile.tilePos.x, tile.tilePos.y)));

					ConfirmPlayerShip(tilesToActivate);

				}
				break;
			case 3:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x, tile.tilePos.y + i);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						SetUIText("Wrong placement");
						Debug.Log("WrongPlacement");
						return false;
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						SetUIText("Other Tiles are already taken");
						Debug.Log("Other Tiles are already taken");
						return false;
					}
					//If we are confirming placement, add the tile to the list, 
					if (confirmPlacement)
					{
						tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));
					}
				}
				// if everyting went good and we were confirming placement set the tiles as teken by player;
				if (confirmPlacement && result == true)
				{
					//add the first tile to the list
					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(new Vector2(tile.tilePos.x, tile.tilePos.y)));

					ConfirmPlayerShip(tilesToActivate);
				
				}
				break;
			
		}
		
		return true;
	}

	/// <summary>
	/// rottes currently selected ship
	/// </summary>
	/// <param name="context"></param>
	public void RotateShip(InputAction.CallbackContext context) 
	{
		if (context.started) 
		{
			Ship shipToRotate = shipList[shipIndex].GetComponent<Ship>();
			shipToRotate.RotateShip();
		}
		

	}
	
	/// <summary>
	/// Confirms wether the ship was placed correctly and changes forcus to another ship if thery are any or stops building phase
	/// </summary>
	/// <param name="context"></param>
	public void ConfirmPlacment(InputAction.CallbackContext context) 
	{
		if (context.started) 
		{
			if (GameHandler.instance.selectedTile != null) 
			{
				GameObject tileToPlaceShipOn = GameHandler.instance.selectedTile;
				Tile tile = tileToPlaceShipOn.GetComponent<Tile>();
				Ship shipToPlace = shipList[shipIndex].GetComponent<Ship>();

				if (!tileToPlaceShipOn.GetComponent<Tile>().isTakenByPlayer == false)
				{
				
					Debug.Log("Tile is already taken");
					return;

				}
				if (!CheckIfPlacementIsValidPlayer(shipToPlace.shipSize, shipToPlace.rotation, tileToPlaceShipOn,true))
				{
					return;

				}
				SetUIText("Ship placed properly");
				Debug.Log("Placement Confirmed Correctly");
				if (shipIndex == shipList.Count -1) 
				{

					BuildingPhaseEnd();
					SetUIText("Building phase ended");
					Debug.Log("Building phase ended");
					GameHandler.instance.GetComponent<GameHandler>().StopBuildingPhase(); 
				
					return;
		
				//go to next phase
			
				}
				shipIndex++;
			}
		}

	}


	/// <summary>
	/// Sets tiles values as taken by player 
	/// </summary>
	/// <param name="tilesToActivate"> List of tiles to set </param>
	private void ConfirmPlayerShip(List<GameObject> tilesToActivate)
	{

		foreach (GameObject go in tilesToActivate)
		{
			Debug.Log("Tiles set as taken");
			go.GetComponent<Tile>().isTakenByPlayer = true;
			go.GetComponent<Tile>().playerShipStationed = shipList[shipIndex];


		}
	}
	/// <summary>
	/// Sets tiles values as taken by enemy
	/// </summary>
	/// <param name="tilesToActivate"> List of tiles to set </param>
	private void ConfirmEnemyShip(List<GameObject> tilesToActivate)
	{

		foreach (GameObject go in tilesToActivate)
		{
			Debug.Log("Enemy Tiles set as taken");
			if (showEnemyPostion)
			{
				RevealEnemyPosition(go);
			}
			
			go.GetComponent<Tile>().isTakenByEnemy = true;
			go.GetComponent<Tile>().enemyShipStationed =  shipList[shipIndexEnemy];

		}
	}
	private void BuildingPhaseEnd()
	{
		SetUIText("Building phase ended");
		Debug.Log("Building phase ended");
		GameHandler.instance.GetComponent<GameHandler>().StopBuildingPhase();
		
	}
	/// <summary>
	/// Cominicates with UI element and sets text in log based on the input
	/// </summary>
	/// <param name="text"> Text to show on screen</param>
	public void SetUIText(string text)
	{

		gameUIHandler.GetComponent<GameUIHandler>().SetBuildingLog(text);
	}

	public bool CheckIfPlacementIsValidEnemy(GameObject tileToPlaceEnemyShip,int shipSize, int shipRotation)
	{

	
		Tile  tile = tileToPlaceEnemyShip.GetComponent<Tile>();
		bool result = true;
		List<GameObject> tilesToActivate = new List<GameObject>();
	
		switch (shipRotation)
		{
			case 0:
				for (int i = 0; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x + i, tile.tilePos.y);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						
						Debug.Log("Enemy Wrong Placement");
						result = false;
						return result;
					}
					//check if other tile is  already taken by enemy
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByEnemy)
					{
						
						Debug.Log(" Enemy Other tiles are already taken");
						result = false;
						return result;

					}

					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));
					
				}
				// if everyting went good 
				if ( result == true)
				{
					
					ConfirmEnemyShip(tilesToActivate);

				}
				break;
			case 1:
				for (int i = 0; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x, tile.tilePos.y - i);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{


						Debug.Log("Enemy Wrong Placement");
						result = false;
						return result;
					}
					//check if other tile is  already taken by enemy
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByEnemy)
					{

						Debug.Log(" Enemy Other tiles are already taken");
						result = false;
						return result;

					}

					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));

				}
				// if everyting went good 
				if (result == true)
				{

					ConfirmEnemyShip(tilesToActivate);

				}
				break;
			case 2:
				for (int i = 0; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x - i, tile.tilePos.y);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{


						Debug.Log("Enemy Wrong Placement");
						result = false;
						return result;
					}
					//check if other tile is  already taken by enemy
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByEnemy)
					{

						Debug.Log(" Enemy Other tiles are already taken");
						result = false;
						return result;

					}

					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));

				}
				// if everyting went good 
				if (result == true)
				{
					
					ConfirmEnemyShip(tilesToActivate);

				}
				break;
			case 3:
				for (int i = 0; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x, tile.tilePos.y + i);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{


						Debug.Log("Enemy Wrong Placement");
						result = false;
						return result;
					}
					//check if other tile is  already taken by enemy
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByEnemy)
					{

						Debug.Log(" Enemy Other tiles are already taken");
						result = false;
						return result;

					}

					tilesToActivate.Add(GridSetUp.instance.GetTileFromDic(tileToCheck));

				}
				// if everyting went good 
				if (result == true)
				{
					
					ConfirmEnemyShip(tilesToActivate);

				}
				break;



		}
		return result;
	}


	/// <summary>
	/// Sets Enemy ship visibility on a tile
	/// </summary>
	/// <param name="tile"></param>
	private void RevealEnemyPosition(GameObject tile) 
	{
		tile.GetComponent<Tile>().SetEnemyTilesDebug();
	}

	/// <summary>
	/// Changes player Ship representation visibility 
	/// </summary>
	/// <param name="context"></param>
	public void SetPlayerShipVisibility(InputAction.CallbackContext context)
	{
		if(!playerShipsHidden)
		{
			foreach (var ship in shipList)
			{
				ship.GetComponent<Ship>().HideBody();

			}
			playerShipsHidden = true;
		}
		else
		{
			foreach (var ship in shipList)
			{
				ship.GetComponent<Ship>().ShowBody();

			}
			playerShipsHidden = false;
		}
		

	}

}
