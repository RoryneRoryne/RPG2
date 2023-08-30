using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
 
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
 
        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText("{0:0}%", health.GetPercentage());
        }
    }
}
