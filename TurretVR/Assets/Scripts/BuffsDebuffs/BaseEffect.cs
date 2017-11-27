using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseEffect : MonoBehaviour
{
    [SerializeField] protected float duration = 3f;
    [SerializeField] protected Text labelPrefab;
    protected Text label;
    public float Ttl { get; set; }

    protected abstract string timerText { get; }

    // Use this for initialization
    void Start()
    {
        var ui = GameObject.FindGameObjectWithTag("Canvas").transform;
        label = Instantiate(labelPrefab, ui);

        SetEffect();
        Ttl = duration;
        InvokeRepeating("CountDown", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Ttl <= 0)
        {
            RevertEffect();
            Destroy(gameObject);
        }
    }

    public void ResetTtl()
    {
        Ttl = duration;
    }

    private void OnDestroy()
    {
        Destroy(label.gameObject);
    }

    protected void CountDown()
    {
        label.enabled = true;
        if (--Ttl == 0)
        {
            CancelInvoke("CountDown");
            label.enabled = false;
        };
        label.text = string.Format(timerText, Ttl.ToString());
    }
    protected abstract void SetEffect();
    protected abstract void RevertEffect();
}
