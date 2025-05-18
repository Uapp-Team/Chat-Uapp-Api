using System.Threading.Tasks;

namespace ChatUapp.Data;

public interface IChatUappDbSchemaMigrator
{
    Task MigrateAsync();
}
