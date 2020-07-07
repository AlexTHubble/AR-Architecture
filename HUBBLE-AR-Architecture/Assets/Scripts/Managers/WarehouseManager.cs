using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    GameObject blankWarehouseObjectPrefab;
    [SerializeField]
    Transform camTranform;
    [SerializeField]
    ARAnchorManager anchorManager;

    Vector3 warehouseDimensions;

    Dictionary<string, WarehouseObject> objectsInWarehouse = new Dictionary<string, WarehouseObject>();
    WarehouseObject foundObject;
    Canvas defaultCanvas;
    InputField ui_uuidInput;

    [HideInInspector]
    public string associatedSheet = "1sU_NTS7lvoqh7xFopYs5qJbZ5iG79CevaVdwUh6-DQw";
    [HideInInspector]
    public string warehouseItemsWorksheet = "Warehouse Items";

    Vector3 startingLocation = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ImportWarehouseInventoryData(GstuSpreadSheet sheet)
    {
        foreach (var warehouseUUID in sheet.columns["UUID"])
        {
            if (warehouseUUID.value != "UUID")
            {
                //Assign the position
                string[] parsedPosValue = sheet[warehouseUUID.value, "Position"].value.Split(',', ')', '(');

                //We need to convert from the 2d location setup by the desktop app into the 3D ar space
                //To do this we place our Z value with the desktops Y value. X stays the same. And we set our Y to be the default height

                string x = parsedPosValue[1];
                string z = parsedPosValue[2];

                //Translation 1, move the objects to the new center defined by the starting location
                Vector3 worldPosition = new Vector3((float.Parse(x) - startingLocation.x),
                     0.0f, (float.Parse(z) - startingLocation.y));

                //Translation 2, move the objects by the current transform position to account for the camera not being at 0,0
                worldPosition = new Vector3(worldPosition.x + camTranform.position.x, 0.0f, worldPosition.z + camTranform.position.z);

                OnScreenDebugLogger.instance.LogOnscreen("Object " + warehouseUUID.value + " at " + worldPosition.ToString());

                Pose pos = new Pose(worldPosition, Quaternion.identity);
                ARAnchor anchor = anchorManager.AddAnchor(pos);

                //WarehouseObject newObj = Instantiate(blankWarehouseObjectPrefab, pos, Quaternion.identity).GetComponent<WarehouseObject>();

                //Assign the UUID
                //newObj.UUID = warehouseUUID.value;

                //string[] parsedTransformValue = sheet[warehouseUUID.value, "Box Dim"].value.Split(',', ')', '(');
                //
                //newObj.objTransform.localScale = new Vector2(float.Parse(parsedTransformValue[1]),
                //    float.Parse(parsedTransformValue[2]));

                //OnScreenDebugLogger.instance.LogOnscreen("Finished scaling object " + warehouseUUID.value);
                //
                ////Add to the objects in warehouse dictionary
                //objectsInWarehouse.Add(warehouseUUID.value, newObj);
                //
                //OnScreenDebugLogger.instance.LogOnscreen("Finished spawning obj " + warehouseUUID.value);
            }
        }
    }

    public void btn_LoadWarehouse()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet), ImportWarehouseInventoryData);
    }

}
