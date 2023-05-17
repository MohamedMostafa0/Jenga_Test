using System;
using UnityEngine;
using JengaTest.Utils;
using JengaTest.Models;
using JengaTest.Helpers;
using JengaTest.Behaviours;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace JengaTest.Managers
{
    public class GameSceneManager : BaseSingleton<GameSceneManager>
    {
        private event Action<GenerationEnum> _changeGenerationEventHandler;
        public event Action<GenerationEnum> ChangeGenerationEventHandler
        {
            add => _changeGenerationEventHandler += value;
            remove => _changeGenerationEventHandler -= value;
        }

        public Transform[] generationPositions;

        private readonly Dictionary<GenerationEnum, List<StackModel>> blocksDic = new();
        private readonly Dictionary<GenerationEnum, List<Block>> blocksDicObjs = new();
        private GenerationEnum currentGeneration = GenerationEnum.Gen7;

        private void Start()
        {
            FillStacks();
            BuildStacks();
        }
        private void FillStacks()
        {
            blocksDic.Add(GenerationEnum.Gen6, new());
            blocksDic.Add(GenerationEnum.Gen7, new());
            blocksDic.Add(GenerationEnum.Gen8, new());
            if (DataManager.StackModels != null)
            {
                foreach (var item in DataManager.StackModels)
                {
                    int grade = GetItemGeneration(item) - 6;
                    if (grade < 0 || grade > 2) continue;
                    GenerationEnum gradeEnum = (GenerationEnum)grade;                    
                    blocksDic[gradeEnum].Add(item);
                }
            }
            Sort();
        }
        private void Sort()
        {
            blocksDic[GenerationEnum.Gen6] = blocksDic[GenerationEnum.Gen6].OrderBy(a => a.Domainid).ThenBy(a => a.Cluster).ThenBy(a => a.StandardId).ToList();
            blocksDic[GenerationEnum.Gen7] = blocksDic[GenerationEnum.Gen7].OrderBy(a => a.Domainid).ThenBy(a => a.Cluster).ThenBy(a => a.StandardId).ToList();
            blocksDic[GenerationEnum.Gen8] = blocksDic[GenerationEnum.Gen8].OrderBy(a => a.Domainid).ThenBy(a => a.Cluster).ThenBy(a => a.StandardId).ToList();
        }
        private void BuildStacks()
        {
            blocksDicObjs.Add(GenerationEnum.Gen6, new());
            blocksDicObjs.Add(GenerationEnum.Gen7, new());
            blocksDicObjs.Add(GenerationEnum.Gen8, new());
            foreach (var item in blocksDic)
            {
                Vector3 pos = generationPositions[(int)item.Key].position;
                pos.y += 0.25f;
                int increase = -1;
                bool shouldRotate;
                for (int i = 0; i < item.Value.Count; i++)
                {
                    Vector3 temp = pos;
                    if ((i / 3) % 2 == 0)
                    {
                        temp.z += increase;
                        shouldRotate = false;
                    }
                    else
                    {
                        temp.x += increase;
                        shouldRotate = true;
                    }
                    GameObject blockObj = ObjectPooler.Instance.GetPooledObject(((ItemType)item.Value[i].Mastery).ToString());
                    if(blockObj != null)
                    {
                        Block block = blockObj.GetComponent<Block>();
                        block.Init(temp, shouldRotate ? Quaternion.Euler(0, 90, 0) : Quaternion.identity, item.Value[i]);
                        blocksDicObjs[item.Key].Add(block);
                    }
                    increase++;
                    if(increase >= 2)
                    {
                        increase = -1;
                        pos.y += 0.5f;
                    }
                }
            }
        }
        private int GetItemGeneration(StackModel model) => model.Grade[0] - '0';
        public void ChangeGeneration(int generation)
        {
            GenerationEnum current = (GenerationEnum)generation;
            _changeGenerationEventHandler?.Invoke(current);
            currentGeneration = current;
        }
        public void ClickTestMyStack()
        {
            foreach (var item in blocksDicObjs[currentGeneration])
            {
                item.Test();
            }
        }
        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
