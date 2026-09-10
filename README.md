# 遊戲設計作業 Repo（3D）

> 115-1 遊戲設計課程作業模板（第 6–7 週：3D 彈射、接蘋果）。收到這個 repo 後，請照下面步驟開發與繳交。

## 繳交規則

1. **從第一行程式就在這個 repo 裡開發**，做一個小功能就 commit 一次並 push。
2. **commit 歷程是評分依據之一**：只有一筆 commit 的作業會被質疑非本人逐步完成。
3. 截止日前的最後一次 push 即為繳交，不需要其他動作。
4. 作業需附 **WebGL 可玩網址**（見下方部署步驟）。

## 開發流程

1. 用 GitHub Desktop 把這個 repo **clone** 到自己電腦
2. 用 Unity Hub 開啟 clone 下來的資料夾（Unity 6 LTS，全班統一版本）
3. 開發 → commit → push，重複到完成

## WebGL 部署步驟（取得可玩網址）

1. Unity 選單 **File → Build Profiles**，平台選 **Web (WebGL)**
2. **Player Settings → Publishing Settings → Compression Format 改為 Disabled**
   > 🚨 **每次新增 Unity 專案這個設定都可能被重設，務必每次檢查。** 沒改對的話，GitHub Pages 上會卡在讀取條不動，或跳出紅色錯誤框「`Unable to parse Build/build.framework.js.br`」。看到這個狀況，回這裡改成 Disabled、重新 Build、重新 push 即可。
3. 點 **Build**，輸出資料夾選 repo 裡的 **`docs`** 資料夾（沒有就新建一個，名稱必須是 docs）
4. commit 並 push（docs 資料夾一起上傳）
5. 你的遊戲網址：`https://pccu-gamedesign.github.io/<repo名稱>/`
   （Pages 已由老師端設定好，push 後等 1–2 分鐘生效）
6. 把網址貼到本 README 最下方的「作業資訊」欄位，再 commit push 一次

## 常見問題

- **push 失敗說檔案太大**：單檔上限 100MB。檢查是不是把 Library/ 傳上來了（不應該，.gitignore 已排除）；大素材請確認副檔名有被 .gitattributes 的 LFS 規則涵蓋。
- **網頁卡在讀取條/跳紅色錯誤框**：九成是忘了第 2 步的 Compression Format 設定，改好重 build 再 push。
- **repo 剛建好時收到「pages build 失敗」的信**：這是正常的（docs 還沒有內容），等你 push 完 WebGL 檔案會自動重跑成功，不用理它。
- **AI 工具使用**：可以用 Antigravity 等 AI 輔助，但每次讓 AI 大改前先 commit，才能回滾；期末需標註 AI 生成內容。

---

## 作業資訊（學生填寫）

- 學號：
- 姓名：
- WebGL 網址：
- 完成的功能／自我評估：
