using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController controller;

    static public int speed = 5;
    static public int d_speed = 3;
    public Transform myPosition;

    
 

	// Use this for initialization
	void Start ()
    {
        myPosition.GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update ()
    {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
        if(transform.position.y < 1)
        {
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }
 
        if (Input.GetKey(KeyCode.Q) && transform.position.y < 0.5)
        {
            transform.Translate(Vector3.up * d_speed * Time.deltaTime);
        }
        // אחראי על לרדת לצלילה 
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.down * d_speed * Time.deltaTime);
        }
        // אחראי על לזהות שאני נכנס מתחת למים 
        if (transform.position.y < -1.6)
        {        
            FindObjectOfType<GameManger>().LosingAir();
        }

        if (transform.position.y > -1.6)
        {
            FindObjectOfType<GameManger>().GetAir();
        }
	}

    public void D_SpeedUp()
    {
            d_speed = d_speed + 1;
    }
}
