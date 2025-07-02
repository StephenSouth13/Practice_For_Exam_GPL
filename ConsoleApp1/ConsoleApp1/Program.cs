using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database;
using Firebase.Database.Query;

namespace Thuc_Hanh_Lab_11
{
    public class Player
    {
        public string PlayerID { get; set; }
        public string Name { get; set; }
        public int Gold { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public bool IsActive { get; set; }
        public int VipLevel { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public DateTime LastLogin { get; set; }
    }

    public class RankedPlayer
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int Gold { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public int VipLevel { get; set; }
        public DateTime LastLogin { get; set; }
    }

    internal class Program
    {
        static FirebaseClient firebase = new FirebaseClient("https://lab11-a162f-default-rtdb.asia-southeast1.firebasedatabase.app/");
        static List<Player> playerList = new List<Player>();
        static Random rand = new Random();

        static async Task<List<Player>> GetPlayersFromFirebaseAsync()
        {
            var data = await firebase.Child("Players").OnceAsync<Player>();
            List<Player> result = new List<Player>();
            foreach (var item in data)
            {
                if (item.Object != null)
                    result.Add(item.Object);
            }
            return result;
        }

        static async Task PushPlayersToFirebaseWithIndexAsync(List<Player> players)
        {
            await firebase.Child("Players").DeleteAsync();
            int index = 1;
            foreach (var p in players)
            {
                await firebase.Child("Players").Child(index.ToString()).PutAsync(p);
                index++;
            }
        }

        static async Task AddRandomPlayers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int randomId = rand.Next(1, 101);
                while (playerList.Any(x => x.PlayerID == randomId.ToString()))
                {
                    randomId = rand.Next(1, 101);
                }
                var p = new Player();
                p.PlayerID = randomId.ToString();
                p.Name = "Player" + rand.Next(1000, 9999);
                p.Gold = rand.Next(0, 2000);
                p.Score = rand.Next(0, 1000);
                p.Coins = rand.Next(0, 300);
                p.IsActive = true;
                p.VipLevel = rand.Next(0, 5);
                p.Region = new string[] { "Asia", "Europe", "America" }[rand.Next(3)];
                p.City = "City" + rand.Next(1, 100);
                p.LastLogin = DateTime.Now.AddDays(-rand.Next(0, 5));
                playerList.Add(p);
            }
            await PushPlayersToFirebaseWithIndexAsync(playerList);
            Console.WriteLine("Đã thêm " + count + " người chơi ngẫu nhiên.");
        }

        static async Task AddManualPlayer()
        {
            var p = new Player();
            Console.Write("PlayerID (1-100): "); p.PlayerID = Console.ReadLine();
            Console.Write("Name: "); p.Name = Console.ReadLine();
            Console.Write("Gold: "); p.Gold = int.Parse(Console.ReadLine());
            Console.Write("Score: "); p.Score = int.Parse(Console.ReadLine());
            Console.Write("Coins: "); p.Coins = int.Parse(Console.ReadLine());
            Console.Write("VipLevel: "); p.VipLevel = int.Parse(Console.ReadLine());
            Console.Write("Region: "); p.Region = Console.ReadLine();
            p.IsActive = true;
            p.City = "Manual";
            p.LastLogin = DateTime.Now;
            playerList.Add(p);
            await PushPlayersToFirebaseWithIndexAsync(playerList);
            Console.WriteLine("Đã thêm người chơi thủ công.");
        }

        static async Task ShowAllPlayers()
        {
            var list = await GetPlayersFromFirebaseAsync();
            Console.WriteLine("Danh sách người chơi:");
            int i = 1;
            foreach (var p in list)
            {
                Console.WriteLine(i + ". " + p.PlayerID + " - " + p.Name + " | Gold: " + p.Gold + " | Score: " + p.Score);
                i++;
            }
        }

        static async Task UpdatePlayer()
        {
            Console.Write("Nhập PlayerID cần cập nhật: ");
            string id = Console.ReadLine();
            var allPlayers = await GetPlayersFromFirebaseAsync();
            var player = allPlayers.FirstOrDefault(p => p.PlayerID == id);
            if (player == null) { Console.WriteLine("Không tìm thấy."); return; }

            Console.Write("Gold mới: ");
            player.Gold = int.Parse(Console.ReadLine());
            Console.Write("Score mới: ");
            player.Score = int.Parse(Console.ReadLine());

            await PushPlayersToFirebaseWithIndexAsync(allPlayers);
            Console.WriteLine("Cập nhật thành công.");
        }

        static async Task DeletePlayer()
        {
            Console.Write("Nhập PlayerID cần xoá: ");
            string id = Console.ReadLine();
            var allPlayers = await GetPlayersFromFirebaseAsync();
            var updatedList = allPlayers.Where(p => p.PlayerID != id).ToList();
            await PushPlayersToFirebaseWithIndexAsync(updatedList);
            Console.WriteLine("Xoá thành công.");
        }

