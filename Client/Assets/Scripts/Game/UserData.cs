using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using LitJson;

public class UserData
{
    public static UserData Instance
    {
        get
        {
            if (_sInstance == null)
                _sInstance = new UserData();

            return _sInstance;
        }
    }

    public List<Record> records;

    private static UserData _sInstance;

    public void LoadRecords()
    {
        string appDBPath = "";
#if UNITY_EDITOR
        appDBPath = Application.streamingAssetsPath + "/Record/" + "SamsaraRecord.db";
        DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        appDBPath = Application.streamingAssetsPath + "/Record/" + "/SamsaraRecord.db";
        DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
#endif
        LoadPlayerRecord(db);

        db.CloseSqlConnection();
    }

    private void LoadPlayerRecord(DbAccess db)
    {
        records = new List<Record>();
        using (SqliteDataReader sqReader = db.ReadFullTable("player_record"))
        {
            while (sqReader.Read())
            {
                var record = new Record();
                record.ID = sqReader.GetInt32(sqReader.GetOrdinal("id"));
                var data = JsonMapper.ToObject(sqReader.GetString(sqReader.GetOrdinal("pos")));
                record.Pos = new Vector2(GameUtil.DoubleToFloat((double)data["x"]), GameUtil.DoubleToFloat((double)data["y"]));
                records.Add(record);
            }

            sqReader.Close();
        }
    }

    public void SavePlayerRecord(Vector2 pos)
    {
        var appDBPath = Application.streamingAssetsPath + "/Record/" + "SamsaraRecord.db";
        DbAccess db = new DbAccess(@"Data Source=" + appDBPath);
        var data = new JsonData();
        data["x"] = pos.x;
        data["y"] = pos.y;
        var str = string.Format("'{0}'", data.ToJson());
        if (records.Count == 0)
        {
            db.InsertInto("player_record", new string[] { "0", str });
            var record = new Record
            {
                ID = 0,
                Pos = pos
            };
            records.Add(record);
        }
        else
        {
            db.UpdateInto("player_record", new string[] { "'pos'" }, new string[] { str }, "'id'", "0");
            records[0].Pos = pos;
        } 

        db.CloseSqlConnection();
    }
}
