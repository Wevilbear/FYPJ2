using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileY;

    public TileMap map;

    public List<Node> currentPath = null;

    int moveSpeed = 2;
    float remainingMovement = 2;

    void Update()
    {
        // Draw debug line 
        if (currentPath != null)
        {
            int currNode = 0;

            while (currNode < currentPath.Count - 1)
            {

                Vector3 start = map.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) +
                    new Vector3(0, 0, -0.5f);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].y) +
                    new Vector3(0, 0, -0.5f);

                Debug.DrawLine(start, end, Color.red);

                currNode++;
            }
        }

        if (Vector3.Distance(transform.position, map.TileCoordToWorldCoord(tileX, tileY)) < 1.0f)
            AdvancePathing();

        // Smoothly animate towards the correct map tile.
        transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord(tileX, tileY), 5f * Time.deltaTime);
    }

    // Advances our pathfinding progress by one tile.
    void AdvancePathing()
    {
        if (currentPath == null)
            return;

        if (remainingMovement <= 0)
            return;

        // Teleport us to our correct "current" position, in case haven't finished the animation yet.
        transform.position = map.TileCoordToWorldCoord(tileX, tileY);

        // Get cost from current tile to next tile
        remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

        // Move us to the next tile in the sequence
        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        // Remove the old "current" tile from the pathfinding list
        currentPath.RemoveAt(0);

        if (currentPath.Count == 1)
        {
            // clear pathfinding info.
            currentPath = null;
        }
    }

    // The "Next Turn" button calls this.
    public void NextTurn()
    {
        // Make sure to wrap-up any outstanding movement left over.
        while (currentPath != null && remainingMovement > 0)
        {
            AdvancePathing();
        }

        // Reset our available movement points.
        remainingMovement = moveSpeed;
    }
}
