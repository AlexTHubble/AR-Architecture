using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity.ThirdPary;
using GoogleSheetsToUnity;
using UnityEngine.UI;
using TMPro;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    GameObject blankWarehouseObjectPrefab;

    [SerializeField]
    GameObject warehousePrefab;

    Vector3 warehouseDimensions;
    Vector3 startingLocation;
    GameObject currentWarehouseObject;

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

    [Header("Canvas used")]
    [SerializeField]
    string defaultCanvasName;
    [SerializeField]
    string itemCreationCanvasName;
    [SerializeField]
    string warehouseCreationCanvasName;

    [HideInInspector]
    public string associatedSheet = "1sU_NTS7lvoqh7xFopYs5qJbZ5iG79CevaVdwUh6-DQw";
    [HideInInspector]
    public string warehouseItemsWorksheet = "Warehouse Items";
    [HideInInspector]
    public string warehouseInfoWorksheet = "Warehouse Info";

    float warehouseXDim, warehouseYDim;


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
        warehouseYDim = float.Parse(sheet["A2"].value);
        warehouseXDim = float.Parse(sheet["B2"].value);

        CreateWarehouseBounds();
    }

    private void CreateWarehouseBounds()
    {
        currentWarehouseObject = Instantiate(warehousePrefab);
        currentWarehouseObject.transform.localScale = new Vector2(warehouseXDim, warehouseYDim);
    }

    public void btn_LoadWarehouse()
    {
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseItemsWorksheet), ImportWarehouseInventoryData);
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, warehouseInfoWorksheet), LoadWarehouseInfo);

        AllCanvasTool.instance.EnableCanvas(defaultCanvasName, true);
    }

    public void btn_CreateNewWarehouse()
    {
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
    }

    public void btn_ReturnToWarehouseCreationMenu()
    {
        AllCanvasTool.instance.EnableCanvas(warehouseCreationCanvasName, true);
    }

    public void btn_GoToObjectCreationMenu()
    {
        AllCanvasTool.instance.EnableCanvas(itemCreationCanvasName, true);
    }
}
