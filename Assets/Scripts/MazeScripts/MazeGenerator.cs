using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode _mazeNodePrefab; // maze unit
    [SerializeField] private Vector2Int _mazeSize; //size of maze

    /// <summary>
    /// Randomly generates number within the set boundaries
    /// </summary>
    /// <param name="minNumber">inclusive</param>
    /// <param name="maxNumber">exclusive</param>
    /// <param name="numberExclude">exclusive within boundaries</param>
    /// <returns></returns>
    private int RandomExcept(int minNumber, int maxNumber, int numberExclude)
    {
        int number;
        do
        {
            number = Random.Range(minNumber, maxNumber);
        } while (number == numberExclude);
        return number;
    }

    /// <summary>
    /// Randomly generates number within the set boundaries
    /// </summary>
    /// <param name="minNumber">inclusive</param>
    /// <param name="maxNumber">exclusive</param>
    /// <param name="numberExclude1">exclusive within boundaries</param>
    /// <param name="numberExclude2">exclusive within boundaries</param>
    /// <returns></returns>
    private int RandomExcept(int minNumber, int maxNumber, int numberExclude1, int numberExclude2)
    {
        int number;
        do
        {
            number = Random.Range(minNumber, maxNumber);
        } while (number == numberExclude1 || number == numberExclude2);
        return number;
    }

    /*
     * Instantly generates maze
     */
    private void GeterateMazeInstant(Vector2Int mazeSize)
    {
        List<MazeNode> nodes = new List<MazeNode>();
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                Vector3 mazePosition = new Vector3(x - (mazeSize.x / 2f), 0, y - (mazeSize.y / 2f)); // startig right from the center of an empty GameObject
                MazeNode mazeNode = Instantiate(_mazeNodePrefab, mazePosition, Quaternion.identity, transform); // creating the maze without holes yet
                nodes.Add(mazeNode);
            }
        }
        List<MazeNode> currentPath = new List<MazeNode>(); // to create holes in nodes
        List<MazeNode> completedNodes = new List<MazeNode>(); // to check on if holes are already present in nodes

        currentPath.Add(nodes[Random.Range(0, nodes.Count)]); // choosing a starting node

        while (completedNodes.Count < nodes.Count)
        {
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / mazeSize.y; // current node's x-position
            int currentNodeY = currentNodeIndex % mazeSize.y; // current node's y-position

            if (currentNodeX < mazeSize.x - 1) // Checking node to the right of the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + mazeSize.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + mazeSize.y]))
                {
                    possibleDirections.Add(1); // 1 = PosX; 2 = NegX; 3 = PosZ; 4 = NegZ
                    possibleNextNodes.Add(currentNodeIndex + mazeSize.y);
                }
            }
            if (currentNodeX > 0) // Checking node to the left of the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - mazeSize.y]) &&
                !currentPath.Contains(nodes[currentNodeIndex - mazeSize.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - mazeSize.y);
                }
            }
            if (currentNodeY < mazeSize.y - 1) //Checking node above the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0) //Checking node below the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            if (possibleDirections.Count > 0) //Chooseing next node
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];
                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1); // Should be NegXWall because we go to the right 
                        currentPath[currentPath.Count - 1].RemoveWall(0); // Should be PosXWall because we open the passage to the right
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }
                currentPath.Add(chosenNode);
            }
            else // Backtracking if we are stuck with no awailable nodes around 
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
        int randomNumberForGem = RandomExcept(0, nodes.Count, nodes.IndexOf(nodes[0]));
        nodes[randomNumberForGem].SetActiveBonusGem(); //Randomly activate bounus gem
        int randomNumberForTimeTurner = RandomExcept(0, nodes.Count, randomNumberForGem, nodes.IndexOf(nodes[0]));
        nodes[randomNumberForTimeTurner].SetActiveTimeTurnerGem();
    }

    /*
     * Geterates maze gradually, over time set in coroutine
     */
    IEnumerator GeterateMaze(Vector2Int mazeSize)
    {
        List<MazeNode> nodes = new List<MazeNode>();
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                Vector3 mazePosition = new Vector3(x - (mazeSize.x / 2f), 0, y - (mazeSize.y / 2f));
                MazeNode mazeNode = Instantiate(_mazeNodePrefab, mazePosition, Quaternion.identity, transform);
                nodes.Add(mazeNode);
                yield return new WaitForSeconds(0.05f);
            }
        }
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        currentPath.Add(nodes[Random.Range(0, nodes.Count)]); // choosing a starting node
        currentPath[0].SetState(NodeState.Current); // setting it current

        while (completedNodes.Count < nodes.Count)
        {
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / mazeSize.y;
            int currentNodeY = currentNodeIndex % mazeSize.y;

            if (currentNodeX < mazeSize.x - 1) // Checking node to the right of the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + mazeSize.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + mazeSize.y]))
                {
                    possibleDirections.Add(1); // 1 = PosX; 2 = NegX; 3 = PosZ; 4 = NegZ
                    possibleNextNodes.Add(currentNodeIndex + mazeSize.y);
                }
            }
            if (currentNodeX > 0) // Checking node to the left of the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - mazeSize.y]) &&
                !currentPath.Contains(nodes[currentNodeIndex - mazeSize.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - mazeSize.y);
                }
            }
            if (currentNodeY < mazeSize.y - 1) //Checking node above the current node
            {
                if(!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0) //Checking node below the current node
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            if (possibleDirections.Count > 0) //Chooseing next node
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];
                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1); // Should be NegXWall because we go to the right 
                        currentPath[currentPath.Count - 1].RemoveWall(0); // Should be PosXWall because we open the passage to the right
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
                chosenNode.SetState(NodeState.Current);
            }
            else // Backtracking if we are stuck with no awailable nodes around 
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Start()
    {
        GeterateMazeInstant(_mazeSize);
        //StartCoroutine(GeterateMaze(_mazeSize));
    }
}
