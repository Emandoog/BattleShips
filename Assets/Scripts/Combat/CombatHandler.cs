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
	//[SerializeField] GameHandler gameUIHandler;


	/// <summary>
	/// Amout of ships based on their size
	/// </summary>
	private int ship2 = 1;
	private int ship3 = 2;
	private int ship4 = 1;
	private int ship5 = 1;
	private Dictionary<Vector2, GameObject> tileMapToHit;
	private List< GameObject> tileMapToCurrentlyHuntIn = new List<GameObject>();
	private List< GameObject> tileMapToCurrentlyTargetIN = new List<GameObject>();
	private Dictionary<Vector2, GameObject> tileMapToIterate;
	private Dictionary<Vector2, GameObject> tileMapToIterate2;

	private bool isHunting = true;
	private bool isTargetting = false;
	private GameObject targetbeggiing; 
	/// </summary>


	private bool enemyHitSomething;




	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;

	}

	public void EnemyAIHuntWithParity() 
	{
		if (tileMapToIterate2 == null)
		{
			tileMapToIterate2 = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);
		}

		if (isHunting) 
		{
			Debug.Log("Currently In hunting Mode");
			if (ship2 != 0)
			{
				//create Hunt map with with spacing of 2 if we do not have one. 
				if (tileMapToCurrentlyHuntIn == null)
				{
					for (int i = 1; i < 99; i += 2)
					{
						var tileTocheck = gridSetUp.TileMap.ElementAt(i).Value;
						if (!tileTocheck.GetComponent<Tile>().isHitByEnemy)
						{
							Debug.Log("HuntMapCreated");
							tileMapToCurrentlyHuntIn.Add(tileTocheck);
						}


					}

				}
				

			}
			
			var tileToAttack = tileMapToCurrentlyHuntIn.ElementAt(Random.Range(0, tileMapToCurrentlyHuntIn.Count)).GetComponent<Tile>();
			if (tileToAttack.EnemyAttackTile())
			{
				isTargetting = true;
				AddSurroudingTilesToTargetMap(tileToAttack);
			
			}


			
		}
		if (isTargetting)
		{
			Debug.Log("Currently In Targeting Mode");

		}
		
		
		if (ship2 != 0)
		{ 
			

		///check everyother tile at random 
		/// if hit we hunt  till we run out of possible tiles 
		///
		
		
		
		
		}
	
	
	
	
	}


	private void AddSurroudingTilesToTargetMap( Tile tileToAttack)
	{
		List<Vector2> listOfTilesAround = new List<Vector2> { tileToAttack.tilePos + new Vector2(0, 1), tileToAttack.tilePos + new Vector2(0, -1), tileToAttack.tilePos + new Vector2(-1, 0), tileToAttack.tilePos + new Vector2(1, 0) };
		foreach (var tile in listOfTilesAround)
		{
			if (gridSetUp.IsInDictionary(tile))
			{
				tileMapToCurrentlyTargetIN.Add(gridSetUp.GetTileFromDic(tile));

			}

		}

	}

	/// <summary>
	/// Logick behind enemy turn in combat mode
	/// </summary>
	/// <returns></returns>
	IEnumerator EnemyTurn() 
	{
		yield return new WaitForSecondsRealtime(1);
		SetTableToPlayer();
		yield return new WaitForSecondsRealtime(1);
		Debug.Log("tries to attack");
		//attack random tile 
		if (tileMapToHit.Count > 0)
		{
			var temp = Random.Range(0, tileMapToHit.Count);
			var tileToAttack = tileMapToHit.ElementAt(temp).Value.GetComponent<Tile>();
			
			if (tileToAttack.EnemyAttackTile()) 
			{
				
				enemyHitSomething = true;
				
			}
			///Debug.Log("EnemyAttacked");

			tileMapToHit.Remove(tileMapToHit.ElementAt(temp).Key);

		}





		yield return new WaitForSecondsRealtime(2);
		SetTableToEnemy();
		
		if (!gameEnded)
		{
			gameUIHandler.GetComponent<GameUIHandler>().ChangeTurn("Player turn");
			isPlayerTurn = true;

		}
	}
	public void EnemyHitsTile()
	{
		if (tileMapToHit == null  ) 
		{
			tileMapToHit = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);

		}
		StartCoroutine (EnemyTurn());
		
		

	}
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
			isPlayerTurn = false;
			if (!gameEnded)
			{
				yield return new WaitForSecondsRealtime(1);
				gameUIHandler.GetComponent<GameUIHandler>().ChangeTurn("Enemy turn");
				EnemyHitsTile();

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
		Debug.Log("Enemy ship Down");
		if (enemyShipsLeft == 0)
		{
			gameEnded = true;
			gameHandler.GetComponent<GameHandler>().PlayerWon();

		}
	}
	public void EnemyShipHitLog()
	{
		Debug.Log("Enemy ship hit");
		SetUIText("You Attack this tile and hit enemy");

	}
	public void PlayerShipHitLog() 
	{
		Debug.Log("Player ship hit");
		SetUIText("Enemy has attacked this tile and hit you");
	}
	/// <summary>
	/// Takes away one player ship, if no player ships remain, shows defeat screen
	/// <smmary></smmary>
	public void PlayerShipDown(string shipName)
	{
		playerShipsLeft--;
		SetUIText("Player ship " + shipName + " ship is destroyed");
		Debug.Log("Player ship Down");
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
}
