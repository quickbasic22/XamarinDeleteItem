using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using XamarinDeleteItem.Models;
using XamarinDeleteItem.Views;

namespace XamarinDeleteItem.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private ObservableCollection<Item> _selectedItem;
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command DeleteSelected { get; }
        public Command<Item> DeleteCommand { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            DeleteSelected = new Command(OnDeleteSelected);
            AddItemCommand = new Command(OnAddItem);
            _selectedItem = new ObservableCollection<Item>();
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public ObservableCollection<Item> SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    SetProperty(ref _selectedItem, value);
                }
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnDeleteSelected()
        {
           if (SelectedItem == null)
            {
                return;
            }
           else
            {
                foreach (var item in SelectedItem)
                {
                    Items.Remove(item);
                   await DataStore.DeleteItemAsync(item.Id);
                }
            }
            // This will push the ItemDetailPage onto the navigation stack
           // await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        
    }
}