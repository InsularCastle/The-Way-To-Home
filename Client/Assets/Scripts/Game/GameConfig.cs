using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using System.IO;
using LitJson;

public class GameConfig
{
    /// <summary>
    /// 设置配置
    /// </summary>
    private List<string> _options;

    /// <summary>
    /// 语言配置
    /// </summary>
    private List<string> _languages;

    /// <summary>
    /// 关卡配置
    /// </summary>
    private Dictionary<int, LevelConfig> _levelConfigs;

    public void LoadConfigs()
    {
        string appDBPath = "";
#if UNITY_EDITOR
        appDBPath = Application.streamingAssetsPath + "/Config/" + "Samsara.db";
        DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        appDBPath = Application.streamingAssetsPath + "/Config/" + "/Samsara.db";
        DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
#elif UNITY_ANDROID && !UNITY_EDITOR
        appDBPath = Application.persistentDataPath + "/Samsara.db";
        if(!File.Exists(appDBPath))
        {  
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "Samsara.db");   
  
            while(!loadDB.isDone){}
            File.WriteAllBytes(appDBPath, loadDB.bytes);
        }  
        DbAccess db = new DbAccess("URI=file:" + appDBPath);
#endif

        LoadGlobalConfig(db);
        LoadLevelConfig(db);

        db.CloseSqlConnection();
    }

    public List<string> GetOptions()
    {
        return _options;
    }

    public List<string> GetLanguages()
    {
        return _languages;
    }

    public LevelConfig GetLevelCfg(int levelID)
    {
        if (_levelConfigs.ContainsKey(levelID))
        {
            return _levelConfigs[levelID];
        }
        return null;
    }

    private void LoadGlobalConfig(DbAccess db)
    {
        using (SqliteDataReader sqReader = db.ReadFullTable("global_config"))
        {
            while (sqReader.Read())
            {
                switch (sqReader.GetString(sqReader.GetOrdinal("name")))
                {
                    case "options":
                        string optionsValue = sqReader.GetString(sqReader.GetOrdinal("value"));
                        _options = JsonMapper.ToObject<List<string>>(optionsValue);
                        break;
                    case "languages":
                        string languagesValue = sqReader.GetString(sqReader.GetOrdinal("value"));
                        _languages = JsonMapper.ToObject<List<string>>(languagesValue);
                        break;
                }
            }

            sqReader.Close();
        }
    }

    private void LoadLevelConfig(DbAccess db)
    {
        _levelConfigs = new Dictionary<int, LevelConfig>();
        using (SqliteDataReader sqReader = db.ReadFullTable("level_config"))
        {
            while (sqReader.Read())
            {
                var level = new LevelConfig
                {
                    
                };
                _levelConfigs.Add(level.ID, level);
            }

            sqReader.Close();
        }
    }
}