﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathster.Helpers.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mathster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Priklady : ContentPage
    {
        private Priklad priklad;
        private byte ID;
        private List<Priklad> fronta;
        public Priklady(byte id, List<Priklad> fronta)
        {
            InitializeComponent();
            DalsiButton.Text = AppResource.Dalsi;
            OdvezdatButton.Text = AppResource.Odevzdat;
            
            priklad = fronta[id];

            switch (priklad.DruhPrikladu)
            {
                case 1:
                    Title = AppResource.Scitani;
                    PrikladLabel.Text = $"{priklad.PrvniCislo} + {priklad.DruheCislo} =";
                    break;
                case 2:
                    Title = AppResource.Odecitani;
                    PrikladLabel.Text = $"{priklad.PrvniCislo} - {priklad.DruheCislo} =";
                    break;
                case 3:
                    Title = AppResource.Nasobeni;
                    PrikladLabel.Text = $"{priklad.PrvniCislo} X {priklad.DruheCislo} =";
                    break;
                case 4:
                    Title = AppResource.Deleni;
                    PrikladLabel.Text = $"{priklad.PrvniCislo} ÷ {priklad.DruheCislo} =";
                    break;
            }

            this.fronta = fronta;
            ID = id;
            
            if (id < (fronta.Count - 1))
            {
                OdvezdatButton.IsVisible = false;
            }
            else
            {
                DalsiButton.IsVisible = false;
            }
            
        }
        
        private async void OdvezdatButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                fronta[ID].UzivateluvVstup = int.Parse(VysledekInput.Text);
                ID = 0;
                switch (fronta[ID].DruhPrikladu)
                {
                    case 1:
                        if (fronta[0].PrvniCislo + fronta[0].DruheCislo == fronta[0].UzivateluvVstup)
                        {
                            await Navigation.PushAsync(new VysledekDobre(ID, fronta));
                        }
                        else
                        {
                            await Navigation.PushAsync(new VysledekSpatne(ID, fronta));
                        }
                        break;
                    case 2:
                        if (fronta[0].PrvniCislo - fronta[0].DruheCislo == fronta[0].UzivateluvVstup)
                        {
                            await Navigation.PushAsync(new VysledekDobre(ID, fronta));
                        }
                        else
                        {
                            await Navigation.PushAsync(new VysledekSpatne(ID, fronta));
                        }
                        break;
                    case 3:
                        if (fronta[0].PrvniCislo * fronta[0].DruheCislo == fronta[0].UzivateluvVstup)
                        {
                            await Navigation.PushAsync(new VysledekDobre(ID, fronta));
                        }
                        else
                        {
                            await Navigation.PushAsync(new VysledekSpatne(ID, fronta));
                        }
                        break;
                    case 4:
                        if (fronta[0].PrvniCislo / fronta[0].DruheCislo == fronta[0].UzivateluvVstup)
                        {
                            await Navigation.PushAsync(new VysledekDobre(ID, fronta));
                        }
                        else
                        {
                            await Navigation.PushAsync(new VysledekSpatne(ID, fronta));
                        }
                        break;
                    }
                var existingPages = Navigation.NavigationStack.ToList();
                foreach(var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert(AppResource.Upozorneni, AppResource.UpozorneniZadejteCislo, AppResource.Ok);
            }
        }

        private async void DalsiButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                fronta[ID].UzivateluvVstup = int.Parse(VysledekInput.Text);
                ID++;
                await Navigation.PushAsync(new Priklady(ID, fronta));
                var existingPages = Navigation.NavigationStack.ToList();
                foreach(var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert(AppResource.Upozorneni, AppResource.UpozorneniZadejteCislo, AppResource.Ok);
            }
        }
        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            VysledekInput.Focus();
        }
    }
}