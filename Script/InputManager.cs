using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoundPixel;

public class InputManager : MonoSingleton<InputManager>
{
    public float borderTickness = 20f;
    public State state = State.NONE;

    public Texture2D cursorPlant;
    public Texture2D cursorWater;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    // Start is called before the irst frame update
    void Start()
    {
        
    }


    public void Plant()
    {
        state = State.PLANT;
        Cursor.SetCursor(cursorPlant, hotSpot, cursorMode);
    }

    public void Water()
    {
        Cursor.SetCursor(cursorWater, hotSpot, cursorMode);
        state = State.WATER;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z) || Input.mousePosition.y >= Screen.height - borderTickness)
        {
            CameraManager.Instance.MoveForward();
        }

        if (Input.GetKey(KeyCode.Q) || Input.mousePosition.x <= borderTickness)
        {
            CameraManager.Instance.MoveLeft();
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= borderTickness)
        {
            CameraManager.Instance.MoveBack();
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - borderTickness)
        {
            CameraManager.Instance.MoveRight();
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (state == State.PLANT)
                {

                    if (hit.collider.gameObject.CompareTag("Terrain"))
                    {
                        TreeController.Instance.GenerateTree(hit.point);
                        //BuildingController.Instance.CreateBuildAtPosition(hit.point, isSelectingBuilding);
                    }

                }
                if (state == State.WATER)
                {
                    Debug.Log("je passe la");
                    if (hit.collider.gameObject.CompareTag("Tree"))
                    {
                        Debug.Log("hrtr");
                        TreeController.Instance.GrowingUp(hit.collider.gameObject);
                    }
                }
            }
            state = State.NONE;
            Cursor.SetCursor(null, hotSpot, cursorMode);
        }
            /* if (Input.GetKeyDown(KeyCode.B))
             {
                 UIController.Instance.setBuildUI();
             }
             if (Input.GetKeyDown(KeyCode.Escape))
             {
                 Debug.Log("ESCAPE Toggle!");
                 if (UIController.Instance.BuildPanel.activeSelf)
                 {
                     Debug.Log("QUIT BUILD PANEL");
                     UIController.Instance.setBuildUI();
                 }
                 else
                 {
                     if (UIController.Instance.SelectingBuildPanel.activeSelf)
                     {
                         UIController.Instance.setSelectBuildPanelUI();
                         SelectionController.Instance.isSelectingBuilding = SelectingBuilding.NONE;
                         SelectionController.Instance.selectionState = SelectionState.SELECTING;
                         BuildingController.Instance.RemovePreviewBuild();
                     }
                     else
                     {
                         Debug.Log("QUIT !");
                         UIController.Instance.setQuitUI();
                     }
                 }
             }*/
        }
}
