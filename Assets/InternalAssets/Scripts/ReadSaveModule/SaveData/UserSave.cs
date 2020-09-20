using Core.ModelManagement;
using Core.SceneConfigurator;
using FullSerializer;
using OrCor_GameName;
using System;
using System.Collections.Generic;
using UniRx;

namespace Core.SaveData
{
    [System.Serializable]
    public class UserSave : BaseModelObject
    {
        public Enumerators.Language AppLanguage { get; set; }
        public List<int> ShopEnumTypes { get; set; }
        public IntReactiveProperty Coins { get; set; }


        public UserSave()
        {
            Coins = new IntReactiveProperty(250);
            AppLanguage = Enumerators.Language.NONE;
            ShopEnumTypes = new List<int>();
            //RefitViewData = new RefitViewData();   
        }      

        #region BaseDataObjImpl
        protected override string ModelPath
        {
            get { return "UserSave"; }
        }

        public override void Write()
        {
            WriteModelToFile(ModelPath);
        }

        public override BaseModelObject Read()
        {
            return ReadModelFromFile<UserSave>(ModelPath);
        }

        public override void Init()
        {
            PlayerDataManager.ModelContr.Register(this);
        }
        #endregion
    }
}