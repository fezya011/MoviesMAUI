using FitnessMAUI.db;
using FitnessMAUI.Model;

namespace FitnessMAUI;

public partial class AddStudioPage : ContentPage
{
    private Studio addStudio;
    private DB dB;

    public Studio AddStudio 
    { 
        get => addStudio;
        set
        {
            addStudio = value;
            OnPropertyChanged();
        }
    }

    public AddStudioPage(DB dB)
	{
        this.dB = dB;
        InitializeComponent();
        AddStudio = new Studio();
        BindingContext = this;
	}

    private void OnSliderValue(object sender, ValueChangedEventArgs e)
    {

        header.Text = $"Выбрано: {e.NewValue:F1}";
    }

    private async void SaveClick(object sender, EventArgs e)
    {
        try
        {
            AddStudio.Rating = Math.Round(AddStudio.Rating, 1);

            await dB.AddStudioAsync(AddStudio);

            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить фильм: {ex.Message}", "OK");
        }
    }
}