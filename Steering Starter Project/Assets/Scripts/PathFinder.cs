using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Kinematic
{
    public Node start;
    public Node goal;
    Graph myGraph;

    FollowPath myMoveType;
    LookWhereGoing myRotateType;

    GameObject[] myPath;

    // Start is called before the first frame update
    void Start()
    {
        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        if(gameObject.name == "Shortest")
            MakeShortestPath();
        if(gameObject.name == "CostEffective")
            MakeCostEffectivePath();
    }

    void MakeShortestPath()
    {
        Graph myGraph = new Graph();
        myGraph.Build(false);
        List<Connection> path = Dijsktra.pathFind(myGraph, start, goal);

        myPath = new GameObject[path.Count + 1];

        int i = 0;
        foreach (Connection c in path)
        {
            Debug.Log("From: " + c.getFromNode() + " to " + c.getToNode() + " and costs: " + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }

        myPath[i] = goal.gameObject;

        myMoveType = new FollowPath();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    void MakeCostEffectivePath()
    {
        Graph newGraph = new Graph();
        newGraph.Build(true);
        List<Connection> newPath = Dijsktra.pathFind(newGraph, start, goal);

        myPath = new GameObject[newPath.Count + 1];

        int i = 0;
        foreach (Connection c in newPath)
        {
            Debug.Log("From: " + c.getFromNode() + " to " + c.getToNode() + " and costs: " + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }

        myPath[i] = goal.gameObject;

        myMoveType = new FollowPath();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.angular = myRotateType.getSteering().angular;
        steeringUpdate.linear = myMoveType.getSteering().linear;
        base.Update();
    }
}

