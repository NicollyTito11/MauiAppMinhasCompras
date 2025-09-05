using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDataBaseHelper
    {
        readonly SQLiteAsyncConnection _conn;
        public SQLiteDataBaseHelper(string path) 
        { 
            _conn = new SQLiteAsyncConnection(path);
            _conn. CreateTableAsync<Produto>().Wait ();
        }


        public Task<int> Insert(Produto p) 
        {
            return _conn.InsertAsync (p);
        }


        public Task<List<Produto>> Update(Produto p) 
        {
            string sql = "UPTADE produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            return _conn.QueryAsync<Produto> (
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
                
                );
        }

        public Task<int> Delete(int id) 
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> Getall() 
        {
           return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * FROM produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        internal async Task<List<Produto>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
