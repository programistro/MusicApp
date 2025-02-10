namespace MusicApp;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        //var result = await FilePicker.PickAsync(new PickOptions
        //{
        //    PickerTitle = "Выберите файл",
        //});

        //if (result == null)
        //{
        //    return;
        //}

        //var stream = await result.OpenReadAsync();

        //var content = new MultipartFormDataContent();

        //content.Add(new StreamContent(stream), "file", result.FileName);

        //var httpClient = new HttpClient();
        //string serverUrl = "https://localhost:7218/api/upload-file";

        //var response = await httpClient.PostAsync(serverUrl, content);

        //// Проверяем результат
        //if (response.IsSuccessStatusCode)
        //{
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    //await DisplayAlert("Успех!", "Файл успешно загружен", "ОК");
        //}
        //else
        //{
        //    //await DisplayAlert("Ошибка", "Не удалось загрузить файл", "ОК");
        //}
    }
}