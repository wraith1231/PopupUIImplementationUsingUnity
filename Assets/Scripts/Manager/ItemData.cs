using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemDataWrapper
{
    public ItemData[] datas;
}
[System.Serializable]
public struct ratings
{
    public double rate;
    public int count;
}

[System.Serializable]
public class ItemData
{
    public int id;
    public string title;
    public double price;
    public string description;
    public string category;
    public string image;
    public ratings rating;

    public Texture texture;
}
