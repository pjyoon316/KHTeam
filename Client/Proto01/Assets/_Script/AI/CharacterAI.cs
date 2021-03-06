﻿using UnityEngine;
using System.Collections;

public class CharacterAI : BaseAI, IGameEventListener
{
    /// <summary>
    /// 터치 됬나?
    /// </summary>
    public bool m_IsTouched;

    private bool isEnableRange { get { return Vector3.Distance(m_Owner.thisTransform.position, m_Target.thisTransform.position) < m_LimitDistance; } }
    private bool isEnableAttack { get { return m_LastAtkTime + m_AtkDelay < Time.realtimeSinceStartup; } }

    public override void AIOn()
    {
        base.AIOn();

        GameEventManager.Register(this);
    }
    
    public override void AIOff()
    {
        base.AIOff();

        GameEventManager.Unregister(this);
    }


    protected override void UpdateAI()
    {
        // 1. 타겟 설정( 못찾으면 리턴)
        UpdateTarget();

        // 2. 타겟에게 다가가기( 다가가는 중이면 리턴)
        if (!ApporachTarget())
            return;

        // 3. 타겟에게 공격(스킬버튼이 눌렸다면 스킬씀)
        ProcessAttack();           
    }

    private void UpdateTarget()
    {
        if(m_Target == null)
        {
            int distance = 5000;
            int index = -1;
            for (int i = 0; i < IngameManager.Instance.m_monsterList.Count; i++ )
            {
                float dis = Vector3.Distance(m_Owner.thisTransform.position, IngameManager.Instance.m_monsterList[i].thisTransform.position);
                if(distance > dis)
                {
                    distance = (int)dis;
                    index = i;
                }
            }

            if (index != -1)
                m_Target = IngameManager.Instance.m_monsterList[index];
        }
    }

    private bool ApporachTarget()
    {
        if (m_Target == null) return false;

        // 다가가기
        if (m_Target != null && !isEnableRange)
        {            
            m_Owner.Move();
            return false;
        }
        else
        {
            // 다가가기 끝
            m_Owner.MoveStop();
            return true;
        }
    }

    private void ProcessAttack()
    {
        if (m_Target == null) return;

        //터치되었으면 공격
        if (m_IsTouched)
        {
            m_Owner.Attack(GameType.AnimationState.Attack);
            m_IsTouched = false;
            return;
        }

        if(isEnableAttack)
        {
            m_Owner.Attack(GameType.AnimationState.Attack);
            m_LastAtkTime = Time.realtimeSinceStartup;
        }
    }

    public void OnGameEvent(GameEventType gameEventType, params object[] args)
    {
        switch(gameEventType)
        {
            case GameEventType.Click_Screen:
                m_IsTouched = true;
                break;
        }
    }
}