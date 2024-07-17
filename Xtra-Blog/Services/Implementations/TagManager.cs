using Microsoft.EntityFrameworkCore;
using XtraBlog.Data;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;

namespace XtraBlog.Services.Implementations;

public class TagManager : ITagService
{
    private readonly AppDbContext _context;

    public TagManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }
}
