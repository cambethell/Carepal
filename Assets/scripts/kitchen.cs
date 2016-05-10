﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.SceneManagement;

public class kitchen : MonoBehaviour {

    new public Camera camera;
    public GameObject pal;
    private Animator m_Anim;

    LevelScene kitchenScene;

    private void Awake() {
        m_Anim = pal.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        kitchenScene = new LevelScene(camera, pal, m_Anim);
        kitchenScene.movementNodes = new List<Node> {
            new Node("n1", new Vector2(3.59f, -1.78f)), // dresser
            new Node("n2", new Vector2(1.87f, -3.6f)), // door 
            new Node("n3", new Vector2(6.548397f, -3.559552f)), // left-floor 
            new Node("n4", new Vector2(-0.9f, -2.68f)), // mid-floor
            new Node("n5", new Vector2(-4.34f, -2.62f)), // desk 
            new Node("n6", new Vector2(-8.67f, -2.72f)), // footbed 
        };

        kitchenScene.clickPos = new Vector3(0, 0, 0);
        createNodeMap();

        // define clockBox clickBox and it's animation function delegate
            // position is center of object - half of width/height
        Clickable stairsBox = new Clickable(new Vector2((3.644229f - 2f), (1.755371f - 2f)), 4f, 4f, kitchenScene.movementNodes[0]);
        stairsBox.StartActivity = () => SceneManager.LoadScene("level 1");

        // populate the clickboxlist
        kitchenScene.clickBoxList = new List<Clickable> {
            stairsBox
        };
    }

    // define node adjacency
    void createNodeMap() {

        // n1 adj to n2
        Node.addAdj(kitchenScene.movementNodes[0], kitchenScene.movementNodes[1]);

        // n1 adj to n3
        Node.addAdj(kitchenScene.movementNodes[0], kitchenScene.movementNodes[2]);

        // n2 adj to n3
        Node.addAdj(kitchenScene.movementNodes[1], kitchenScene.movementNodes[2]);

        // n2 adj to n4
        Node.addAdj(kitchenScene.movementNodes[1], kitchenScene.movementNodes[3]);

        // n4 adj to n5
        Node.addAdj(kitchenScene.movementNodes[3], kitchenScene.movementNodes[4]);

        // n5 adj to n6
        Node.addAdj(kitchenScene.movementNodes[4], kitchenScene.movementNodes[5]);
    }

    // Update is called once per frame
    void Update () {

        // pal has stopped moving: checking for activities to run
        if (!kitchenScene.isPalMoving()) {

            // check if there is a recently clicked box not handled
            if (kitchenScene.clickedBox != null) {

                // if the node finished moving on is the same as the node near the object start the activity
                if (kitchenScene.clickedBox.nodeNearRect == kitchenScene.palNode) {
                    kitchenScene.clickedBox.StartActivity();
                }

                // set the clicked box to null last
                kitchenScene.clickedBox = null;
            }
        }

        // update the kitchen
        kitchenScene.sceneUpdate();
    }
}
