﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : Singleton<RocketController> {

    [SerializeField] int MaxRockets = 2;
    [SerializeField] Rocket RocketPrefab;
    [SerializeField] RectTransform RocketsPanel;
    [SerializeField] Image RocketImagePrefab;

    private int RocketCount;
    private List<Image> rocketImages;

	// Use this for initialization
	void Start () {
        RocketCount = MaxRockets;
        rocketImages = new List<Image>();
        for (int i = 0; i<RocketCount; i++)
        {
            rocketImages.Add(Instantiate(RocketImagePrefab, RocketsPanel));
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire3") && RocketCount > 0)
        {
            Instantiate(RocketPrefab);
            RocketCount--;
            Image rocketImage = rocketImages.Last();
            rocketImages.Remove(rocketImage);
            Destroy(rocketImage.gameObject);
        }
	}
}
