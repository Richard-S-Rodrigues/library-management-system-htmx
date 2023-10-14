using LibraryManagementSystemHtmx.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryManagementSystemHtmx.Pages;

public class BookEditModel : PageModel 
{
  [BindProperty]
  public Book Book { get; set; }

  public void OnGet()
  {
  }

  public void OnPost()
  {
    
  }
}