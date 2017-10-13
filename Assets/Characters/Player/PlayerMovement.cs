using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkMoveStopRadius = 0.2f, walkAttackStopRadius = 3f;

    ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    AICharacterControl aiCharacterControl = null;

    GameObject walkTarget;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = GetComponent <AICharacterControl>();

        walkTarget = new GameObject ("walkTarget");

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

	public Vector3 ShortenDestination (Vector3 destination, float shortenDistance)
	{
		Vector3 shortDistance = (destination - transform.position).normalized * shortenDistance;
		return destination - shortDistance;
	}

    void ProcessMouseClick (RaycastHit raycastHit, int layerHit)
    {
        switch (layerHit)
        {
            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget (walkTarget.transform);
                break;
            default:
                Debug.Log("Don't know what layer is clicked.");
                return;
        }
    }
}

