using System;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using JengaTest.Utils;
using JengaTest.Models;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace JengaTest.Managers
{
    public sealed class APIManager : BaseSingleton<APIManager>
    {
        private event Action<bool> _loadingEventHandler;
        public event Action<bool> LoadingEventHandler
        {
            add => _loadingEventHandler += value;
            remove => _loadingEventHandler -= value;
        }

        [SerializeField] private string baseUrl;

        public void GetStacks(Action<List<StackModel>> onSuccess, Action<ErrorModel> onError)
        {
            SendAPI<object, List<StackModel>>("Assessment/stack", HttpMethod.Get, null, onSuccess, onError);
        }
        private void SendAPI<TRequest, TResponse>(string url, HttpMethod method, TRequest body, Action<TResponse> onSuccess, Action<ErrorModel> onError)
        {
            try
            {
                string data = JsonConvert.SerializeObject(body);
                StartCoroutine(Call<TResponse>(url, method, data, onSuccess, onError));
            }
            catch (Exception ex)
            {
                onError?.Invoke(new(500, ex.Message));
            }
        }
        private IEnumerator Call<TResponse>(string url, HttpMethod method, string data, Action<TResponse> onSuccess, Action<ErrorModel> onError)
        {
            _loadingEventHandler?.Invoke(true);
            using UnityWebRequest www = new($"{baseUrl}/{url}", method.Method);
            www.downloadHandler = new DownloadHandlerBuffer();
            // headers
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");
            // body
            if (!string.IsNullOrEmpty(data))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
                onError?.Invoke(new((int)www.responseCode, www.error));
            else
            {
                TResponse model = JsonConvert.DeserializeObject<TResponse>(www.downloadHandler.text);
                onSuccess?.Invoke(model);
            }
            _loadingEventHandler?.Invoke(false);
        }
    }
}