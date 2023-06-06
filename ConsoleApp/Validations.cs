using System.Text.RegularExpressions;
using Infrastructure.DbContext;
using Infrastructure.Model;

namespace ConsoleApp;

public class Validations
{
    public static bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        
        return Regex.IsMatch(name, @"^[A-Z][a-z]*(?:-[A-Z][a-z]*)?$");
    }
    
    public static bool IsValidPatronymic(string patronymic)
    {
        if (string.IsNullOrWhiteSpace(patronymic))
            return false;
        
        return patronymic == "" || IsValidName(patronymic);
    }
    
    public static bool IsValidBirthDate(string date)
    {
        return (DateOnly.TryParse(date, out DateOnly birthdate)
                && birthdate.Year < DateTime.Now.Year - 15);
    }

    public static bool IsValidRateInputString(string arg)
    {
        if (arg == "") return true;
        string[] splitted = arg.Split(':', StringSplitOptions.RemoveEmptyEntries);
        return splitted.Length == 2 && Enum.TryParse(splitted[0], out Subject _) &&
               float.TryParse(splitted[1], out float grade) && grade is >= 100 and <= 200;
    }

    public static bool IsValidApplicationInputString(string arg)
    {
        if (arg == "") return true;
        string[] splitted = arg.Split(':', StringSplitOptions.RemoveEmptyEntries);
        return splitted.Length == 3 && int.TryParse(splitted[0], out _) &&
               Enum.TryParse(splitted[2], out EducationForm _) &&
               DbContext.GetInstitutionByName(splitted[1].Trim()) is not null;

    }
    
    public static bool IsValidEducationLevel(string date)
    {
        return Enum.TryParse(date, out EducationLevel _);
    }
}