using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static partial class DB2
{
    public static Dictionary<string, Type> DefinitionStructs = new Dictionary<string, Type>();

    public static void InitializeDefinitions()
    {
        DefinitionStructs.Clear();

        Type[] myTypeArray = Type.GetType("DB2+Definitions_" + Settings.Data[10]).GetNestedTypes();
        
        foreach (Type type in myTypeArray)
        {
            DefinitionStructs.Add(type.Name, type);
        }
    }
}
