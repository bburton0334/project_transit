using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace project_transit.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        static string retUsername = "";
        static string retPassword = "";

        private void btnEnter_Clicked(object sender, EventArgs e)
        {
            string username = entryUsername.ToString();
            string password = entryPassword.ToString();

            if (username == null || password == null)
            {
                DisplayAlert("Error", "Username and Password cannot be blank.", "Okay");
            }
            else
            {
                connectToDB(username, password);
                if (retUsername == null || retPassword == null)
                {
                    DisplayAlert("Warning", "login match not found.", "Okay");
                }
                else
                {
                    DisplayAlert("Status", "Match Found.", "Okay");
                }
            }
        }

        private void connectToDB(string username, string password)
        {
            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            connStringBuilder.Host = "free-tier14.aws-us-east-1.cockroachlabs.cloud";
            connStringBuilder.Port = 26257;
            connStringBuilder.SslMode = SslMode.Require;
            connStringBuilder.Username = "briana";
            connStringBuilder.Password = "8I5tmUaHgv96GE7EsGgoIA";
            connStringBuilder.Database = "hood-test-database-3180.defaultdb";
            connStringBuilder.RootCertificate = "C:/Users/diver/AppData/Roaming/postgresql/root.crt";
            connStringBuilder.TrustServerCertificate = true;
            Simple(connStringBuilder.ConnectionString, username, password);
        }

        static void Simple(string connString, string username, string password)
        {

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // getting password username match


                using (var cmd = new NpgsqlCommand("SELECT * FROM credentials WHERE username='" + username + "' AND password='" + password + "'", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        retUsername = reader.GetValue(1).ToString();
                        retPassword = reader.GetValue(2).ToString();
                    }
            }
        }
    }
}