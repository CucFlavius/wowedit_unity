using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseListItem : MonoBehaviour
{

    private GameObject DBview;

    public void MapTabClicked()
    {
        string selectedTab = gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text;

        DBview = GameObject.Find("Panel_DatabaseView");
        DatabaseView dbView = DBview.GetComponent<DatabaseView>();
        dbView.SelectedTab(selectedTab);
    }
}
