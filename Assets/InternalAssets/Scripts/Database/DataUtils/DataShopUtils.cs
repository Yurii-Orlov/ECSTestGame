using Core.DataManagement;
using Core.Management;
using Core.SimpleData;
using CustomLogger;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrCor_GameName.Utils
{

    public static class DataShopUtils
    {
        public static DataShopRecord GetShopRecordById(int id)
        {
            List<DataShopRecord> shopRecords = GetDataShopRecords();
            DataShopRecord shopData = shopRecords.FirstOrDefault(p => p.Id == id);

            if(shopData == null)
            {
                DebugLogger.Log(null, "No DataShopRecord for id =>> " + id, LogColor.Red);
                return null;
            }

            return shopData;
        }

        public static List<DataShopRecord> GetDataShopRecords()
        {
            DataManager manager = CoreManager.Instance.GetData<DataManager>();

            return manager.GetScriptableObjectDictionary<DataShopRecords>().Records.ToList();
        }
    }
}
