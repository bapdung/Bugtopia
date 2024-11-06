using UnityEngine;
using System.Collections;

public class Beetle : MonoBehaviour {
    Animator beetle;
    private IEnumerator coroutine;

	void Start ()
    {
        beetle = GetComponent<Animator>();
	}
	
	void Update()
    {
        if (beetle.GetCurrentAnimatorStateInfo(0).IsName("fly"))
        {
            beetle.SetBool("takeoff", false);
            beetle.SetBool("landing", false);
        }
        if (beetle.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            beetle.SetBool("attack", false);
            beetle.SetBool("hit", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            beetle.SetBool("walk", true);
            beetle.SetBool("idle", false);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            beetle.SetBool("walk", false);
            beetle.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            beetle.SetBool("backward", true);
            beetle.SetBool("idle", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            beetle.SetBool("backward", false);
            beetle.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            beetle.SetBool("turnleft", true);
            beetle.SetBool("idle", false);
            beetle.SetBool("fly", false);
            beetle.SetBool("flyleft", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            beetle.SetBool("turnleft", false);
            beetle.SetBool("idle", true);
            beetle.SetBool("flyleft", false);
            beetle.SetBool("fly", true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            beetle.SetBool("turnright", true);
            beetle.SetBool("idle", false);
            beetle.SetBool("flyright", true);
            beetle.SetBool("fly", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            beetle.SetBool("turnright", false);
            beetle.SetBool("idle", true);
            beetle.SetBool("flyright", false);
            beetle.SetBool("fly", true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beetle.SetBool("takeoff", true);
            beetle.SetBool("idle", false);
            beetle.SetBool("landing", true);
            beetle.SetBool("fly", false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            beetle.SetBool("attack", true);
            beetle.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            beetle.SetBool("hit", true);
            beetle.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            beetle.SetBool("die", true);
            beetle.SetBool("idle", false);
        }
    }

       

}
