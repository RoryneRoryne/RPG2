using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmosRadius = 0.3f;
        
        //ondrawgizmos is a method to make unity draw a gizmos on certain object.
        //ondrawgizmos adalah sebuah method yang berfungsi untuk membuat unity menggambar gizmos pada object tertentu.
        private void OnDrawGizmos() 
        {
            //this loop is to cycle all the waypoint so they gizmos will be drawn.
            //berfungsi untuk mengulang seluruh waypoint agar semua gizmos pada waypoint bisa tergambar.
            //the reason why the length in loop is transform.childCount because transform is the one who manage all parenting in hierarchy.
            //alasan pada length loop berisi transform.childCount karena transform lah yang mengatur semua parent pada hierarchy.
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                //to draw gizmos for patrol path poin.
                //untuk menggambar gizmos pada poin patrol path.
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }
        //this method is to connect one waypoint to another.
        //untuk menghubungkan waypoint ke waypoint lain.
        public int GetNextIndex(int i)
        {
            //this if statement is to make the waypoint could go back to zero.
            //if statement ini berfungsi untuk mengembalikan waypoint ke 0.
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        //the reason why we extract this method because this function will be used in more than 1 place
        //alasan kenapa method ini di ekstrak karena fungsi pada method akan dipakai lebih dari 1 tempat
        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
