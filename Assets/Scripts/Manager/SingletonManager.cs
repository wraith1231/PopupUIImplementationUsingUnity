using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager s_instance;

    public ItemData GetItem(int num) { return  s_instance.GetItem(num); }

    private List<int> _itemDataIds = new List<int>();
    public List<int> ItemDataIds { get { return _itemDataIds; } }
    public Dictionary<int, ItemData> ItemData { get { return _itemDatas; } }
    private Dictionary<int, ItemData> _itemDatas = new Dictionary<int, ItemData>();

    void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Init()
    {
        GetApi();
    }

    public static ItemData GetRandomItem()
    {
        System.Random random = new System.Random();
        int id = s_instance.ItemDataIds[random.Next(s_instance.ItemDataIds.Count)];

        return s_instance.ItemData[id];
    }

    private async void GetApi()
    {
        string baseUrl = "https://fakestoreapi.com/products";
        //string query = "?date=2025-08-03";
        //string data = await GetApiDataAsync(baseUrl + query);
        string data = await GetApiDataAsync(baseUrl);

        data = "{\"datas\": " + data + "}";
        ItemDataWrapper wrapper = JsonUtility.FromJson<ItemDataWrapper>(data);

        try
        {
            foreach (ItemData item in wrapper.datas)
            {
                _itemDataIds.Add(item.id);
                _itemDatas[item.id] = item;
                StartCoroutine(LoadTexture(item.image));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}");
        }

    }

    private async Task<string> GetApiDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"API 호출 실패 : {e.Message}");
                return null;
            }
        }
    }

    private IEnumerator LoadTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            
            foreach(int item in _itemDataIds)
            {
                if (_itemDatas[item].image.Equals(url))
                {
                    _itemDatas[item].texture = texture;
                }
            }

        }
    }
}
