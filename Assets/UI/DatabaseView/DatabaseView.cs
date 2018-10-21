using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DatabaseView : MonoBehaviour
{
    public GameObject ScrollList;
    public GameObject ListItem;
    public GameObject ColumnNamesPanel;
    public GameObject ColumnValuesPanel;
    public GameObject ColumnItem;

    public void Initialize()
    {
        // find DB2 files
        List<string> fileList = Casc.GetFileListFromFolder(@"dbfilesclient");
        
        foreach (string file in fileList)
        {
            GameObject item = Instantiate(ListItem, ScrollList.transform);
            item.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = file;
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void SelectedTab (string tabName)
    {
        DB2.Read(tabName);
        ClearColumns();
        Type obj = DB2.DefinitionStructs["_" + tabName.Substring(0, tabName.Length - 4)];
        var fieldValues = obj.GetFields().ToList();

        foreach (FieldInfo member in fieldValues)
        {
            GameObject columnValueItem = Instantiate(ColumnItem, ColumnValuesPanel.transform);
            columnValueItem.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = member.FieldType.Name;
            columnValueItem.GetComponent<UnityEngine.UI.Image>().color = new Color32(130,130,130,32);

                        GameObject columnNameItem = Instantiate(ColumnItem, ColumnNamesPanel.transform);
            columnNameItem.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = member.Name;
        }
    }

    public void ClearColumns()
    {
        foreach (Transform child in ColumnValuesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ColumnNamesPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
