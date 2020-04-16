using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    RaycastHit hit;
    Vector3 fwd;

    public float cooldown = 1f;
    public float cooldownTimer;
    public GameObject fill;


    private void Start()
    {
        fill = GameObject.Find("/Menu/GUI/Displayer/Punch/Fill");
    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void Update()
    {
        
            fill.GetComponent<Image>().fillAmount = cooldownTimer;
        
        CheckPunch();

        

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        if (cooldownTimer < 0)
        {
            cooldownTimer = 0;
        }
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
        };

        ClientSend.PlayerMovement(_inputs);
    }


    public void CheckPunch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cooldownTimer == 0)
            {
                Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f)) // TODO add layermask for ring
                {
                    Debug.Log("Hit " + hit.collider.gameObject.GetComponentInParent<PlayerManager>().id);
                    ClientSend.PlayerAttack(hit.collider.gameObject.GetComponentInParent<PlayerManager>().id);

                }
                else
                {
                    ClientSend.PlayerMiss();
                }
                cooldownTimer = cooldown;
            }
        }
    }



}
