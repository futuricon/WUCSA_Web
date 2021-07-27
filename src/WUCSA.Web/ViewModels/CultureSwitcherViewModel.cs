using System.Collections.Generic;
using System.Globalization;

namespace WUCSA.Web.ViewModels
{
    public class CultureSwitcherViewModel
    {
        public CultureInfo CurrentUICulture { get; set; }
        public List<CultureInfo> SupportedCultures { get; set; }
    }
}
