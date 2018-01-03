using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour {

    [SerializeField] private GameObject PortalPrefab;
    [SerializeField] private float shipLength = 12f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float portalOffset = 102f;

    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject invisibleCapsule;

    private bool visible = false;

    private GameObject portal;

	// Use this for initialization
	void Start () {
        

        
	}

    private void FixedUpdate()
    {
        if (!visible)
        {
            StartCoroutine(MoveIn());
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator MoveIn()
    {
        visible = true;
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-1)) 
        {
            
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
       
        Destroy(portal.gameObject);
    }

}
