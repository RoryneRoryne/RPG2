using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class ExpDisplay : MonoBehaviour
    {
        Experience experience;
        // Start is called before the first frame update
        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText("{0:0}", experience.GetExp());
        }
    }
}
