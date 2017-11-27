using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem> {

    public Dictionary<string, BaseEffect> effects;

	// Use this for initialization
	void Start () {
        effects = new Dictionary<string, BaseEffect>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddEffect(BaseEffect effect)
    {
        string key = effect.tag;

        if (!effects.ContainsKey(key))
        {
            effects.Add(key, Instantiate(effect));
        }
        else if (effects[key] == null)
        {
            effects[key] = Instantiate(effect);
        }
        else
        {
            effects[key].ResetTtl();
        }
    }
}
