using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // ObservableCollection garante atualização automática da interface
    public ObservableCollection<Produto> Lista { get; set; } = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = Lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        Lista.Clear();

        List<Produto> tmp = await App.Db.Getall();
        tmp.ForEach(p => Lista.Add(p));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;
        lst_produtos.IsRefreshing = true;

        Lista.Clear();
        List<Produto> tmp = await App.Db.Search(q);
        tmp.ForEach(p => Lista.Add(p));

        lst_produtos.IsRefreshing = false;
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = Lista.Sum(i => i.Total);
        DisplayAlert("Total dos Produtos", $"O total é {soma:C}", "OK");
    }

    private async void MenuItem_Remover_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem menuItem = sender as MenuItem;
            Produto p = menuItem.BindingContext as Produto;

            bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");
            if (confirm)
            {
                await App.Db.Delete(p.Id);
                Lista.Remove(p); // Remove da ObservableCollection para atualizar UI automaticamente
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }

        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
        
    }
}
