﻿using Mathster.Resources.Database_Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Mathster
{
    public partial class App : Application
    {
        private static DatabaseController _database;
        public static string DatabaseLocation = string.Empty;

        public App(string databaseLocation)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarTextColor = Color.FromHex("#C9FF50")
            };
            DatabaseLocation = databaseLocation;
        }

        public static DatabaseController Database
        {
            get
            {
                if (_database == null) _database = new DatabaseController();

                return _database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            // If the app sleeps, every 24 hours from last usage will send reminding notification  
            // DependencyService.Get<INotificationManager>().StartService(Localization.AlertPractice,
            //     Localization.AlertPracticeText, DateTime.Now.AddSeconds(10));
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}