using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity.ThirdPary;
using GoogleSheetsToUnity;
using UnityEngine.UI;
using TMPro;

public class WarehouseManager : MonoBehaviour
{
    public static WarehouseManager instance = null;

    [SerializeField]
    GameObject blankWarehouseObjectPrefab;

    [SerializeField]
    GameObject warehousePrefab;

    [SerializeField]
    GameObject startingLocationPrefab;

    Vector3 warehouseDimensions;
    Vector3 startingLocation;
    GameObject currentWarehouseObject;
    Transform currentStartingLocationTransform;

    Dictionary<string, WarehouseObject> objectsInWarehouse = new Dictionary<string, WarehouseObject>();

    [SerializeField]
    TMP_InputField warehouseXDimInput;
    [SerializeField]
    TMP_InputField warehouseYDimInput;
    [SerializeField]
    TMP_InputField boxXDimInput;
    [SerializeField]
    TMP_InputField boxYDimInput;
    [SerializeField]
    TMP_InputField itemKeyInput;
    [SerializeField]
    TMP_InputField updateUUIDInput;
    [SerializeField]
    TMP_InputField updateBoxXDimInput;
    [SerializeField]
    TMP_InputField updateBoxYDimInput;

    [Header("Canvas used")]
    [SerializeField]
    string defaultCanvasName;
    [SerializeField]
    string itemCreationCanvasName;
    [SerializeField]
    string warehouseCreationCanvasName;
    [SerializeField]
    string itemUpdateCanvasName;

    [HideInInspector]
    public string associatedSheet = "1sU_NTS7lvoqh7xFopYs5qJbZ5iG79CevaVdwUh6-DQw";
    [HideInInspector]
    public string warehouseItemsWorksheet = "Warehouse Items";
    [HideInInspector]
    public string warehouseInfoWorksheet = "Warehouse Info";

    float warehouseXDim, warehouseYDim;
    WarehouseObject objToUpdate;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void ImportWarehouseInventoryData(GstuSpreadSheet sheet)
    {
        foreach (var warehouseUUID in sheet.columns["UUID"])
        {
            if(warehouseUUID.value != "UUID")
            {
                //Assign the position
                string[] parsedPosValue = sheet[warehouseUUID.value, "Position"].value.Split(',', ')', '(');

                string x = parsedPosValue[1];
                string y = parsedPosValue[2];

                Vector2 pos = new Vector2(float.Parse(x),
                     float.Parse(y));

                WarehouseObject newObj = Instantiate(blankWarehouseObjectPrefab, pos, Quaternion.identity).GetComponent<WarehouseObject>();

                //Assign the UUID
                newObj.UUID = warehouseUUID.value;

                string[] parsedTransformValue = sheet[warehouseUUID.value, "Box Dim"].value.Split(',', ')', '(');

                newObj.objTransform.localScale = new Vector2(float.Parse(parsedTransformValue[1]),
                    float.Parse(parsedTransformValue[2]));

                //Add to the objects in warehouse dictionary
                objectsInWarehouse.Add(warehouseUUID.value, newObj);
            }
        }
    }

    private void CreateWarehouseInfo(GstuSpreadSheet sheet)
    {
        SpreadsheetManager.Write(new GSTU_Search(associatedSheet, warehouseInfoWorksheet,
            "A2"), new ValueRange(warehouseXDim.ToString()), null);

        SpreadsheetManager.Write(new GSTU_Search(associatedSheet, warehouseInfoWorksheet,
            "B2"), new ValueRange(warehouseYDim.ToString()), null);
    }

    private void LoadWarehouseInfo(GstuSpreadSheet sheet)
    {
        warehouseYDim = float.Parse(sheet["B2"].value);
        warehouseXDim = float.Parse(sheet["A2"].value);

        CreateWarehouseBounds();
    }

    private void UpdateInvidualObjectBoxDim(GstuSpreadSheet sheet)
    {
        BatchRequestBody updateRequest = new BatchRequestBody();

        //Update the obj's scale in scene
        objToUpdate.objTransform.localScale = new Vector2(float.Parse(boxXDimInput.text), float.Parse(boxYDimInput.text));

        updateRequest.Add(sheet[objToUpdate.UUID, "Box Dim"].AddCellToBatchUpdate(associatedSheet, warehouseItemsWorksheet,
            objToUpdate.objTransform.localScale.ToString()));

        updateRequest.Send(associatedSheet, warehouseItemsWorksheet, null);
    }

    private void UpdateIndvidualObjectUUID(GstuSpreadSheet sheet)
    {
        BatchRequestBody updateRequest = new BatchRequestBody();

        updateRequest.Add(sheet[objToUpdate.UUID, "UUID"].AddCellToBatchUpdate(associatedSheet, warehouseItemsWorksheet,
            itemKeyInput.text));

        objectsInWarehouse.Remove(objToUpdate.UUID);
        objectsInWarehouse.Add(itemKeyInput.text, objToUpdate);

        objToUpdate.UUID = itemKeyInput.text;

        updateRequest.Send(associatedSheet, warehouseItemsWorksheet, null);
    }

