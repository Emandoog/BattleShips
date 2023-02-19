using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSetUp : MonoBehaviour
{

	public static GridSetUp instance;
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;

	}
	[SerializeField] private GameObject tile;
	[SerializeField] private GameObject parentObject;
   
  
	public Dictionary<Vector2, GameObject> TileMap = new Dictionary<Vector2, GameObject>();
	
	void Start()
    {
        SetUpGrid();
		

	}
    /// <summary>
    /// Creates grid to play on 
    /// </summary>
    private  void SetUpGrid()
    {
		bool nextLine = false;

		for (float i = 0; i < 10; i++) 
        {
        
            for (float x = 0; x<10; x++)
            {
				var tilePos = new Vector2(i, x);
				var clone =  Instantiate(tile,new Vector3(i,0,x),gameObject.transform.rotation, parentObject.transform);

                clone.name = (i + "," + x);
                clone.GetComponent<Tile>().SetTilePos(tilePos);

				TileMap.Add (tilePos, clone);
               
                //add Checker theme to the tiles
                if (nextLine)
                {
					if (x % 2 != 0)
					{
						clone.GetComponent<Tile>().OffColor();

					}

				}
                else
                { 
					if (  x % 2 == 0)
					{
						clone.GetComponent<Tile>().OffColor();

					}
				}



			}
            nextLine = nextLine == true ? false : true;
        
        }
    
    
    }
	/// <summary>
	/// Checks if given tile position is in the dictionary, returns true or false
	/// </summary>
	/// <param name="tilePos"> The tile position to check</param>
	/// <returns></returns>
	public bool IsInDictionary(Vector2 tilePos)
    {
        
        bool result = TileMap.ContainsKey(tilePos) == true ? true : false;

        return result;
    }
	/// <summary>
	/// Returns GameObject based on tile pos given, does not check if the tile is valid.
	/// </summary>
	/// <param name="tilePos"> Position of a tile </param>
	/// <returns></returns>
	public GameObject GetTileFromDic(Vector2 tilePos)
	{

		GameObject result = TileMap[tilePos];

		return result;
	}
}
