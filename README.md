# Firebase LINQ Console App

✨ Ứng dụng Console C# thao tác với Firebase Realtime Database, tương thích hoàn toàn với **C# 7.3**. Hỗ trợ nhập liệu, CRUD, phân tích dữ liệu LINQ và lưu kết quả lên Firebase.

---

## 📂 Công nghệ sử dụng

* .NET Framework / .NET Core (C# 7.3)
* Firebase Realtime Database
* Newtonsoft.Json
* FirebaseDatabase.net (client SDK - KHÔNG cần Admin SDK)

---

## ⚖️ Cài đặt

```bash
Install-Package FirebaseDatabase.net
Install-Package Newtonsoft.Json
```

> Firebase URL được dùng:

```
https://lab11-a162f-default-rtdb.asia-southeast1.firebasedatabase.app/
```

---

## 🚀 Tính năng

### CRUD danh sách người chơi

| STT | Chức năng                              |
| --- | -------------------------------------- |
| 1   | Thêm **7 người chơi ngẫu nhiên**       |
| 2   | Thêm **5 người chơi ngẫu nhiên**       |
| 3   | Nhập **1 người chơi thủ công**         |
| 4   | Hiển thị danh sách người chơi          |
| 5   | Cập nhật Gold hoặc Score theo PlayerID |
| 6   | Xóa người chơi theo PlayerID           |

### Bài tập LINQ

| STT | Bài tập                                              | Firebase Node                 |
| --- | ---------------------------------------------------- | ----------------------------- |
| 7   | Bài 1: Phân tích tài chính người chơi (Gold + Coins) | `/quiz_bai1_richPlayers`      |
| 8   | Bài 2: Thống kê người chơi VIP                       | `/quiz_bai2_recentVipPlayers` |

### Xếp hạng

| STT | Bài tập                     | Firebase Node |
| --- | --------------------------- | ------------- |
| 9   | Top 5 người chơi theo Gold  | `/TopGold`    |
| 10  | Top 5 người chơi theo Score | `/TopScore`   |

---

## 📆 Cách chạy

Sau khi build xong, khi chạy chương trình sẽ hiển menu:

```
======= MENU =======
1. Thêm 7 người chơi ngẫu nhiên
2. Thêm 5 người chơi ngẫu nhiên
3. Nhập tay 1 người chơi
4. Hiển thị toàn bộ danh sách người chơi
5. Cập nhật Gold hoặc Score theo PlayerID
6. Xóa người chơi theo PlayerID
7. Bài 1: Phân tích tài chính người chơi
8. Bài 2: Thống kê người chơi VIP
9. Hiển thị Top 5 Gold
10. Hiển thị Top 5 Score
0. Thoát
```

---

## 📊 Ghi chú

* Mệnh danh `PlayerID` được giới hạn từ `1-100` để dễ quản lý
* Tất cả danh sách đầy Firebase đều **bắt đầu từ key 1, 2, 3...**
* Hoàn toàn tương thích với **C# 7.3** (KHÔNG dùng `new()` hay `target-typed object creation`)
* Dữ liệu ban đầu có thể lấy từ URL:
  [`https://github.com/NTH-VTC/OnlineDemoC-/blob/main/simple_players.json`](https://github.com/NTH-VTC/OnlineDemoC-/blob/main/simple_players.json)

---

## 🚀 Tác giả

* Dự án tự luyện C# LINQ + Firebase cho sinh viên
* Phù hợp cho bài giữa kỳ, lab thực hành hoặc bài kiểm tra Firebase
