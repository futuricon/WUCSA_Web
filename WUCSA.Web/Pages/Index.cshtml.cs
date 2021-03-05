using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ImageHelper _imageHelper;

        public IndexModel(ILogger<IndexModel> logger, ImageHelper imageHelper)
        {
            _logger = logger;
            _imageHelper = imageHelper;
        }

        public void OnGet()
        {

        }

        public void OnPostImgtest()
        {
            _imageHelper.GenerateImage("keepline@inbox.ru");
        }
    }
}
