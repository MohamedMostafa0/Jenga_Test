using JengaTest.Models;
using System.Collections.Generic;

namespace JengaTest.Managers
{
    public static class DataManager
    {
        public static List<StackModel> StackModels { get; private set; }
        public static void SetStackModels(List<StackModel> stackModels) => StackModels = stackModels;
    }
}
