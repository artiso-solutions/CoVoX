using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding
{
    public class Command
    {
        public string Id { get; set; }
        public List<string> VoiceTriggers { get; set; }
    }

    public class CommandSimilarity
    {
        public double Similarity { get; set; }

        public Command Command { get; set; }
    }
}
