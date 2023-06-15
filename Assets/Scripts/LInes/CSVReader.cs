using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{ 
    //반점과 반점 사이의 필드 안에서 값을 이용하는 정규화 식
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    
    //운영 체제 별 줄바꿈 문자 통합 스트링
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };


    //딕셔너리 여러개를 저장한 리스트를 반환하는 함수 Read
    //csv 파일 이름을 문자열 파라메터로 제공받는다.
    public static List<Dictionary<string, object>> Read(string file)
    {
        //이 함수가 리턴하고자 하는 형식의 인스턴스화
        //"list"라는 변수에 저장함
        var list = new List<Dictionary<string, object>>();

        //리소스 폴더에서 파라메터로 받은 이름을 가진 파일을 찾아서
        //TextAsset타입으로 불러와
        //"data"라는 변수에 저장
        TextAsset data = Resources.Load(file) as TextAsset;

        ///
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        //foreach (var temp in lines)
        //{
        //    Debug.Log(temp);
        //}

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}
