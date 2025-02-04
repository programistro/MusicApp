namespace MusicApp;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var result = await FilePicker.PickAsync(new PickOptions
		{
			PickerTitle = "�������� ����",
		});

		if (result == null)
		{
			return;
		}

		var stream = await result.OpenReadAsync();


    }
}