using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarantula : MonoBehaviour
{
    private Animator tarantula;
    // Start is called before the first frame update
    void Start()
    {
        tarantula = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tarantula.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            tarantula.SetBool("bite", false);
            tarantula.SetBool("threat", false);
            tarantula.SetBool("hit", false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            tarantula.SetBool("bite", true);
            tarantula.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            tarantula.SetBool("walk", true);
            tarantula.SetBool("idle", false);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            tarantula.SetBool("idle", true);
            tarantula.SetBool("walk", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            tarantula.SetBool("turnright", true);
            tarantula.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            tarantula.SetBool("turnleft", true);
            tarantula.SetBool("idle", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            tarantula.SetBool("turnright", false);
            tarantula.SetBool("idle", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            tarantula.SetBool("turnleft", false);
            tarantula.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            tarantula.SetBool("threat", true);
            tarantula.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            tarantula.SetBool("hit", true);
            tarantula.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            tarantula.SetBool("die", true);
            tarantula.SetBool("idle", false);
        }
    }
}
