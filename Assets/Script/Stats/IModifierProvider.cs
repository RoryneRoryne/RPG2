using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public interface IModifierProvider 
    {
        //Ienumerable sama saja dengan ienumerator tetapi ienumerable lebih mudah digunakan untuk foreach loop.
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}