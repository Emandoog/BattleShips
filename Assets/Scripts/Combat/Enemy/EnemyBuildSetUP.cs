using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyBuildSetUP : MonoBehaviour
{

	[SerializeField] BuildingHandler buildingHandler; 
	private Dictionary<Vector2, GameObject> tileMap = new Dictionary<Vector2, GameObject>();
	public int shipsSpawned = 0; 
	

	/// <summary>
	/// Spawns enemy ships on the grid
	/// </summary>
	public void SetUpEnemyShips()
	{
		
		tileMap = new Dictionary<Vector2, GameObject>(GridSetUp.instance.GetComponent<GridSetUp>().TileMap);

		
			//Spawn five enemy Ships at random tile locations 
			while (shipsSpawned < 5 ) 
			{
				bool done = false;
				var temp = Random.Range(0, tileMap.Count);

				List<int> rotations = new List<int>() {0,1,2,3};
				rotations = RandomizeElemets(rotations);
		
				var tileToCheck = tileMap.ElementAt(temp).Value;
				while(rotations.Count > 0 && done == false)
				{
					if (buildingHandler.EnemyPlaceShip(tileToCheck, rotations[0]))
					{
						shipsSpawned++;
						done = true;

					}
					rotations.RemoveAt(0);
			}
			tileMap.Remove(tileMap.ElementAt(temp).Key);
			}
		

	}

	//Basic List Randomizer
	List<T> RandomizeElemets<T>(List<T> inputList )
	{
		var temp = inputList.Count;
		List<T> outputList =  new List<T>();
		for (int i = 0; i < temp; i++)
		{
			
			int index = Random.Range(0, inputList.Count);	
			outputList.Add(inputList[index]);
			
			inputList.RemoveAt(index);

		}

		return outputList;
	
	}
}
