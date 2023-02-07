using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed
}

/*
 *Class that describes the essential characteristics of a maze unit, i.e. 
 *such essential constructs as walls and floor as well as what that unit can possibly
 *contain - bonusGems and timeTurnerGem
 */
public class MazeNode : MonoBehaviour
{
    public GameObject bonusGem; // gem we get bonus points from
    public GameObject timeTurnerGem; // gem that winds time back 
    [SerializeField] private GameObject[] _walls;
    [SerializeField] private MeshRenderer floor;

    /// <summary>
    /// Function to remove walls, since we need gaps for a ball to go through a maze
    /// </summary>
    /// <param name="wallToRemove"></param>
    public void RemoveWall(int wallToRemove)
    {
        _walls[wallToRemove].gameObject.SetActive(false);
    }

    public void SetActiveBonusGem()
    {
        bonusGem.SetActive(true); //Initially our bonusGem is disactivated
    }

    public void SetActiveTimeTurnerGem()
    {
        timeTurnerGem.SetActive(true); //Initially our timeTurner is disactivated
    }

    /*
     * This function is needed to see how our maze generates itself according to 
     * the insructions set in MazeGenerator class. Through coroutine and colors set in this
     * function we can see the path of generation algorithm 
     */
    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white;
                break;
            case NodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.material.color = Color.blue;
                break;
            default:
                break;
        }
    }


}
