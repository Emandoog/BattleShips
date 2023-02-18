using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    public int rotation = 0;
    public int shipSize = 2;
    public float offset = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
       // RotateShip();

	}

    // Update is called once per frame
    void Update()
    {
        
    }
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
}
