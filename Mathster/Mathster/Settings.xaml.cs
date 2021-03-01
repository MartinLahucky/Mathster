﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Mathster.Resources.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mathster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            MenuButton.IconImageSource = "menu_icon.png";
            Title = Localization.Settings;
            NameEntry.Placeholder = Localization.EnterYourName;
            NameEntry.PlaceholderColor = Color.White;
            ;
            NameLabel.Text = Localization.Name;
            DarkModeLabel.Text = Localization.DarkMode;

            var task = Task.Run(async () =>
            {
                var table = await App.Database.GetTable();

                if (table.Name == "")
                    NameEntry.Text = string.Empty;
                else
                    NameEntry.Text = table.Name;
            });
            Task.WaitAll(task);
            AboutAppLabel.Text = $"{Localization.AppVersion}: Closed Beta 1.3";
        }

        private async void MenuButton_OnClicked(object sender, EventArgs e)
        {
            var table = await App.Database.GetTable();
            table.Name = NameEntry.Text;
            await App.Database.UpdateTable(table);
            await Navigation.PushAsync(new MainPage());
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages) Navigation.RemovePage(page);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DarkModeSwitch.IsToggled = Application.Current.UserAppTheme == OSAppTheme.Dark;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            var table = await App.Database.GetTable();
            var newName = string.Empty;
            var nameLimit = 12;
            if (NameEntry.Text.Length <= nameLimit) nameLimit = NameEntry.Text.Length;

            for (var i = 0; i < nameLimit; i++) newName += NameEntry.Text[i];

            table.Name = newName;
            await App.Database.UpdateTable(table);
        }

        private async void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var settings = await App.Database.GetSettings();
            Application.Current.UserAppTheme = e.Value ? OSAppTheme.Dark : OSAppTheme.Light;
            settings.Theme = Application.Current.UserAppTheme;
            await App.Database.UpdateSettings(settings);
        }

        private void NameEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameEntry.Text.Length == 12) NameEntry.Text = e.OldTextValue;
        }
    }
}