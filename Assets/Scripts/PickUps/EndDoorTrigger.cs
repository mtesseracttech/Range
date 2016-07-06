using UnityEngine;

namespace PickUps
{
    public class EndDoorTrigger : MonoBehaviour
    {
        public bool YellowButton;
        public bool PinkButton;
        public bool BlueButton;
        public bool OrangeButton;
        public Animator _leftDoor;
        public Animator _rightDoor;

        void Start()
        {
            YellowButton = false;
            PinkButton = false;
            BlueButton = false;
            OrangeButton = false;

        }

        void OnTriggerEnter(Collider other)
        {
            if (YellowButton && PinkButton && BlueButton && OrangeButton)
            {
                _leftDoor.enabled = true;
                _rightDoor.enabled = true;
            }
        }

    }
}
