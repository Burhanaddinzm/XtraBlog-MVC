using Microsoft.EntityFrameworkCore;
using XtraBlog.Data;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Tag;

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

    public async Task<bool> CheckDuplicateAsync(string tagName, int? tagId = null)
    {
        Tag? existingTag;

        if (tagId != null)
        {
            existingTag = await _context.Tags.FirstOrDefaultAsync(
                x => x.Name.Trim().ToLower() == tagName.Trim().ToLower() &&
                x.Id != tagId
                );
        }
        else
        {
            existingTag = await _context.Tags.FirstOrDefaultAsync(
                x => x.Name.Trim().ToLower() == tagName.Trim().ToLower()
                );
        }

        return existingTag != null;
    }

    public async Task CreateTagAsync(CreateTagVM tagVM)
    {
        await _context.Tags.AddAsync(new Tag { Name = tagVM.Name });
        await _context.SaveChangesAsync();
    }

}
