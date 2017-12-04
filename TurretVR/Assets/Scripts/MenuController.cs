using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] GameObject firstButton;
        // Use this for initialization
	void Start () {
        EventSystem.current.SetSelectedGameObject(firstButton);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Play()
    {
        SceneController.Instance.FadeAndLoadScene("Space1");
    }
}
