using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    GameObject blankWarehouseObjectPrefab;

    Vector3 warehouseDimensions;
    Vector3 startingLocation;

    Dictionary<string, WarehouseObject> objectsInWarehouse = new Dictionary<string, WarehouseObject>();
    WarehouseObject foundObject;

    Canvas defaultCanvas;
    InputField ui_uuidInput;

    [HideInInspector]
    public string associatedSheet = "1sU_NTS7lvoqh7xFopYs5qJbZ5iG79CevaVdwUh6-DQw";
    [HideInInspector]
    public string associatedWorksheet = "Warehouse Items";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FindPath()
    {
        //TODO: This
    }

    private void ImportWarehouseData(GstuSpreadSheet sheet)
    {
        foreach (var warehouseUUID in sheet.columns["UUID"])
        {
            //Assign the position
            string[] parsedPosValue = sheet[warehouseUUID.value, "Position"].value.Split(',', '(', ')');

            Vector3 pos = new Vector3(float.Parse(parsedPosValue[0]),
                 float.Parse(parsedPosValue[1]), float.Parse(parsedPosValue[2]));

            WarehouseObject newObj = Instantiate(blankWarehouseObjectPrefab, pos, Quaternion.identity).GetComponent<WarehouseObject>();

            //Assign the UUID
            newObj.UUID = warehouseUUID.value;

            string[] parsedTransformValue = sheet[warehouseUUID.value, "Box Dim"].value.Split('x');

            newObj.objTransform.localScale = new Vector3(float.Parse(parsedTransformValue[0]),
                float.Parse(parsedTransformValue[1]), float.Parse(parsedTransformValue[2]));

            //Add to the objects in warehouse dictionary
            objectsInWarehouse.Add(warehouseUUID.value, newObj);
        }
    }

    private void UpdateWarehouseData(GstuSpreadSheet sheet)
    {
        BatchRequestBody updateRequest = new BatchRequestBody();

        foreach(KeyValuePair<string, WarehouseObject> pair in objectsInWarehouse)
        {
            //Add the updated data to the bach request
            updateRequest.Add(sheet[pair.Key, "Position"].AddCellToBatchUpdate(associatedSheet, 
                associatedWorksheet, pair.Value.objTransform.position.ToString()));
        }

        //Send the request
        updateRequest.Send(associatedSheet, associatedWorksheet, null);
    }

    public void btn_LoadWarehouse()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, associatedWorksheet), ImportWarehouseData);
    }

    public void btn_UpdateWarehouseInformation()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, associatedWorksheet), UpdateWarehouseData);
    }

    public void btn_StartSearch()
    {
        //Check to see if the uuid field is empty
        if(ui_uuidInput.text != "") 
        {
            //Check to see if the warehouse contains the key
            if(objectsInWarehouse.ContainsKey(ui_uuidInput.text))
            {
                foundObject = objectsInWarehouse[ui_uuidInput.text];
            }
            else
            {
                OnScreenDebugLogger.instance.LogOnscreen("UUID not found");
            }
        }
        else
        {
            OnScreenDebugLogger.instance.LogOnscreen("UUID feild is blank!");
        }

    }
}
