using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        public Command NextPageCommand { get; set; }
        public Command PreviousPageCommand { get; set; }
        private const int thepagesize = 3;
        private const int thepagenumber = 1;
        private int pagesize;
        private int pagenumber;

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            DeleteSelected = new Command(OnDeleteSelected);
            AddItemCommand = new Command(OnAddItem);
            NextPageCommand = new Command(NextPage);
            PreviousPageCommand = new Command(PreviousPage);
            _selectedItem = new ObservableCollection<Item>();
            PageSize = thepagesize;
            PageNumber = thepagenumber;
        }
                
        public int PageSize
        {
            get => pagesize;
            set
            {
                if (pagesize != value)
                SetProperty(ref pagesize, value);
            }
        }

        public int PageNumber
        {
            get => pagenumber;
            set
            {
                if (pagenumber != value)
                    SetProperty(ref pagenumber, value);
            }
        }

        public void NextPage()
        {
            PageNumber++;
            if (PageNumber > thepagesize)
                PageNumber = thepagesize;
           
        }

        public void PreviousPage()
        {
            PageNumber--;
            if (PageNumber < 0)
                PageNumber = 0;
            
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);

                items = items.Take(PageSize);
                
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