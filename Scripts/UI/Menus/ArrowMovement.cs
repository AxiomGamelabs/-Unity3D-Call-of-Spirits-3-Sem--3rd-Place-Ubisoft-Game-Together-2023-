using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] private GameObject arrowContainer;
    [SerializeField] private Transform positionToMoveArrowContainer;

    public void MoveArrowContainer()
    {
        arrowContainer.SetActive(true);
        arrowContainer.transform.position = positionToMoveArrowContainer.position;
    }

    public void DeactivateArrowContainer() 
    { 
        arrowContainer.SetActive(false); 
    }
   
}
