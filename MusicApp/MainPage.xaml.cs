using System.Text.Json;

namespace MusicApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private List<string> list = new List<string>();

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7218");

            var response = await client.GetAsync("api/get-all-files");
            response.EnsureSuccessStatusCode();

            var filesJson = await response.Content.ReadAsStringAsync();
            list = JsonSerializer.Deserialize<List<string>>(filesJson);

            list1.ItemsSource = list;
        }
    }
}
