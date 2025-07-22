using System;
using Volo.Abp.Application.Dtos;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class GetSessionTitlesListDto : PagedAndSortedResultRequestDto
    {
        public Guid ChatbotId { get; set; }
    }
}
