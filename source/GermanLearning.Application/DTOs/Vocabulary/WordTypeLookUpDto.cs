using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.Application.DTOs.Vocabulary
{
    public class WordTypeLookUpDto
    {
        public int Id { get; set; } // Corresponds to WordType enum int value
        public string Name { get; set; } = string.Empty;
    }
}
