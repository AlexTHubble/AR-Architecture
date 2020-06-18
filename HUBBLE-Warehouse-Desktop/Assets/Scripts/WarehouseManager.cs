using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity.ThirdPary;
using GoogleSheetsToUnity;
using UnityEngine.UI;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    GameObject blankWarehouseObjectPrefab;

    Vector3 warehouseDimensions;
    Vector3 startingLocation;

    Dictionary<string, WarehouseObject> objectsInWarehouse = new Dictionary<string, WarehouseObject>();

    [SerializeField]
    Canvas defaultCanvas;
    [SerializeField]
    Canvas createNewWarehouseCanvas;

    [SerializeField]
    InputField warehouseXInput;
    [SerializeField]
    InputField warehouseYInput;
    [SerializeField]
    InputField objectXSize;
    [SerializeField]
    InputField objectYSize;

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

    private void ImportWarehouseData(GstuSpreadSheet sheet)
    {
        foreach (var warehouseUUID in sheet.columns["UUID"])
        {
            //Assign the position
            string[] parsedPosValue = sheet[warehouseUUID.value, "Position"].value.Split(',', '(', ')');

            Vector3 pos = new Vector2(float.Parse(parsedPosValue[0]),
                 float.Parse(parsedPosValue[1]));

            WarehouseObject newObj = Instantiate(blankWarehouseObjectPrefab, pos, Quaternion.identity).GetComponent<WarehouseObject>();

            //Assign the UUID
            newObj.UUID = warehouseUUID.value;

            string[] parsedTransformValue = sheet[warehouseUUID.value, "Box Dim"].value.Split('x');

            newObj.objTransform.localScale = new Vector2(float.Parse(parsedTransformValue[0]),
                float.Parse(parsedTransformValue[1]));

            //Add to the objects in warehouse dictionary
            objectsInWarehouse.Add(warehouseUUID.value, newObj);
        }
    }

    private void UpdateWarehouseData(GstuSpreadSheet sheet)
    {
        BatchRequestBody updateRequest = new BatchRequestBody();

        foreach (KeyValuePair<string, WarehouseObject> pair in objectsInWarehouse)
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

    public void btn_CreateNewWarehouse()
    {

    }

    public void btn_AddNewItem()
    {

    }
}
