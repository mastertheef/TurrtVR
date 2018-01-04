using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour {
    [SerializeField] private float shipLength = 12f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float portalInOffset = 102f;
    [SerializeField] private float portalOutOffset = 227f;
    [SerializeField] private float spawnDistance = 350f;

    [SerializeField] private GameObject PortalPrefab;
    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject invisibleCapsule;
    

    private bool started = false;
    private bool visible = false;
    private Vector3 invisibleCapsuleDefaultPosition;
    private GameObject portal;

	// Use this for initialization
	void Start () {
        invisibleCapsuleDefaultPosition = invisibleCapsule.transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (!started)
        {
            StartCoroutine(Teleporting());
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator MoveIn()
    {
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalInOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-1)) 
        {
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
       
        Destroy(portal.gameObject);
        visible = true;
    }

    private IEnumerator MoveOut()
    {
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalOutOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        invisibleCapsule.transform.localPosition = new Vector3(0, 0, -4.4f - shipLength*2);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-2))
        {
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(portal.gameObject);

        ship.transform.localPosition = Vector3.zero;
        invisibleCapsule.transform.localPosition = invisibleCapsuleDefaultPosition;
        visible = false;
    }

    private Vector3 GetRandomPosition()
    {
        Quaternion randAng = Quaternion.Euler(Random.Range(GameManager.Instance.EnemyMaxBottom, GameManager.Instance.EnemyMaxTop), Random.Range(GameManager.Instance.EnemyMaxLeft, GameManager.Instance.EnemyMaxRight), 0);
        return randAng * Vector3.forward * spawnDistance;
    }

    private IEnumerator Teleporting()
    {
        started = true;
        while (true)
        {
            transform.position = GetRandomPosition();

            StartCoroutine(MoveIn());
            while (!visible) { yield return null; }

            yield return new WaitForSeconds(5);
            StartCoroutine(MoveOut());
            while (visible) { yield return null; }

            yield return new WaitForSeconds(1);
        }
    }
}
