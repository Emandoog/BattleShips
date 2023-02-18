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
	[SerializeField] private GameObject uIBuilding;



	private void Start()
	{
		
	}

	public void PlayerPlaceShip() 
	{
		 
			GameObject tileToPlaceShipOn = GameHandler.instance.selectedTile;
			Tile tile = tileToPlaceShipOn.GetComponent<Tile>();
			Ship shipToPlace = shipList[shipIndex].GetComponent<Ship>();


				if (tileToPlaceShipOn != null)
				{
					if (!tileToPlaceShipOn.GetComponent<Tile>().isTakenByPlayer == false)
					{
						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Tile is already taken");
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

							//gameObject.transform.position = new Vector3(gameObject.transform.position.x , gameObject.transform.position.y, gameObject.transform.position.z - offset);
							shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x + shipToPlace.offset, 1.2f, tileToPlaceShipOn.transform.position.z );
							break;
						case 1:
							//	gameObject.transform.position = new Vector3(gameObject.transform.position.x - offset, gameObject.transform.position.y, gameObject.transform.position.z + offset);
							shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x , 1.2f, tileToPlaceShipOn.transform.position.z - shipToPlace.offset);
							break;
						case 2:

							//	gameObject.transform.position = new Vector3(gameObject.transform.position.x + offset, gameObject.transform.position.y, gameObject.transform.position.z + offset);
							shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x - shipToPlace.offset, 1.2f, tileToPlaceShipOn.transform.position.z);
							break;
						case 3:

							//gameObject.transform.position = new Vector3(gameObject.transform.position.x + offset, gameObject.transform.position.y, gameObject.transform.position.z - offset);
							shipList[shipIndex].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x , 1.2f, tileToPlaceShipOn.transform.position.z + shipToPlace.offset);
							break;







					}

					//shipList[0].transform.position = new Vector3(tileToPlaceShipOn.transform.position.x + 0.5f, 1.2f, tileToPlaceShipOn.transform.position.z);
					uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Ship placed");
					Debug.Log("Ship placed");

				}
		
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

						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Wrong placement");
						Debug.Log("Wrong Placement");
						result = false;
						return result; 
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Other tiles are already taken");
						Debug.Log("Other Tiles are already taken");
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
					foreach (GameObject go in tilesToActivate) 
					{
						Debug.Log("Tiles set as taken"); 
						go.GetComponent<Tile>().isTakenByPlayer= true; 
					
					}
					
				}
				break;
			case 1:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x, tile.tilePos.y - i);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Wrong placement");
						Debug.Log("WrongPlacement");
						return false;
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Other tiles are already taken");
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
					foreach (GameObject go in tilesToActivate)
					{
						Debug.Log("Tiles set as taken");
						go.GetComponent<Tile>().isTakenByPlayer = true;

					}

				}
				break;
			case 2:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x - i, tile.tilePos.y);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Wrong placement");
						Debug.Log("WrongPlacement");
						return false;
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Other tiles are already taken");
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
					foreach (GameObject go in tilesToActivate)
					{
					
						Debug.Log("Tiles set as taken");
						go.GetComponent<Tile>().isTakenByPlayer = true;

					}

				}
				break;
			case 3:
				for (int i = 1; i < shipSize; i++)
				{
					Vector2 tileToCheck = new Vector2(tile.tilePos.x, tile.tilePos.y + i);

					//check if other tiles exists
					if (!GridSetUp.instance.IsInDictionary(tileToCheck))
					{

						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Wrong placement");
						Debug.Log("WrongPlacement");
						return false;
					}
					//check if other tile is  already taken by player
					if (GridSetUp.instance.GetTileFromDic(tileToCheck).GetComponent<Tile>().isTakenByPlayer)
					{
						uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Other Tiles are already taken");
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
					foreach (GameObject go in tilesToActivate)
					{
						Debug.Log("Tiles set as taken");
						go.GetComponent<Tile>().isTakenByPlayer = true;

					}

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
			uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Ship placed properly");
			Debug.Log("Placement Confirmed Correctly");
			if (shipIndex == shipList.Count -1) 
			{
				uIBuilding.GetComponent<UIBuilding>().SetBuildingLog("Building phase ended");
				Debug.Log("Building phase ended");
				return;
			//stop bulding phase
			//go to next phase
			
			}
			shipIndex++;
		}

		
	}
}
