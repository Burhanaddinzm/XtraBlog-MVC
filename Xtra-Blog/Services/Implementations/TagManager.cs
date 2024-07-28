using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
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

    public async Task<List<Tag>> GetAllTagsAsync(AppUser? user)
    {
        return user != null
               ? await _context.Tags.Where(x => x.UserId == user.Id).ToListAsync()
               : await _context.Tags.ToListAsync();
    }

    public async Task<bool> CheckDuplicateAsync(string tagName, string userId, int? tagId = null)
    {
        Tag? existingTag;

        if (tagId != null)
        {
            existingTag = await _context.Tags.FirstOrDefaultAsync(
                x => x.Name.Trim().ToLower() == tagName.Trim().ToLower() &&
                x.Id != tagId &&
                x.UserId == userId
                );
        }
        else
        {
            existingTag = await _context.Tags.FirstOrDefaultAsync(
                x => x.Name.Trim().ToLower() == tagName.Trim().ToLower() &&
                x.UserId == userId
                );
        }

        return existingTag != null;
    }

    public async Task CreateTagAsync(UpdateTagVM tagVM, AppUser user)
    {
        await _context.Tags.AddAsync(new Tag { Name = tagVM.Name, UserId = user.Id });
        await _context.SaveChangesAsync();
    }

    public async Task<Tag?> GetTagAsync(int id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task UpdateTagAsync(UpdateTagVM tagVM)
    {
        var existingTag = await GetTagAsync(tagVM.Id);

        if (existingTag == null)
        {
            throw new Exception("Tag doesn't exist!");
        }

        existingTag.Name = tagVM.Name;

        _context.Tags.Update(existingTag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(Tag tag)
    {
        tag.IsDeleted = true;
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }
}
