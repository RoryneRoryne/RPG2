using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
 
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
 
        private void Update()
        {
            //jila tidak memiliki target, tampilan hp enemy akan kosong.
            if (fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().SetText("");
                return;
            }
            // Menginisialisasi variabel 'health' dengan memamnggil metode getTarget dari fighter.
            Health health = fighter.GetTarget();
            //Menampilkan HP Enemy dengan memanggil metode GetPercentage dari health.
            // Format string "{0:0}%" mengindikasikan bahwa kita ingin menampilkan nilai persentase.
            GetComponent<TextMeshProUGUI>().SetText("{0:0}%", health.GetPercentage());
        }
    }
}
