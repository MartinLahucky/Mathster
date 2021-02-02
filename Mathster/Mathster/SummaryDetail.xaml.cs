﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mathster.Resources.Exercises;
using Mathster.Resources.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mathster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryDetail : ContentPage
    {
        private byte id;
        private Exercise[] queue;
        private List<Exercise> list;
        private bool transaction;


        public SummaryDetail(byte id, Exercise[] queue, bool transaction, List<Exercise> list = null)
        {
            InitializeComponent();
            MenuButton.IconImageSource = "menu_icon.png";
            TimeStaticLabel.Text = Localization.CountTime;
            AssignmentStaticLabel.Text = Localization.Assignment;
            CorrectStaticLabel.Text = Localization.SolutionCorrect;
            WrongStaticLabel.Text = Localization.YourSolution;
            
            Exercise exercise;


            if (list != null)
            {
                exercise = list[id];
                this.list = list;
                if (id == list.Count - 1)
                {
                    NextButton.IsVisible = false;
                }
            }
            else
            {
                exercise = queue[id];
                if (id == queue.Length - 1)
                {
                    NextButton.IsVisible = false;
                }
            }

            long sec = exercise.CountLenght / TimeSpan.TicksPerSecond, min = sec / 60;
            sec = sec - min * 60;
            TimeLabel.Text = $"{min}min {sec}s";

            if (exercise.Assignment.Length >= 13)
            {
                WrongLabel.HeightRequest = 75;
                CorrectLabel.HeightRequest = 75;
            }

            AssignmentLabel.Text =
                $"{exercise.Assignment}";
            WrongLabel.Text = $"{exercise.UserInput}";
            CorrectLabel.Text = $"{exercise.Result}";
            if (exercise.UserInput == exercise.Result)
            {
                WrongStaticLabel.IsVisible = false;
                WrongFrame.IsVisible = false;
            }

            this.queue = queue;
            this.transaction = transaction;
            this.id = id;

            if (id == 0)
            {
                PreviousButton.IsVisible = false;
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

        private async void PreviousButton_OnClicked(object sender, EventArgs e)
        {
            id--;
            await Navigation.PushAsync(new SummaryDetail(id, queue, transaction, list));
        }

        private async void SummaryButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Summary(queue, transaction));
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            id++;
            await Navigation.PushAsync(new SummaryDetail(id, queue, transaction, list));
        }
    }
}