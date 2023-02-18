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
	public static CombatHandler instance;
	[SerializeField] GameUIHandler gameUIHandler;
	[SerializeField] GameHandler gameHandler;
	//[SerializeField] GameHandler gameUIHandler;

	private Dictionary<Vector2, GameObject> tileMapToHit;
	private Dictionary<Vector2, GameObject> tileMapToIterate;
	

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;

	}
	

	private void Start()
	{
		//tileMapToHit = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);
	}
	IEnumerator EnemyTurn() 
	{
		yield return new WaitForSecondsRealtime(1);
		SetTableToPlayer();
		yield return new WaitForSecondsRealtime(1);
		Debug.Log("tries to attack");
		Debug.Log("possible moves left :" + tileMapToHit.Count);
		if (tileMapToHit.Count > 0)
		{
			var temp = Random.Range(0, tileMapToHit.Count);
			var tileToAttack = tileMapToHit.ElementAt(temp).Value.GetComponent<Tile>();
			tileToAttack.EnemyAttackTile();
			Debug.Log("EnemyAttacked");

			tileMapToHit.Remove(tileMapToHit.ElementAt(temp).Key);

		}
		yield return new WaitForSecondsRealtime(2);

		SetTableToEnemy();
		gameUIHandler.GetComponent<GameUIHandler>().ChangeTurn("Player turn");
		isPlayerTurn = true;
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
			

			
			yield return new WaitForSecondsRealtime(1);
			gameUIHandler.GetComponent<GameUIHandler>().ChangeTurn("Enemy turn");
			EnemyHitsTile();
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
}
