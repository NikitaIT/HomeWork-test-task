using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskApp.Domain.Core;

namespace Interfaces
{
    public interface IArticleRepository : IDisposable
    {
        IEnumerable<Article> GetBookList();
        Article GetBook(int id);
        void Create(Article item);
        void Update(Article item);
        void Delete(int id);
        void Save();
    }
}
