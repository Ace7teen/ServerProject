using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public InputField usernameField;
    public Camera cam;
    public GameObject GUI;
    public Image hpDisplay;
    public int maxHp;
    public GameObject gameOver;
    public Image victory;
    public Image defeat;
    public string prevUsername;
    public Image hitMarker;


    private float timer;
    private float timerMax = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    
    

    private void Update()
    {

        if (Time.time >= timer + timerMax)
        {

            hitMarker.enabled = false;
        }


        if (GUI.activeSelf)
        {
            GetCurrentFill();

        }
    }

    public void GameOver(int _id)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GUI.SetActive(false);
        usernameField.text = prevUsername;
        usernameField.interactable = true;
        cam.enabled = true;

        if (Client.instance.myId == _id)
        {
            gameOver.SetActive(true);
            defeat.enabled = false;
            victory.enabled = true;


        }
        else
        {


            gameOver.SetActive(true);
            defeat.enabled = true;
            victory.enabled = false;
            
        }


    }
    
    public void HitOpp()
    {
        timer = Time.time;
        hitMarker.enabled = true;
    }

    public void HitAnOpp()
    {
        hitMarker.enabled = true;
    }

    public void Spectaterify()
    {
        GUI.SetActive(false);
        cam.enabled = true;
    }

    public void Menuify()
    {
        gameOver.SetActive(false);
        startMenu.SetActive(true);
        Client.instance.Initi();
    }

    private void GetCurrentFill()
    {
        try
        {
            float _fillAmount = (float)GameManager.players[Client.instance.myId].hp / 3;
            hpDisplay.fillAmount = _fillAmount;
        }
        catch
        {
            Debug.Log("UI not loaded yet...");
        }
    }

    public void Quit() { Application.Quit(); }

    public void ConnectToServer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.enabled = false;
        startMenu.SetActive(false);
        GUI.SetActive(true);
        usernameField.interactable = false;
        prevUsername = usernameField.text;
        Client.instance.ConnectToServer();
    }

}