        static async Task ShowTop5GoldAsync()
        {
            var list = await GetPlayersFromFirebaseAsync();
            var top = list.OrderByDescending(p => p.Gold).Take(5).ToList();
            int rank = 1;
            foreach (var p in top)
            {
                Console.WriteLine("Top " + rank + ": " + p.Name + " - Gold: " + p.Gold);
                rank++;
            }

            var result = new Dictionary<string, object>();
            int index = 1;
            foreach (var p in top)
            {
                result[index.ToString()] = new
                {
                    Index = index,
                    p.Name,
                    p.Gold,
                    p.Score,
                    p.Coins,
                    p.VipLevel,
                    p.LastLogin
                };
                index++;
            }
            await firebase.Child("TopGold").PutAsync(result);
        }

        static async Task ShowTop5ScoreAsync()
        {
            var list = await GetPlayersFromFirebaseAsync();
            var top = list.OrderByDescending(p => p.Score).Take(5).ToList();
            int rank = 1;
            foreach (var p in top)
            {
                Console.WriteLine("Top " + rank + ": " + p.Name + " - Score: " + p.Score);
                rank++;
            }

            var result = new Dictionary<string, object>();
            int index = 1;
            foreach (var p in top)
            {
                result[index.ToString()] = new
                {
                    Index = index,
                    p.Name,
                    p.Gold,
                    p.Score,
                    p.Coins,
                    p.VipLevel,
                    p.LastLogin
                };
                index++;
            }
            await firebase.Child("TopScore").PutAsync(result);
        }

        static async Task LinqBai1()
        {
            var list = await GetPlayersFromFirebaseAsync();
            var result = list.Where(p => p.Gold > 1000 && p.Coins > 100)
                             .OrderByDescending(p => p.Gold)
                             .Select(p => new { p.Name, p.Gold, p.Coins })
                             .ToList();

            foreach (var p in result)
                Console.WriteLine(p.Name + " | Gold: " + p.Gold + " | Coins: " + p.Coins);

            var pushData = new Dictionary<string, object>();
            int i = 1;
            foreach (var p in result)
            {
                pushData[i.ToString()] = new
                {
                    Index = i,
                    p.Name,
                    p.Gold,
                    p.Coins
                };
                i++;
            }
            await firebase.Child("quiz_bai1_richPlayers").PutAsync(pushData);
        }

        static async Task LinqBai2()
        {
            var list = await GetPlayersFromFirebaseAsync();
            Console.WriteLine("Tổng số người chơi VIP: " + list.Count(p => p.VipLevel > 0));

            var group = list.Where(p => p.VipLevel > 0).GroupBy(p => p.Region)
                            .Select(g => new { Region = g.Key, Count = g.Count() });
            foreach (var g in group)
                Console.WriteLine(g.Region + ": " + g.Count + " người VIP");

            var now = new DateTime(2025, 06, 30);
            var recent = list.Where(p => p.VipLevel > 0 && (now - p.LastLogin).TotalDays <= 2)
                             .Select(p => new { p.Name, p.VipLevel, p.LastLogin })
                             .ToList();

            foreach (var p in recent)
                Console.WriteLine(p.Name + " | VIP: " + p.VipLevel + " | LastLogin: " + p.LastLogin);

            var pushData = new Dictionary<string, object>();
            int i = 1;
            foreach (var p in recent)
            {
                pushData[i.ToString()] = new
                {
                    Index = i,
                    p.Name,
                    p.VipLevel,
                    p.LastLogin
                };
                i++;
            }
            await firebase.Child("quiz_bai2_recentVipPlayers").PutAsync(pushData);
        }

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.WriteLine("\n======= MENU =======");
                Console.WriteLine("1. Thêm 7 người chơi ngẫu nhiên");
                Console.WriteLine("2. Thêm 5 người chơi ngẫu nhiên");
                Console.WriteLine("3. Nhập tay 1 người chơi");
                Console.WriteLine("4. Hiển thị toàn bộ danh sách người chơi");
                Console.WriteLine("5. Cập nhật Gold hoặc Score theo PlayerID");
                Console.WriteLine("6. Xoá người chơi theo PlayerID");
                Console.WriteLine("7. Bài 1: Phân tích tài chính người chơi");
                Console.WriteLine("8. Bài 2: Thống kê người chơi VIP");
                Console.WriteLine("9. Hiển thị Top 5 Gold");
                Console.WriteLine("10. Hiển thị Top 5 Score");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn: ");

                string chon = Console.ReadLine();
                switch (chon)
                {
                    case "1": await AddRandomPlayers(7); await ShowAllPlayers(); break;
                    case "2": await AddRandomPlayers(5); await ShowAllPlayers(); break;
                    case "3": await AddManualPlayer(); await ShowAllPlayers(); break;
                    case "4": await ShowAllPlayers(); break;
                    case "5": await UpdatePlayer(); await ShowAllPlayers(); break;
                    case "6": await DeletePlayer(); await ShowAllPlayers(); break;
                    case "7": await LinqBai1(); break;
                    case "8": await LinqBai2(); break;
                    case "9": await ShowTop5GoldAsync(); break;
                    case "10": await ShowTop5ScoreAsync(); break;
                    case "0": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ."); break;
                }
            }
        }
    }
}
