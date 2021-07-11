using ExpenseTracker.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repository.Shared
{
    public class MailTagReplacer : ITagReplacer
    {
        public List<string> Tags { get; set; }
        
        public MailTagReplacer()
        {
            Tags = new List<string> { "HEADING", "SUBHEADING", "BODY", "FOOTER", "SUBFOOTER" };
        }

        public string ReplaceTags(Dictionary<string, string> tagText, string text)
        {
            foreach(var t in tagText)
            {
                if(Tags.Contains(t.Key.ToUpper()))
                {
                    text = text.Replace($"[{t.Key.ToUpper()}]", t.Value);
                }
            }

            var tags = Tags.Where(t => !tagText.ContainsKey(t.ToUpper()) && !tagText.ContainsKey(t.ToLower()));

            foreach(var t in tags)
            {
                text = text.Replace($"[{t.ToUpper()}]", string.Empty);
            }

            return text;
        }
    }
}
