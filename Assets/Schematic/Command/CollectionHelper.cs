using UnityEngine;
using System.Collections.Generic ;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;


//集合或数组的助手类
public static class CollectionHelper
{
    //升序排列
    public static void OrderBy<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
       where TKey : IComparable<TKey>
    {
        for (int i = 0; i < array.Length - 1; i++)
            for (int j = i + 1; j < array.Length; j++)
                if (handler(array[i]).CompareTo(handler(array[j])) > 0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
    }

    //降序排列
    public static void OrderByDescending<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable
    {
        for (int i = 0; i < array.Length - 1; i++)
            for (int j = i + 1; j < array.Length; j++)
                if (handler(array[i]).CompareTo(handler(array[j])) < 0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
    }

    public delegate bool FindHandler<T>(T item);
    public delegate TKey SelectHandler<TSource, TKey>(TSource source);
    //查找
    public static T Find<T>(T[] array, FindHandler<T> handler)
    {
        foreach (var item in array)
        {
            //调用委托
            if (handler(item))
                return item;
        }
        return default(T);
    }
    //查找
    public static T[] FindAll<T>(T[] array, FindHandler<T> handler)
    {
        List<T> tempList = new List<T>();
        foreach (var item in array)
        {
            //调用委托
            if (handler(item))
                tempList.Add(item);
        }
        return tempList.Count > 0 ? tempList.ToArray() : null;
    }

    public static TKey[] Select<T, TKey>(T[] array,
        SelectHandler<T, TKey> handler)
    {
        TKey[] tempArr = new TKey[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            tempArr[i] = handler(array[i]);
        }
        return tempArr;
    }

    //bu 严谨
    public static int FindMinDifferenceIndex<T>(T[] array, T t, Func<T[], T, int> action)
    {
        if (array == null || array.Length == 0) return -1;

        return action(array, t);

    }


    /// <summary>  
    /// int数组转byte数组  
    /// </summary>  
    /// <param name="src">源int数组</param> 
    /// <param name="offset">起始位置,一般为0</param>  
    /// <returns>values</returns>  
    public static byte[] IntToBytes(int[] src, int offset)
    {
        byte[] values = new byte[src.Length * 4];
        for (int i = 0; i < src.Length; i++)
        {
            values[offset + 3] = (byte)((src[i] >> 24) & 0xFF);

            values[offset + 2] = (byte)((src[i] >> 16) & 0xFF);

            values[offset + 1] = (byte)((src[i] >> 8) & 0xFF);

            values[offset] = (byte)(src[i] & 0xFF);

            offset += 4;
        }
        return values;
    }


    /// <summary>  
    /// byte数组转int数组  
    /// </summary>  
    /// <param name="src">源byte数组</param>  
    /// <param name="offset">起始位置</param>  
    /// <returns></returns>  
    public static int[] BytesToInt(byte[] src, int offset)
    {
        int[] values = new int[src.Length / 4];
        for (int i = 0; i < src.Length / 4; i++)
        {
            int value = (int)((src[offset] & 0xFF)
                   | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            values[i] = value;
            offset += 4;
        }
        return values;
    }
    //对象转换成字节数组
    public static byte[] Object2Bytes(object obj)
    {
        byte[] buff;
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            buff = ms.GetBuffer();
        }
        return buff;
    }

    // <summary>
    /// 将byte数组转换成对象
    /// </summary>
    /// <param name="buff">被转换byte数组</param>
    /// <returns>转换完成后的对象</returns>
    public static object Bytes2Object(byte[] buff)
    {
        object obj;

        using (MemoryStream ms = new MemoryStream(buff))
        {
            IFormatter iFormatter = new BinaryFormatter();
            obj = iFormatter.Deserialize(ms);
        }

        Debug.Log(obj);

        return obj;
    }

    public static string Bytes2String(byte[] buff)
    {
        //byte[] bs = (byte[])buff.Take(leng);
        Debug.Log(Encoding.UTF8.GetString(buff));
      return Encoding.UTF8.GetString(buff);
    }


    public static byte[] String2Bytes (string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static float Find(float[] array,Func<float[], float> fun)
    {
        return fun(array);
    }
}

