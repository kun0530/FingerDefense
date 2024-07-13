using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTable
{
    public static readonly string FormatPath = "DataTable/{0}"; // Resources Load에 사용됨 - 추후 삭제
    public abstract void Load(string path);
}