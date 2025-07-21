using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUapp.Core.ChatbotManagement.DTOs.TrainingSource;

public class UpdateTrainingSourceDto
{
    public string Name { get; set; } = default!;
    public string TextContent { get; set; } = default!;
}
