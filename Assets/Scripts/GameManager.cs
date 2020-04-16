using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public Text HUDcons;
    public Image hp1;
    public Image hp2;
    public GameObject lose;


    
    public AudioClip ShaveHit;
    public AudioClip ShaveOuch;
    public AudioClip ShavePunch;
    public AudioClip ShaveKO;
    public AudioClip ShaveIdle;
    public AudioClip ShaveUgh;





    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

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

    public void Update()
    {
        StatusDEBUG();
    }

    public void StatusDEBUG()
    {
        string HUDtext = "Players:\n";
      
        foreach (var player in players.Values)
        {
            HUDtext += player.id + " - " + player.username +" - " + player.hp + "\n";
        }
        HUDcons.text = HUDtext;
    }

    public void PlayAudioFile(AudioClip _clip, PlayerManager _sourceObj )
    {
        if(_sourceObj.id == Client.instance.myId)
        {
            _sourceObj.aSource.PlayOneShot(_clip, 1);

        }
        else
        {
            _sourceObj.bSource.Stop();
            _sourceObj.aSource.PlayOneShot(_clip, 1);
            _sourceObj.bSource.PlayDelayed(_clip.length);

        }
    }

    public void Specatatify(int _id)
    {
        UIManager.instance.Spectaterify();
        

    }

    public void DestroyPlayer(int _id)
    {
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id); 
    }
    
    public void PlayerDowned(int _id)
    {
        if (_id == Client.instance.myId)
        {
            Specatatify(_id);
        }

        DestroyPlayer(_id);
       
    }
    

    public void GameOver(int _id)
    {
        UIManager.instance.GameOver(_id);
        
        for (int i = 0; i <= GameManager.players.Count; i++)
        {
            DestroyPlayer(_id);
            
        }
        players = new Dictionary<int, PlayerManager>();

        Client.instance.Disconnect();
        
    }
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
        StatusDEBUG();
    }
}
