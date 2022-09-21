﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using XamarinDeleteItem.Models;
using XamarinDeleteItem.ViewModels;
using XamarinDeleteItem.Views;

namespace XamarinDeleteItem.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemsViewModel();
        }

      

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;

            var i = item.BindingContext as Item;

            _viewModel.Items.Remove(i);
            _viewModel.DataStore.DeleteItemAsync(i.Id);
        }
    }
}