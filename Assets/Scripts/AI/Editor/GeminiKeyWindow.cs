using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Unity 選單「AI 工具 → 設定 Gemini API Key」：
/// 貼上 key → 存到專案根目錄的 Secrets/gemini_key.txt → 馬上測試這把 key 能不能用。
/// 換電腦（教室電腦還原、回家用自己的電腦）重新 clone 後，再做一次即可。
/// </summary>
public class GeminiKeyWindow : EditorWindow
{
    string key = "";
    string status = "";
    MessageType statusType = MessageType.None;
    UnityWebRequest request;

    [MenuItem("AI 工具/設定 Gemini API Key", false, 0)]
    static void Open()
    {
        var window = GetWindow<GeminiKeyWindow>(true, "設定 Gemini API Key");
        window.minSize = new Vector2(480, 220);
        window.key = GeminiKeyProvider.GetKey();
        if (window.key.Length > 0)
        {
            window.status = "這台電腦已經存過 key。要換新的 key，貼上後再按「儲存並測試」。";
            window.statusType = MessageType.Info;
        }
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "1. 到 aistudio.google.com → Get API key，複製 AIza 開頭的 key\n" +
            "2. 貼在下面，按「儲存並測試」\n\n" +
            "key 會存在專案根目錄的 " + GeminiKeyProvider.KeyFilePath + "：\n" +
            "不會上傳 GitHub，也不會被打包進遊戲。",
            MessageType.None);

        EditorGUILayout.Space();
        key = EditorGUILayout.PasswordField("API Key", key);
        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(request != null || string.IsNullOrWhiteSpace(key)))
        {
            if (GUILayout.Button(request != null ? "測試中……" : "儲存並測試", GUILayout.Height(30)))
                SaveAndTest();
        }

        if (!string.IsNullOrEmpty(status))
            EditorGUILayout.HelpBox(status, statusType);
    }

    void SaveAndTest()
    {
        string path = GeminiKeyProvider.EditorKeyFileFullPath;
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, key.Trim() + "\n");

        // 列出模型清單來驗證 key，不會用掉生成內容的額度
        request = UnityWebRequest.Get("https://generativelanguage.googleapis.com/v1beta/models?pageSize=1");
        request.SetRequestHeader("x-goog-api-key", GeminiKeyProvider.GetKey());
        request.timeout = 20;
        request.SendWebRequest();
        EditorApplication.update += Poll;

        status = "已儲存，測試中……";
        statusType = MessageType.Info;
    }

    void Poll()
    {
        if (request == null || !request.isDone)
            return;

        EditorApplication.update -= Poll;
        long code = request.responseCode;

        if (request.result == UnityWebRequest.Result.Success)
        {
            status = "測試成功：key 可以用了。";
            statusType = MessageType.Info;
        }
        else if (code == 400 || code == 401 || code == 403)
        {
            status = "key 無效（HTTP " + code + "）：確認有複製到完整的 AIza 開頭整串，前後沒有多餘的字。";
            statusType = MessageType.Error;
        }
        else if (code == 429)
        {
            status = "請求太頻繁或今天的額度用完了（HTTP 429），等一分鐘再試。";
            statusType = MessageType.Warning;
        }
        else
        {
            status = "連不上 Google（" + request.error + "），檢查網路後再按一次。key 已經存好了。";
            statusType = MessageType.Warning;
        }

        request.Dispose();
        request = null;
        Repaint();
    }

    void OnDisable()
    {
        EditorApplication.update -= Poll;
        request?.Dispose();
        request = null;
    }
}
