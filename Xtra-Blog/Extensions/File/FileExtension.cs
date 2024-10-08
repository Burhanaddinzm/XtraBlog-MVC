﻿using XtraBlog.Helpers.File;
using IO = System.IO;
namespace XtraBlog.Extensions.File;

public static class FileExtension
{
    public static async Task<string> SaveFileAsync(this IFormFile file, string root, params string[] folders)
    {
        Console.WriteLine(file.Name, file.ContentType, file.Headers.ContentType.ToString());

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string[] pathParts = folders.Prepend(root).Append(uniqueFileName).ToArray();
        string path = Path.Combine(pathParts);

        using FileStream fs = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(fs);

        return uniqueFileName;
    }

    public static bool CheckFileType(this IFormFile file, string fileType)
    {
        if (file.ContentType.Contains(fileType))
        {
            return true;
        }
        return false;
    }


    public static bool CheckFileSize(this IFormFile file, int megabytes)
    {
        if (file.Length / 1024 / 1024 >= megabytes)
        {
            return false;
        }
        return true;
    }


    public static void DeleteFile(this IFormFile file, string root, params string[] fileLocation)
    {
        string[] pathParts = fileLocation.Prepend(root).ToArray();
        string path = Path.Combine(pathParts);

        if (IO.File.Exists(path))
        {
            IO.File.Delete(path);
        }
    }

    public static FileValidationResult ValidateFile(this IFormFile file)
    {
        if (!file.CheckFileSize(2))
        {
            return new FileValidationResult(false, "File size can't exceed 2 MB!");
        }

        if (!file.CheckFileType("image"))
        {
            return new FileValidationResult(false, "File type is invalid!");
        }

        return new FileValidationResult(true);
    }
}
