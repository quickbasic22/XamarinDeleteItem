using System.ComponentModel;
using Xamarin.Forms;
using XamarinDeleteItem.ViewModels;

namespace XamarinDeleteItem.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}