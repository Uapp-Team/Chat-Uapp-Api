using ChatUapp.Core.Interfaces.Chatbot;
using Volo.Abp.Guids;

namespace ChatUapp.Infrastructure.Utility
{
    public class DomainGuidGenerator : IDomainGuidGenerator
    {
        private readonly IGuidGenerator _guidGeneratord;
        public DomainGuidGenerator(IGuidGenerator guidGeneratord)
        {
            _guidGeneratord = guidGeneratord;
        }

        public Guid Create()
        {
            return _guidGeneratord.Create();
        }
    }
}
