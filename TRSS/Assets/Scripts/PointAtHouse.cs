using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TRSS {

    public class PointAtHouse : MonoBehaviour {

        public ArenaManager aManager;
        public CreateHouses cHouse;

        GameObject truck;
        GameObject curHouse;

        // Use this for initialization
        void Start() {

            aManager = Object.FindObjectOfType<ArenaManager>();
            cHouse = aManager.GetComponent<CreateHouses>();

        }

        // Update is called once per frame
        void Update() {

            //gameObject.transform.Rotate(Point());

        }

        void findTarget() {

            GameObject thisHouse;
            MoveHouse thisScript;

            for (int i = 0; i < cHouse.houses.Count; i++) {

                thisHouse = cHouse.houses[i];
                thisScript = thisHouse.GetComponent("MoveHouse") as MoveHouse;

                if (thisScript.target == true)
                {

                    curHouse = thisHouse;
                    break;

                }

            }

        }

        public float Point() {

            findTarget();

            float angle;

            angle = Vector3.Angle(gameObject.transform.position, curHouse.transform.position);
    
            return angle;

        }

    }
}
