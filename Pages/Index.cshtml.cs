using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryManagementSystemHtmx.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private static int counter = 0;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        counter = 0;
    }

    public IActionResult OnPostIncrement()
    {
        return Content($"Counter: <span>{++counter}</span>"); 
    }
}
