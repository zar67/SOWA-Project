using Firebase.Database;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDatabaseManager : SingletonMonoBehaviour<FirebaseDatabaseManager>
{
    private FirebaseDatabase m_firebaseDatabase;

    protected override void Awake()
    {
        base.Awake();

        m_firebaseDatabase = FirebaseDatabase.DefaultInstance;
    }

    public void AddValueChangedListener(string path, EventHandler<ValueChangedEventArgs> callback)
    {
        m_firebaseDatabase.GetReference(path).ValueChanged += callback;
    }

    public void RemoveValueChangedListener(string path, EventHandler<ValueChangedEventArgs> callback)
    {
        m_firebaseDatabase.GetReference(path).ValueChanged -= callback;
    }


    public void SaveData<T>(string path, T data)
    {
        m_firebaseDatabase.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }

    public void AppendData<T>(string path, T data)
    {
        m_firebaseDatabase.GetReference(path).Push().SetValueAsync(JsonUtility.ToJson(data));
    }

    public async Task<T> LoadData<T>(string path)
    {
        DataSnapshot dataSnapshot = await m_firebaseDatabase.GetReference(path).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return default;
        }

        return JsonUtility.FromJson<T>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> DoesDataExist(string path)
    {
        DataSnapshot dataSnapshot = await m_firebaseDatabase.GetReference(path).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void DeleteData(string path)
    {
        m_firebaseDatabase.GetReference(path).RemoveValueAsync();
    }
}
