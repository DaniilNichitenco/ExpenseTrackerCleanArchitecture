using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Application.Interfaces
{
    public interface ITagReplacer
    {
        List<string> Tags { get; set; }
        string ReplaceTags(Dictionary<string, string> tagText, string text);
    }
}
