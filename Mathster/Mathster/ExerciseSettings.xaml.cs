﻿using System;
using System.Linq;
using Mathster.Resources.Localization;
using Sterform.Mathster.Exercise;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mathster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseSettings : ContentPage
    {
        private const int ExSizeMax = 20;
        private const int MulDivSizeMax = 3;
        private const int NumSizeMax = 6;

        //  Default values
        private int exSize = 5;
        private byte exType;
        private int mulDivSize = 2;
        private int numSize = 2;

        public ExerciseSettings(byte exType)
        {
            InitializeComponent();

            this.exType = exType;
            NextButton.Text = Localization.Next;
            ExStaticLabel.Text = Localization.SelectExercisesAmount;
            NumSizeStaticLabel.Text = Localization.SelectNumSize;
            MulDivStaticLabel.Text = Localization.SelectDivMulSize;
            MenuToolbarButton.IconImageSource = "menu_icon.png";

            ExCountSlider.Value = exSize;
            ExCountSlider.Maximum = ExSizeMax;
            ExCountSlider.Minimum = 1;
            ExCountLabel.Text = exSize.ToString();

            NumSizeSlider.Value = numSize;
            NumSizeSlider.Maximum = NumSizeMax;
            NumSizeSlider.Minimum = 1;
            NumSizeCountLabel.Text = numSize.ToString();

            MulDivSlider.Value = mulDivSize;
            MulDivSlider.Maximum = MulDivSizeMax;
            MulDivSlider.Minimum = 1;
            MulDivSlider.IsEnabled = false;
            EquationSelectFrame.IsVisible = false;

            RadioButton1.Content = Localization.LinearEquation;
            RadioButton2.Content = Localization.QuadraticEquation;
            RadioButton3.Content = Localization.CompleteTheSquare;

            switch (exType)
            {
                case 0:
                    Title = Localization.Random;
                    break;

                case 1:
                    Title = Localization.Addition;
                    MulDivFrame.IsVisible = false;
                    break;

                case 2:
                    Title = Localization.Subtraction;
                    MulDivFrame.IsVisible = false;
                    break;

                case 3:
                    Title = Localization.Multiplication;
                    break;

                case 4:
                    Title = Localization.Division;
                    break;

                default:
                    Title = Localization.Equation;
                    EquationSelectFrame.IsVisible = true;
                    NumSizeFrame.IsVisible = false;
                    MulDivFrame.IsVisible = false;
                    break;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var settings = await App.Database.GetSettings();
            BackgroundColor = Color.FromHex(settings.BackgroundHex);
            Frame1.BackgroundColor = Color.FromHex(settings.BackgroundHex);
            Frame2.BackgroundColor = Color.FromHex(settings.BackgroundHex);
            Frame3.BackgroundColor = Color.FromHex(settings.BackgroundHex);
            if (settings.DarkMode)
            {
                ExCountLabel.TextColor = Color.White;
                NumSizeCountLabel.TextColor = Color.White;
                MulDivCountLabel.TextColor = Color.White;
            }
        }

        private async void MenuButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages) Navigation.RemovePage(page);
        }

        private void ExCountSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            exSize = (int)ExCountSlider.Value;
            ExCountLabel.Text = exSize.ToString();
        }

        private void NumSizeSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            numSize = (int)NumSizeSlider.Value;
            if (numSize == 1)
            {
                MulDivSlider.Value = 1;
                MulDivSlider.IsEnabled = false;
            }
            else
            {
                MulDivSlider.IsEnabled = true;
            }

            NumSizeCountLabel.Text = numSize.ToString();
        }

        private void MulDivSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            mulDivSize = (int)MulDivSlider.Value;
            switch (mulDivSize)
            {
                case 1:
                    MulDivCountLabel.Text = 5.ToString();
                    break;
                case 2:
                    MulDivCountLabel.Text = 10.ToString();
                    break;
                case 3:
                    MulDivCountLabel.Text = 20.ToString();
                    break;
            }
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            var queue = new Exercise[(int)ExCountSlider.Value];


            if (exType >= 5)
            {
                // Gets type of the equation
                foreach (var radioButton in RadioGroup.Children)
                {
                    var rb = (RadioButton)radioButton;
                    if (rb.IsChecked) exType = byte.Parse(rb.Value.ToString());
                }

                for (byte i = 0; i < (int)ExCountSlider.Value; i++)
                    queue[i] = new Equation().GenerateExercise(i, exType);
            }
            else
            {
                int numMin;
                int numMax;
                switch ((int)NumSizeSlider.Value)
                {
                    case 1:
                        numMin = 1;
                        numMax = 10;
                        break;

                    case 2:
                        numMin = 10;
                        numMax = 100;
                        break;

                    case 3:
                        numMin = 100;
                        numMax = 1000;
                        break;

                    case 4:
                        numMin = 1000;
                        numMax = 10000;
                        break;

                    case 5:
                        numMin = 10000;
                        numMax = 100000;
                        break;

                    case 6:
                        numMin = 100000;
                        numMax = 1000000;
                        break;

                    default:
                        numMin = 1;
                        numMax = 10;
                        break;
                }

                byte mulDivMax;
                byte mulDivMin;
                switch ((int)MulDivSlider.Value)
                {
                    case 1:
                        mulDivMin = 2;
                        mulDivMax = 6;
                        break;

                    case 2:
                        mulDivMin = 2;
                        mulDivMax = 11;
                        break;

                    case 3:
                        mulDivMin = 2;
                        mulDivMax = 21;
                        break;

                    default:
                        mulDivMin = 2;
                        mulDivMax = 6;
                        break;
                }

                for (byte i = 0; i < (int)ExCountSlider.Value; i++)
                    queue[i] = new BasicExercise().GenerateExercise(i, numMin, numMax, exType, mulDivMin, mulDivMax);
            }

            await Navigation.PushAsync(new ExerciseCounting(0, queue));
        }
    }
}