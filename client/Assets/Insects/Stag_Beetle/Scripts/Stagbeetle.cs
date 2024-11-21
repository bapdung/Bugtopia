using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagbeetle : MonoBehaviour {
    Animator stagbeetle;
    IEnumerator coroutine;
	// Use this for initialization
	void Start () {
        stagbeetle = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyUp(KeyCode.W))||(Input.GetKeyUp(KeyCode.A))||(Input.GetKeyUp(KeyCode.D))||(Input.GetKeyUp(KeyCode.F))||(Input.GetKeyUp(KeyCode.Keypad1)))
        {
            stagbeetle.SetBool("idle", true);
            stagbeetle.SetBool("walk", false);
            stagbeetle.SetBool("turnleft", false);
            stagbeetle.SetBool("turnright", false);
            stagbeetle.SetBool("flyleft", false);
            stagbeetle.SetBool("flyright", false);
            stagbeetle.SetBool("attack", false);
            stagbeetle.SetBool("hit", false);
            StartCoroutine("fly");
            fly();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            stagbeetle.SetBool("walk", true);
            stagbeetle.SetBool("idle", false);
            stagbeetle.SetBool("turnleft", false);
            stagbeetle.SetBool("turnright", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            stagbeetle.SetBool("turnleft", true);
            stagbeetle.SetBool("turnright", false);
            stagbeetle.SetBool("flyleft", true);
            stagbeetle.SetBool("idle", false);
            stagbeetle.SetBool("walk", false);
            stagbeetle.SetBool("fly", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            stagbeetle.SetBool("turnright", true);
            stagbeetle.SetBool("turnleft", false);
            stagbeetle.SetBool("flyright", true);
            stagbeetle.SetBool("idle", false);
            stagbeetle.SetBool("walk", false);
            stagbeetle.SetBool("fly", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stagbeetle.SetBool("takeoff", true);
            stagbeetle.SetBool("landing", true);
            stagbeetle.SetBool("flyleft", false);
            stagbeetle.SetBool("flyright", false);
            stagbeetle.SetBool("idle", false);
            stagbeetle.SetBool("walk", false);
            stagbeetle.SetBool("turnleft", false);
            stagbeetle.SetBool("turnright", false);
            stagbeetle.SetBool("fly", false);
            StartCoroutine("fly");
            fly();
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.F))
        {
            stagbeetle.SetBool("attack", true);
            stagbeetle.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            stagbeetle.SetBool("hit", true);
            stagbeetle.SetBool("idle", false);
        }
        if (Input.GetKey(KeyCode.Keypad0))
        {
            stagbeetle.SetBool("die", true);
            stagbeetle.SetBool("idle", false);
        }
	}
    IEnumerator fly()
    {
        yield return new WaitForSeconds(1.00f);
        stagbeetle.SetBool("fly", true);
        stagbeetle.SetBool("takeoff", false);
        stagbeetle.SetBool("flyleft", false);
        stagbeetle.SetBool("flyright", false);
    }
    IEnumerator idle()
    {
        yield return new WaitForSeconds(1.5f);
        stagbeetle.SetBool("idle", true);
        stagbeetle.SetBool("landing", false);
    }
}
