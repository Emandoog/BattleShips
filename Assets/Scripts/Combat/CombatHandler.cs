using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatHandler : MonoBehaviour
{

	public int enemyShipsLeft = 5;
	public int playerShipsLeft = 5;
	public bool isPlayerTurn = true;
	private bool gameEnded = false;
	public static CombatHandler instance;
	[SerializeField] GameUIHandler gameUIHandler;
	[SerializeField] GameHandler gameHandler;
	[SerializeField] GridSetUp gridSetUp;
	[SerializeField] float waitTime = 1;
	/// <summary>
	/// Amout of ships based on their size
	/// </summary>
	[SerializeField] private int ship2 = 1;
	[SerializeField] private int ship3 = 2;
	[SerializeField] private int ship4 = 1;
	[SerializeField] private int ship5 = 1;
	private Dictionary<Vector2, GameObject> tileMapToHit;
	private List<GameObject> tileMapToCurrentlyHuntIn = new List<GameObject>();
	private List<GameObject> tileMapToCurrentlyTargetIN = new List<GameObject>();
	private Dictionary<Vector2, GameObject> tileMapToIterate;
	private Dictionary<Vector2, GameObject> tileMapToIterate2;

	[SerializeField] private bool isHunting = true;
	[SerializeField] private bool isTargetting = false;
	





	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;

	}
	/// <summary>
	/// Hunt and targer algorithm for battle ship Game
	/// </summary>
	public void EnemyAIHunt()
	{
		if (ship2 == 0 && ship3 == 0 && ship4 == 0 && ship5 == 0)
		{
			
			return;
		}
		if (tileMapToIterate2 == null)
		{
			tileMapToIterate2 = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);

			for (int i = 0; i < tileMapToIterate2.Count; i++)
			{
				var tileTocheck = gridSetUp.TileMap.ElementAt(i).Value;
				tileMapToCurrentlyHuntIn.Add(tileTocheck);

			}
			
		}

		if (isHunting)
		{
			//Debug.Log("hunting");
			// proceed to attack random  tile from map of possible hits
			var buffer = Random.Range(0, tileMapToCurrentlyHuntIn.Count);
			var tileToAttack = tileMapToCurrentlyHuntIn.ElementAt(buffer).GetComponent<Tile>();
			if (tileToAttack.EnemyAttackTile())
			{
				//stop hunting and start targetting if a ship is hit 
				isTargetting = true;
				isHunting = false;
				//add surrouding tiles to target map 
				AddSurroudingTilesToTargetMap(tileToAttack);

			}
			//remove element that we hit from hunt list 
			tileMapToCurrentlyHuntIn.RemoveAt(buffer);

			//end turn somewhere here
			//EnemyAIHunt();
			
			StartCoroutine(PlayerTurn());
			return;
		}
		if (isTargetting)
		{

			//Debug.Log("targetting");
			//Debug.Log(tileMapToCurrentlyTargetIN);

			if (tileMapToCurrentlyTargetIN.Count> 0)
			{
				//Debug.Log("Currently In Targeting Mode");
				var buffer = Random.Range(0, tileMapToCurrentlyTargetIN.Count);
				var tileToAttack = tileMapToCurrentlyTargetIN.ElementAt(buffer).GetComponent<Tile>();

				if (tileToAttack.EnemyAttackTile())
				{
					//Debug.Log(tileToAttack.tilePos);
					//Debug.Log("hit enemy, trying to add tiles to the list");
					//if we hit a ship add next all surrouding tiles to target map
					AddSurroudingTilesToTargetMap(tileToAttack);
				}

				//remove this tile from possible targets 

			//	Debug.Log("hit enemy, removing item that was hit from the list");
				tileMapToCurrentlyTargetIN.RemoveAt(buffer);
				//tileMapToCurrentlyHuntIn.RemoveAt(buffer);
				
				//Debug.Log(" Done targetting this tile ");
				Debug.Log(tileMapToCurrentlyTargetIN);
			}
			//check if we ran out of targets
			if (tileMapToCurrentlyTargetIN.Count == 0)
			{
				isTargetting = false;
				isHunting = true;
				
			}
			//we stop enemy turn here 

			StartCoroutine(PlayerTurn());
			return;
		}




	}


	/// <summary>
	/// Checks if a given tile has surrouding tiles and addes them to the target list if they were not hit by enemy before
	/// </summary>
	/// <param name="tileToAttack">Tile to check</param>
	private void AddSurroudingTilesToTargetMap(Tile tileToAttack)
	{
		//Debug.Log("trying to add more entries");
		List<Vector2> listOfTilesAround = new List<Vector2> { tileToAttack.tilePos + new Vector2(0, 1), tileToAttack.tilePos + new Vector2(0, -1), tileToAttack.tilePos + new Vector2(-1, 0), tileToAttack.tilePos + new Vector2(1, 0) };
		foreach (var tile in listOfTilesAround)
		{
			//check if its a valid tile
			if (gridSetUp.IsInDictionary(tile))
			{   //check if tile already been hit or not 
				//Debug.Log("first check passed");
				//check if tile is not alrteayd on the list 
				if (!tileMapToCurrentlyTargetIN.Contains(gridSetUp.GetTileFromDic(tile)))
				{
					if (gridSetUp.GetTileFromDic(tile).GetComponent<Tile>().isHitByEnemy == false)
					{
						//Debug.Log(gridSetUp.GetTileFromDic(tile).GetComponent<Tile>().isHitByEnemy);
						//Debug.Log("adding one new entry to target map");
						//Debug.Log("Added To the list :" + tile);
						tileMapToCurrentlyTargetIN.Add(gridSetUp.GetTileFromDic(tile));
					}

				}
				else
				{
					//Debug.Log("Tile is alreayd in the list ");
				}
				


			}

		}

	}

	/// <summary>
	/// Logick behind enemy turn in combat mode
	/// </summary>
	/// <returns></returns>
	IEnumerator EnemyTurn()
	{
		yield return new WaitForSecondsRealtime(waitTime);
		SetTableToPlayer();
		yield return new WaitForSecondsRealtime(waitTime);
		//Debug.Log("tries to attack");

		EnemyAIHunt();


		
		//attack random tile randomtiles 
		//if (tileMapToHit.Count > 0)
		//{
		//	var temp = Random.Range(0, tileMapToHit.Count);
		//	var tileToAttack = tileMapToHit.ElementAt(temp).Value.GetComponent<Tile>();

		//	if (tileToAttack.EnemyAttackTile())
		//	{

		//		enemyHitSomething = true;

		//	}
		//	///Debug.Log("EnemyAttacked");

		//	tileMapToHit.Remove(tileMapToHit.ElementAt(temp).Key);

		//}

		yield return new WaitForSecondsRealtime(waitTime*2);
		SetTableToEnemy();

		if (!gameEnded)
		{
			//ends enemy turn
			gameUIHandler.GetComponent<GameUIHandler>().ChangeTurn("Player turn");
			isPlayerTurn = true;

		}
	}
	//public void EnemyHitsTile()
	//{
	//	if (tileMapToHit == null)
	//	{
	//		tileMapToHit = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);

	//	}

	//	StartCoroutine(EnemyTurn());



	//}
	/// <summary>
	/// This changes borad to represent the enemyboard and and start  after a delay enemy turn . 
	/// </summary>
	/// <returns></returns>
	IEnumerator PlayerTurn()
	{

		if (isPlayerTurn)
		{
			SetTableToEnemy();
			var tileToAttack = GameHandler.instance.selectedTile.GetComponent<Tile>();
			if (tileToAttack.isHitByPlayer)
			{
				SetUIText("This tile has been already hit by you ");
				yield break;


			}
			else
			{

				if (!tileToAttack.PlayerAttackTile())
				{
					SetUIText("You attack this tile and miss");

				}

			}
			//this ends player turn 
			isPlayerTurn = false;
			
			if (!gameEnded)
			{
				yield return new WaitForSecondsRealtime(waitTime);
				//this is where enemy turn starts 
				gameUIHandler.GetComponent<GameUIHandler>().ChangeTurn("Enemy turn");
				//EnemyHitsTile();
				StartCoroutine(EnemyTurn());

			}


		}

	}
	/// <summary>
	/// 
	/// Attepts to hit selected tile as a player 
	/// </summary>
	public void PlayerHitSelectedTile()
	{

		StartCoroutine(PlayerTurn());


	}
	/// <summary>
	/// Takes away one enemy ship, if no enemy ships remian, shows victory screen
	/// </summary>
	public void EnemyShipShowDown(string shipName)
	{
		enemyShipsLeft--;
		SetUIText("Enemy " + shipName + " ship is destroyed");
		//Debug.Log("Enemy ship Down");

		if (enemyShipsLeft == 0)
		{
			gameEnded = true;
			gameHandler.GetComponent<GameHandler>().PlayerWon();

		}
	}
	public void EnemyShipHitLog()
	{
		//Debug.Log("Enemy ship hit");
		SetUIText("You attack this tile and hit enemy");

	}
	public void PlayerShipHitLog()
	{
		//Debug.Log("Player ship hit");
		SetUIText("Enemy has attacked this tile and hit you");
	}
	/// <summary>
	/// Takes away one player ship, if no player ships remain, shows defeat screen
	/// <smmary></smmary>
	public void PlayerShipDown(string shipName)
	{
		playerShipsLeft--;
		SetUIText("Player ship " + shipName + " ship is destroyed");
		//Debug.Log("Player ship Down");
		if (playerShipsLeft == 0)
		{
			gameEnded = true;
			gameHandler.GetComponent<GameHandler>().PlayerLost();

		}
	}
	public void SetUIText(string text)
	{

		gameUIHandler.GetComponent<GameUIHandler>().SetBuildingLog(text);
	}


	/// <summary>
	/// Sets tiles to represent player board 
	/// </summary>
	public void SetTableToPlayer()
	{
		if (tileMapToIterate == null)
		{
			tileMapToIterate = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);

		}
		foreach (var tile in tileMapToIterate)
		{

			tile.Value.GetComponent<Tile>().SetPlayerBoard();


		}
	}
	/// <summary>
	/// Sets tiles to represent enemy board 
	/// </summary>
	public void SetTableToEnemy()
	{
		if (tileMapToIterate == null)
		{
			tileMapToIterate = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);

		}
		foreach (var tile in tileMapToIterate)
		{

			tile.Value.GetComponent<Tile>().SetEnemyBoard();


		}
	}

	public void ShipDown(int shipIdex)
	{
		//Debug.Log("Ship with index is down: " + shipIdex);
		switch (shipIdex)
		{
			case 2:
				ship2--;
				//tileMapToCurrentlyHuntIn.Clear();

				break;
			case 3:
				ship3--;
				break;
			case 4:

				ship4--;
				//tileMapToCurrentlyHuntIn.Clear();
				break;
			case 5:
				ship5--;
				//tileMapToCurrentlyHuntIn.Clear();
				break;
		}
		//there are 2 ships like this so it wont work like the other ones, we want to clear the hunt list only if there are no more ships of this type
		//if (ship3 == 0)
		//{
		//	tileMapToCurrentlyHuntIn.Clear();
		//}


		//}
		//List<T> GetRandomElementFromList<T>(List<T> inputList)
		//{
		//	var temp = inputList.Count;
		//	List<T> outputList = new List<T>();
		//	for (int i = 0; i < temp; i++)
		//	{

		//		int index = Random.Range(0, inputList.Count);
		//		outputList.Add(inputList[index]);

		//		inputList.RemoveAt(index);

		//	}

		//	return outputList;

		//}


		//public void EnemyAIHuntWithParity()
		//{
		//	if (ship2 == 0 && ship3 == 0)
		//	{
		//		Debug.Log("All Moves done xd");
		//		return;
		//	}
		//	if (tileMapToIterate2 == null)
		//	{
		//		tileMapToIterate2 = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);
		//	}

		//	if (isHunting)
		//	{
		//		Debug.Log("Currently In hunting Mode");
		//		if (ship2 != 0)
		//		{
		//			//create Hunt map with with spacing of 2 if we do not have one. 
		//			if (tileMapToCurrentlyHuntIn.Count == 0)
		//			{
		//				bool nextLine = false;
		//				int x = 0;
		//				int bufferRow = 0;
		//				Debug.Log("Currently creating new huntmap with spacing 2 ");
		//				for (int i = 0; i < 99; i = i + 2)
		//				{
		//					var tileTocheck = gridSetUp.TileMap.ElementAt(i).Value;
		//					int currentRow = (int)tileTocheck.GetComponent<Tile>().tilePos.x;
		//					//this need an offset every time we go to the other line of gridmap
		//					if (currentRow != bufferRow)
		//					{
		//						i++;
		//						tileTocheck = gridSetUp.TileMap.ElementAt(i).Value;
		//					}
		//					Debug.Log(i);

		//					//only add tiles with proper parity that were not hit already 
		//					if (!tileTocheck.GetComponent<Tile>().isHitByEnemy)
		//					{


		//						tileMapToCurrentlyHuntIn.Add(tileTocheck);
		//					}
		//					bufferRow = currentRow;

		//				}
		//				Debug.Log("Hunt Map Created");
		//				Debug.Log("Hunt map has: " + tileMapToCurrentlyHuntIn.Count);
		//			}


		//		}
		//		else if (ship3 != 0)
		//		{
		//			//create Hunt map with with spacing of 3 if we do not have one. 
		//			if (tileMapToCurrentlyHuntIn.Count == 0)
		//			{
		//				Debug.Log("Currently creating new huntmap with spacing 3 ");
		//				for (int i = 2; i < 99; i += 3)
		//				{
		//					var tileTocheck = gridSetUp.TileMap.ElementAt(i).Value;
		//					//only add tiles with proper parity that were not hit already 
		//					if (!tileTocheck.GetComponent<Tile>().isHitByEnemy)
		//					{


		//						tileMapToCurrentlyHuntIn.Add(tileTocheck);
		//					}


		//				}
		//				Debug.Log("Hunt Map Created");
		//				Debug.Log("Hunt map has: " + tileMapToCurrentlyHuntIn.Count);
		//			}


		//		}
		//		Debug.Log("Attacking a random tile ");

		//		// proceed to attack random  tile from the hunt map 
		//		var buffer = Random.Range(0, tileMapToCurrentlyHuntIn.Count);
		//		var tileToAttack = tileMapToCurrentlyHuntIn.ElementAt(buffer).GetComponent<Tile>();
		//		if (tileToAttack.EnemyAttackTile())
		//		{
		//			//stop hunting and start targetting if a ship is hit 
		//			isTargetting = true;
		//			isHunting = false;
		//			//add surrouding tiles to target map 
		//			AddSurroudingTilesToTargetMap(tileToAttack);

		//		}
		//		//remove element that we hit from hunt list 
		//		tileMapToCurrentlyHuntIn.RemoveAt(buffer);

		//		//end turn somewhere here
		//		EnemyAIHuntWithParity();
		//		//StartCoroutine(PlayerTurn());


		//	}
		//	if (isTargetting)
		//	{
		//		if (tileMapToCurrentlyTargetIN.Count == 0)
		//		{


		//			return;
		//		}

		//		Debug.Log("Currently In Targeting Mode");
		//		var buffer = Random.Range(0, tileMapToCurrentlyTargetIN.Count);
		//		var tileToAttack = tileMapToCurrentlyTargetIN.ElementAt(buffer).GetComponent<Tile>();

		//		if (tileToAttack.EnemyAttackTile())
		//		{


		//			//if we hit a ship add next all surroudingtiles to it
		//			AddSurroudingTilesToTargetMap(tileToAttack);
		//		}
		//		//remove this tile from possible targets 
		//		tileMapToCurrentlyTargetIN.RemoveAt(buffer);

		//		//check if we ran out of targets 
		//		if (tileMapToCurrentlyTargetIN.Count == 0)
		//		{
		//			isTargetting = false;
		//			isHunting = true;
		//			//return;
		//		}
		//		//we stop enemy turn here 
		//		EnemyAIHuntWithParity();
		//		//StartCoroutine(PlayerTurn());

		//	}


		//	if (ship2 != 0)
		//	{


		//		///check everyother tile at random 
		//		/// if hit we hunt  till we run out of possible tiles 
		//		///




		//	}




		//}
	}
}
