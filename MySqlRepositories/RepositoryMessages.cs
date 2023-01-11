using Microsoft.EntityFrameworkCore.Update.Internal;

namespace MySqlReposytories
{
    internal struct RepositoryMessages
    {
        public const string CreateArticl = "Create new article #{0}";
        public const string ReadArticl = "Read article #{0}";
        public const string ReadCollectionArticl = "$Read collection article";
        public const string UpdateArticl = "Update article #{0}";
        public const string DeleteArticl = "Delete article #{0}";
        public const string AddCollectionArticl = $"Add collection article";

        public const string CreateDocument = "Create new document #{0}";
        public const string ReadDocument = "Read document #{0}";
        public const string ReadCollectionDocument = "Read collection document";
        public const string UpdateDocument = "Update document #{0}";
        public const string DeleteDocument = "Delete document #{0}";
    }
}
