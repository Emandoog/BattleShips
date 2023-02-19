using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    public int rotation = 0;
    public int shipSize = 2;
    public float offset = 0.5f;
    public  int playerHp ;
	public int enemyHp;

	[SerializeField] GameObject body;
	private CombatHandler combatHandler;
	void Start()
    {
		combatHandler = CombatHandler.instance.GetComponent<CombatHandler>();
		playerHp = shipSize;
        enemyHp = shipSize;
	}

   
	/// <summary>
	/// Rotates active player ship 
	/// </summary>
    public void RotateShip()
    {

     switch (rotation)
     {
            case 0:
                rotation = 1;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - offset, gameObject.transform.position.y, gameObject.transform.position.z - offset);
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
			case 1:
				rotation = 2;
				gameObject.transform.position = new Vector3(gameObject.transform.position.x - offset, gameObject.transform.position.y, gameObject.transform.position.z + offset);
				gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
				break;
			case 2:
				rotation = 3;
				gameObject.transform.position = new Vector3(gameObject.transform.position.x + offset, gameObject.transform.position.y, gameObject.transform.position.z + offset );
				gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
				break;
			case 3:
				rotation = 0;
				gameObject.transform.position = new Vector3(gameObject.transform.position.x + offset, gameObject.transform.position.y, gameObject.transform.position.z - offset);
				gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
				break;

	 }
    

    
    }

	/// <summary>
	/// Makes the player ship take damage and sets the ship as dead if it takes too much damage
	/// </summary>
	public void PlayerShipTakeDamage() 
	{
		

		playerHp--;
		combatHandler.PlayerShipHitLog();
		if (playerHp == 0)
		{
			combatHandler.PlayerShipDown(gameObject.name);
			combatHandler.ShipDown(shipSize);

		}
	
	
	}
	/// <summary>
	/// Makes the enemy ship takee damage and sets the ship as dead if it takes too much damage
	/// </summary>
	public void EnemyShipTakeDamage()
	{
		
		enemyHp--;
		combatHandler.EnemyShipHitLog();
		
		if (enemyHp == 0)
		{
			combatHandler.EnemyShipShowDown(gameObject.name);

			

		}


	}
	/// <summary>
	/// Hides the visual representation of player ships
	/// </summary>
	public void HideBody() 
	{ 

		body.SetActive(false);
	

	}
	/// <summary>
	/// Shows the visual representation of player ships
	/// </summary>
	public void ShowBody()
	{

		body.SetActive(true);

	}
}
