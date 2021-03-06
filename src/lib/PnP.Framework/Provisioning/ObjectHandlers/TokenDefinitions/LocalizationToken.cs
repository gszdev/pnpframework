using Microsoft.SharePoint.Client;
using PnP.Framework.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PnP.Framework.Provisioning.ObjectHandlers.TokenDefinitions
{
    [TokenDefinitionDescription(
     Token = "{localization:[key]}",
     Description = "Returns a value from a in the template provided resource file given the locale of the site that the template is applied to",
     Example = "{localization:MyListTitle}",
     Returns = "My List Title")]
    internal class LocalizationToken : TokenDefinition
    {
        private readonly List<ResourceEntry> _resourceEntries;
        public LocalizationToken(Web web, string key, List<ResourceEntry> resourceEntries)
            : base(web, $"{{loc:{Regex.Escape(key)}}}", $"{{localize:{Regex.Escape(key)}}}", $"{{localization:{Regex.Escape(key)}}}", $"{{resource:{Regex.Escape(key)}}}", $"{{res:{Regex.Escape(key)}}}")
        {
            _resourceEntries = resourceEntries;
        }

        public override string GetReplaceValue()
        {
            var entry = _resourceEntries.FirstOrDefault(r => r.LCID == this.Web.Language);
            if (entry != null)
            {
                return entry.Value;
            }
            else
            {
                // fallback
                return _resourceEntries.First().Value;
            }

        }

        public List<ResourceEntry> ResourceEntries
        {
            get { return _resourceEntries; }
        }
    }
}