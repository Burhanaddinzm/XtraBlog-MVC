namespace XtraBlog.Helpers.File;

public class FileValidationResult
{

    public bool IsValid { get; }
    public string ErrorMessage { get; }

    public FileValidationResult(bool isValid, string errorMessage = "")
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
}
