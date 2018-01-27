using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class PlayerActionPoint : MonoBehaviour
{
    public virtual void Update()
    {

    }

    public virtual void Start()
    {

    }

    public virtual bool CheckPlayerAction(PlayerController playerController)
    {
        return false;
    }

    public virtual bool isActive()
    {

        return false;
    }

    public virtual void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        
    }
}

