using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace IsoTest
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Player : MonoBehaviour
    {

        [SerializeField] float rotateSpeed = 5;

        NavMeshAgent meshAgent;
        Camera mainCamera;
        PlayerInputActions inputActions;
        private void Awake()
        {
            meshAgent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main;

            inputActions = new PlayerInputActions();
            inputActions.Enable();
            inputActions.Player.ClickPosition.performed += SetDestionation;
        }

        private void Update()
        {
            RotateIfHasPath();
            Move();
            
        }

        public void Move()
        {
            if (inputActions.Player.Movement.IsPressed())
            {
                Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();


                meshAgent.ResetPath();


                Matrix4x4 mat = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -45));

                Vector3 rotatedInput = mat.MultiplyPoint3x4(inputVector);
                Vector3 vec3Input = new Vector3(rotatedInput.x, 0, rotatedInput.y);



                meshAgent.Move(meshAgent.speed * Time.deltaTime * vec3Input);
                Rotate(vec3Input + transform.position);

            }
        }

        private void Rotate(Vector3 Target) {
            

            Vector3 direction = (Target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }

        private void RotateIfHasPath() {
            if (meshAgent.hasPath)
            {
                Rotate(meshAgent.steeringTarget);
            }
        }

        public void SetDestionation(InputAction.CallbackContext context) {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity)) {
                meshAgent.SetDestination(raycastHit.point);
            }
        }


        void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null) {
                interactable.OnInteract();
            }
        }
    }
}
