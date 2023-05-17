using UnityEngine;
using JengaTest.Utils;
using JengaTest.Models;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace JengaTest.Managers
{
    public class StartSceneManager : BaseSingleton<StartSceneManager>
    {
        [SerializeField] private GameObject loadingPanel;
        protected override void OnAwake()
        {
            APIManager.Instance.LoadingEventHandler += OnLoadingEvent;
            APIManager.Instance.GetStacks(OnGetStacksSuccess, OnGetStacksError);
        }

        private void OnLoadingEvent(bool loading)
        {
            loadingPanel.SetActive(loading);
        }

        private void OnGetStacksSuccess(List<StackModel> stacks)
        {
            DataManager.SetStackModels(stacks);
        }
        private void OnGetStacksError(ErrorModel error)
        {
            // TODO handle error
        }
        public void GotoGameScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
