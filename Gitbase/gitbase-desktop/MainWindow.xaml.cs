using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace gitbase_desktop {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var lBytes = Encoding.UTF8.GetBytes("1380");
            var rBytes = Encoding.UTF8.GetBytes("0908");
            var lMD5Hash = MD5.Create().ComputeHash(lBytes);
            var rMD5Hash = MD5.Create().ComputeHash(rBytes);
            Console.WriteLine("Пароль для пользователя Postgres: {0}{1}",
                Convert.ToBase64String(lMD5Hash), 
                Convert.ToBase64String(rMD5Hash)
            );
            var lSHA512Hash = SHA512.Create().ComputeHash(lBytes);
            var rSHA512Hash = SHA512.Create().ComputeHash(rBytes);
            Console.WriteLine("Ключ jwt-токенов: {0}{1}",
                Convert.ToBase64String(lSHA512Hash),
                Convert.ToBase64String(rSHA512Hash)
            );
            var SHA256Hash = SHA256.Create().ComputeHash(lBytes);
            Console.WriteLine("Пароль администратора: {0}",
                Convert.ToBase64String(SHA256Hash)
            );
        }
    }
}
