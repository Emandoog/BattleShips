using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
  
	public Vector2 tilePos = Vector2.zero;
	public bool isTakenByPlayer = false;
	public bool isTakenByEnemy = false;

	public bool isHitByPlayer = false;
	public bool isHitByEnemy = false;

	public GameObject playerShipStationed;
	public GameObject enemyShipStationed;

	[SerializeField] private Material offMaterial;
	[SerializeField] private Material hitMaterial;
	[SerializeField] private Material hitShipMaterial;
	private Material baseMaterial;
	
	
	private Color baseColor;
	//private Color hitcolor;
	//private Color hitShipaColor;
	private Color currentlSetColorBase;
	private Renderer rend;

	void Start()
    {
		rend = GetComponent<Renderer>();

		baseColor = rend.material.color;
		baseMaterial = rend.material;
		currentlSetColorBase = rend.material.color;
		//hitcolor = hitMaterial.color;
		//hitShipaColor = hitShipMaterial.color;

	}

   

	private void OnMouseEnter()
	{
		GameHandler.instance.hoveredOverTile = gameObject;

		rend.material.color = new Color(rend.material.color.r + 1, rend.material.color.g + 1, rend.material.color.b + 1);
	}
	private void OnMouseExit()
	{
		GameHandler.instance.hoveredOverTile = null;
		rend.material.color = currentlSetColorBase;
	}

	/// <summary>
	/// Sets tile material as a diffrent material to get checker effect 
	/// </summary>
	public void OffColor()
	{
		rend = GetComponent<Renderer>();
		rend.material = offMaterial;
		baseMaterial = rend.material;
		baseColor = rend.material.color;
	}
	/// <summary>
	/// Sets tile's  Vector2  position value 
	/// </summary>
	/// <param name="newTilePos"> Position of the tile in Vector2 </param>
	public void SetTilePos(Vector2 newTilePos) 
	{
		tilePos = newTilePos;
	}
	/// <summary>
	/// Sets tiles as diffrent color to see enemy positions 
	/// </summary>
	public void SetEnemyTilesDebug() 
	{
		rend.material.color = new Color(rend.material.color.r + 1, rend.material.color.g , rend.material.color.b);


	}

	/// <summary>
	/// Checks if Player has ship placed on this tile, if yes sends a signal to it to take damage, sets the tile as hit by enemy returns true if it enemy ship is stationed on tile
	/// </summary>
	public bool  EnemyAttackTile()
	{
		
		isHitByEnemy = true;
		if (playerShipStationed!= null )
		{
			playerShipStationed.GetComponent<Ship>().PlayerShipTakeDamage();
			SetPlayerBoard();
			return true;
		}
		SetPlayerBoard();
		return false;


	}
	/// <summary>
	/// Checks if enemy has ship placed on this tile, if yes sends a singnal to it to take damage, sets the tile as hit by player
	/// </summary>
	public bool PlayerAttackTile()
	{

		isHitByPlayer = true;
		if (enemyShipStationed != null)
		{
			enemyShipStationed.GetComponent<Ship>().EnemyShipTakeDamage();
			SetEnemyBoard();
			return true;
		}
		SetEnemyBoard();
		return false;

	}

	/// <summary>
	/// Sets tile based on hits on enemy ships
	/// </summary>
	public void SetEnemyBoard() 
	{
		rend.material.color = currentlSetColorBase;
		if (isHitByPlayer && isTakenByEnemy)
		{ 
			//rend = GetComponent<Renderer>();
			rend.material = hitShipMaterial;
			currentlSetColorBase = hitShipMaterial.color;


		}
		else if (isHitByPlayer && !isTakenByEnemy)
		{
			//rend = GetComponent<Renderer>();
			rend.material = hitMaterial;
			currentlSetColorBase = hitMaterial.color;

		}
		else
		{
			//rend = GetComponent<Renderer>();
			rend.material = baseMaterial;
			currentlSetColorBase = baseMaterial.color;
		}
	}
	/// <summary>
	/// Sets tile color based on hits on player ships
	/// </summary>
	public void SetPlayerBoard()
	{
		rend.material.color = currentlSetColorBase;
		if (isHitByEnemy && isTakenByPlayer)
		{
			//rend = GetComponent<Renderer>();
			rend.material = hitShipMaterial;
			currentlSetColorBase = hitShipMaterial.color;
		}
		else if (isHitByEnemy && !isTakenByPlayer)
		{
			//rend = GetComponent<Renderer>();
			rend.material = hitMaterial;
			currentlSetColorBase = hitMaterial.color;
		}
		else
		{
			//rend = GetComponent<Renderer>();
			rend.material = baseMaterial;
			currentlSetColorBase = baseMaterial.color;
		}
	}

}