    private void UpdateWarehouseData(GstuSpreadSheet sheet)
    {
        //Begin wiping old data--------------------------------------------------------------------------------------------------
        BatchRequestBody updateRequest = new BatchRequestBody();

        foreach (var warehouseUUID in sheet.columns["UUID"])
        {
            if(warehouseUUID.value != "UUID")
            {
                updateRequest.Add(sheet[warehouseUUID.value, "Box Dim"].AddCellToBatchUpdate(associatedSheet, warehouseItemsWorksheet,
                ""));
                updateRequest.Add(sheet[warehouseUUID.value, "Position"].AddCellToBatchUpdate(associatedSheet, warehouseItemsWorksheet,
                ""));
                updateRequest.Add(sheet[warehouseUUID.value, "UUID"].AddCellToBatchUpdate(associatedSheet, warehouseItemsWorksheet,
                ""));
            }
        }

        updateRequest.Send(associatedSheet, warehouseItemsWorksheet, null);
        //End wiping old data-------------------------------------------------------------------------------------------------------

        //Begin importing new data--------------------------------------------------------------------------------------------------
        List<List<string>> warehouseObjectImportList = new List<List<string>>();

        foreach (KeyValuePair<string, WarehouseObject> pair in objectsInWarehouse)
        {
            List<string> temp = new List<string>()
            {
                pair.Value.UUID,
                pair.Value.objTransform.localScale.ToString(),
                pair.Value.objTransform.position.ToString()
            };

            warehouseObjectImportList.Add(temp);
        }

        SpreadsheetManager.Write(new GSTU_Search(associatedSheet,
        warehouseItemsWorksheet, "A2"), new ValueRange(warehouseObjectImportList), null);
        //End importing new data----------------------------------------------------------------------------------------------------
    }

    private void CreateWarehouseBounds()
    {
        if (currentWarehouseObject != null)
            Destroy(currentWarehouseObject);

        currentWarehouseObject = Instantiate(warehousePrefab);
        currentWarehouseObject.transform.localScale = new Vector2(warehouseXDim, warehouseYDim);
    }

    private void DestroyAllWarehouseObjects()
    {
        //Clear the previous list
        foreach (KeyValuePair<string, WarehouseObject> entry in objectsInWarehouse)
        {
            Destroy(entry.Value.gameObject);
        }

        objectsInWarehouse.Clear();
    }

    public void SetWarehouseObjectToUpdate(WarehouseObject objIn)
    {
        if (objToUpdate && objToUpdate != objIn)
            objToUpdate.ToggleSelected(false);

        objToUpdate = objIn;
        AllCanvasTool.instance.EnableCanvas(itemUpdateCanvasName, true);

        updateBoxXDimInput.text = objToUpdate.objTransform.localScale.x.ToString();
        updateBoxYDimInput.text = objToUpdate.objTransform.localScale.y.ToString();
        updateUUIDInput.text = objToUpdate.UUID;
    }

    public void btn_LoadWarehouse()
    {
        DestroyAllWarehouseObjects();

        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet), ImportWarehouseInventoryData);
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseInfoWorksheet), LoadWarehouseInfo);

        AllCanvasTool.instance.EnableCanvas(defaultCanvasName, true);
    }

    public void btn_CreateNewWarehouse()
    {
        DestroyAllWarehouseObjects();

        warehouseXDim = float.Parse(warehouseXDimInput.text);
        warehouseYDim = float.Parse(warehouseYDimInput.text);

        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet),
            CreateWarehouseInfo);

        CreateWarehouseBounds();

        AllCanvasTool.instance.EnableCanvas(defaultCanvasName, true);

    }

    public void btn_AddNewItem()
    {
        GameObject temp = Instantiate(blankWarehouseObjectPrefab);
        temp.transform.localScale = new Vector2(float.Parse(boxXDimInput.text), float.Parse(boxYDimInput.text));

        WarehouseObject tempWarehouseObject = temp.GetComponent<WarehouseObject>();

        tempWarehouseObject.UUID = itemKeyInput.text;

        objectsInWarehouse.Add(itemKeyInput.text, tempWarehouseObject);

        AllCanvasTool.instance.EnableCanvas(defaultCanvasName, true);
    }

    public void btn_UploadWarehouseData()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet), UpdateWarehouseData);
    }

    public void btn_ReturnToWarehouseCreationMenu()
    {
        AllCanvasTool.instance.EnableCanvas(warehouseCreationCanvasName, true);
    }

    public void btn_GoToObjectCreationMenu()
    {
        AllCanvasTool.instance.EnableCanvas(itemCreationCanvasName, true);
    }

    public void btn_GoToDefaultCanvas()
    {
        AllCanvasTool.instance.EnableCanvas(defaultCanvasName, true);

        if (objToUpdate)
            objToUpdate.ToggleSelected(false);
    }

    public void btn_UpdateObjectUUID()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet), UpdateIndvidualObjectUUID);
    }

    public void btn_UpdateObjectBounds()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet), UpdateInvidualObjectBoxDim);
    }

    public void btn_DeleteObj()
    {
        objectsInWarehouse.Remove(objToUpdate.UUID);
        GameObject.Destroy(objToUpdate.gameObject);
        objToUpdate = null;

        btn_GoToDefaultCanvas();
    }

    public void btn_SetStartingLocation()
    {
        
    }
}
