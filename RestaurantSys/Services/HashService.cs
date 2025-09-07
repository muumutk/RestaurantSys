using System.Security.Cryptography;
using System.Text;

namespace RestaurantSys.Services
{
    public class HashService
    {
        public static string HashPasswordSHA256(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // 計算雜湊值
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));

                // 將 byte 陣列轉換成十六進位字串
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
