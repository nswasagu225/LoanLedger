using LoanLedger.Application.Contacts;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<CreateContactResponse> Create(
        CreateContactRequest request)
    {
        return await _contactService.CreateAsync(request);
    }

    [HttpGet("{userId:guid}")]
    public async Task<List<ContactDto>> GetByUser(Guid userId)
    {
        return await _contactService.GetByUserAsync(userId);
    }
}