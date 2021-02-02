﻿using System;
using System.Linq;
using Mathster.Resources.Exercises;
using Mathster.Resources.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mathster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseSettings : ContentPage
    {
        private byte ExType;

        //  Default values
        private int ExSize = 5;
        private int ExSizeMax = 20;


        private int NumSize = 2;
        private int NumSizeMax = 6;

        private int MulDivSize = 2;
        private int MulDivSizeMax = 3;

        public ExerciseSettings(byte exType)
        {
            InitializeComponent();

            this.ExType = exType;
            NextButton.Text = Localization.Next;
            ExStaticLabel.Text = Localization.SelectExercisesAmount;
            NumSizeStaticLabel.Text = Localization.SelectNumSize;
            MulDivStaticLabel.Text = Localization.SelectDivMulSize;
            MenuToolbarButton.IconImageSource = "menu_icon.png";

            ExCountSlider.Value = ExSize;
            ExCountSlider.Maximum = ExSizeMax;
            ExCountSlider.Minimum = 1;
            ExCountLabel.Text = ExSize.ToString();

            NumSizeSlider.Value = NumSize;
            NumSizeSlider.Maximum = NumSizeMax;
            NumSizeSlider.Minimum = 1;
            NumSizeCountLabel.Text = NumSize.ToString();

            MulDivSlider.Value = MulDivSize;
            MulDivSlider.Maximum = MulDivSizeMax;
            MulDivSlider.Minimum = 1;
            MulDivSlider.IsEnabled = false;


            switch (exType)
            {
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

                case 5:
                    Title = Localization.Random;
                    break;
                
                case 6:
                    Title = Localization.Equation;
                    NumSizeFrame.IsVisible = false;
                    MulDivFrame.IsVisible = false;
                    break;
                
            }
        }

        private async void MenuButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
                Navigation.RemovePage(page);
            }
        }

        private void ExCountSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            ExSize = (int) ExCountSlider.Value;
            ExCountLabel.Text = ExSize.ToString();
        }

        private void NumSizeSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            NumSize = (int) NumSizeSlider.Value;
            if (NumSize == 1)
            {
                MulDivSlider.Value = 1;
                MulDivSlider.IsEnabled = false;
            }
            else
            {
                MulDivSlider.IsEnabled = true;
            }

            NumSizeCountLabel.Text = NumSize.ToString();
        }

        private void MulDivSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            MulDivSize = (int) MulDivSlider.Value;
            switch (MulDivSize)
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
            int numMin, numMax;
            byte mulDivMin, mulDivMax;

            switch ((int) NumSizeSlider.Value)
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

            switch ((int) MulDivSlider.Value)
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


            Exercise[] queue = new Exercise[(int) ExCountSlider.Value];

            for (byte i = 0; i < (int) ExCountSlider.Value; i++)
            {
                queue[i] = new BasicExercise().GenerateExercise(i, numMin, numMax, ExType, mulDivMin, mulDivMax);
            }
            
            await Navigation.PushAsync(new ExerciseCounting(0, queue));
        }
    }
}