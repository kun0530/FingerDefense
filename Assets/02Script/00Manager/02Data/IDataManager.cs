using System;

public interface IDataManager
{
    void SaveFile<T>(string fileName, T data);
    T LoadFile<T>(string fileName);
}


