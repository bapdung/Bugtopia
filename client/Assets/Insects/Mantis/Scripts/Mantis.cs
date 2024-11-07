using UnityEngine;
using System.Collections;

public class Mantis : MonoBehaviour {
    Animator mantis;
    private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
        mantis = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.S))
        {
            mantis.SetBool("idle", true);
            mantis.SetBool("walk", false);
            mantis.SetBool("turnleft", false);
            mantis.SetBool("turnright", false);
            mantis.SetBool("eat", false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            mantis.SetBool("walk", true);
            mantis.SetBool("idle", false);
            mantis.SetBool("turnleft", false);
            mantis.SetBool("turnright", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            mantis.SetBool("turnleft", true);
            mantis.SetBool("turnright", false);
            mantis.SetBool("walk", false);
            mantis.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.D))
        {
            mantis.SetBool("turnright", true);
            mantis.SetBool("turnleft", false);
            mantis.SetBool("walk", false);
            mantis.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.F))
        {
            mantis.SetBool("attack", true);
            mantis.SetBool("idle", false);
            StartCoroutine("eat");
            eat();
        }
        if (Input.GetKey(KeyCode.Keypad1))
        {
            mantis.SetBool("hit", true);
            mantis.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Keypad0))
        {
            mantis.SetBool("die", true);
            mantis.SetBool("idle", false);
        }
	}
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);
        mantis.SetBool("turnleft", false);
        mantis.SetBool("turnright", false);
        mantis.SetBool("hit", false);
        mantis.SetBool("idle", true);
    }
    IEnumerator eat()
    {
        yield return new WaitForSeconds(0.5f);
        mantis.SetBool("eat", true);
        mantis.SetBool("attack", false);
    }
}
