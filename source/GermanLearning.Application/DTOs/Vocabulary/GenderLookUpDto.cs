using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.Application.DTOs.Vocabulary
{
    public class GenderLookUpDto
    {
        public int Id { get; set; } // Corresponds to Gender enum int value
        public string Name { get; set; } = string.Empty;
    }
}
