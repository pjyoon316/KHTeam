﻿using UnityEngine;
using System.Collections;

public class Character : Actor 
{
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        input = thisObject.AddComponent<KeyboardInput>();

        AISystem = thisObject.AddComponent<CharacterAI>();
        AISystem.m_Owner = this;
        AISystem.m_Sight = 15f;
        AISystem.m_LimitDistance = 8f;
        AISystem.m_AtkDelay = 0.5f;

        onDamage = OnDamage;
    }

    private void OnDamage(BaseEntity attacker, SkillImpactInfo skillImpact)
    {
        animation2D.OnDamage();
        currentHP -= 10f;

        if (currentHP < 0f)
        {
            animation2D.OnDead();
            Invoke("Delete", 0.5f);
        }
    }

    public void Delete()
    {
        Destroy(thisObject);
    }
    
}
