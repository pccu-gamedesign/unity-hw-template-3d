#if UNITY_EDITOR
using System.IO;
#endif
using UnityEngine;

/// <summary>
/// 取得 Gemini API key 的唯一入口（第 11 週起使用）。
///
/// ・在 Unity 編輯器裡：讀專案根目錄的 Secrets/gemini_key.txt。
///   這個資料夾在 Assets 外面，不會被打包進遊戲，也已經被 .gitignore 排除，不會上傳 GitHub。
///   用 Unity 選單「AI 工具 → 設定 Gemini API Key」貼上 key 就好，不用自己建檔。
/// ・在網頁版（WebGL）等正式 build：遊戲裡沒有任何 key，只能用玩家自己輸入的 key（SetRuntimeKey）。
///
/// 不要把 key 寫進任何 .cs 檔、場景或 Prefab：build 之後 key 會跟著遊戲一起公開。
/// </summary>
public static class GeminiKeyProvider
{
    /// <summary>key 檔的位置（相對於專案根目錄）。</summary>
    public const string KeyFilePath = "Secrets/gemini_key.txt";

    static string runtimeKey;

    /// <summary>設定玩家在遊戲中輸入的 key：只存在記憶體，關掉遊戲就消失。</summary>
    public static void SetRuntimeKey(string key)
    {
        runtimeKey = Clean(key);
    }

    /// <summary>目前有沒有可用的 key。</summary>
    public static bool HasKey => !string.IsNullOrEmpty(GetKey());

    /// <summary>取得 key；沒有就回傳空字串（不會丟例外）。</summary>
    public static string GetKey()
    {
        if (!string.IsNullOrEmpty(runtimeKey))
            return runtimeKey;

#if UNITY_EDITOR
        foreach (string path in EditorKeyFileCandidates())
        {
            if (!File.Exists(path))
                continue;

            foreach (string line in File.ReadAllLines(path))
            {
                string key = Clean(line);
                if (key.Length > 0)
                    return key;
            }
        }
#endif
        return "";
    }

    /// <summary>沒有 key 時，可以顯示給玩家看的說明。</summary>
    public static string MissingKeyMessage
    {
        get
        {
#if UNITY_EDITOR
            return "找不到 Gemini API key：請用 Unity 選單「AI 工具 → 設定 Gemini API Key」貼上你的 key。";
#else
            return "線上版沒有內建 AI 金鑰，AI 對話功能未開放。";
#endif
        }
    }

#if UNITY_EDITOR
    /// <summary>key 檔的完整路徑（只在編輯器裡有）。</summary>
    public static string EditorKeyFileFullPath =>
        Path.Combine(Directory.GetParent(Application.dataPath).FullName, KeyFilePath);

    static string[] EditorKeyFileCandidates() => new[]
    {
        EditorKeyFileFullPath,
        EditorKeyFileFullPath + ".txt",   // Windows 隱藏副檔名時，手動建檔常變成 gemini_key.txt.txt
    };
#endif

    static string Clean(string s) => (s ?? "").Trim().Trim('"', '\'').Trim();
}
