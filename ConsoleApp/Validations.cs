namespace ConsoleApp;

public class Validations
{
    public static bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;


        // TODO
        return true;
    }
    
    public static bool IsValidPatronymic(string patronymic)
    {
        if (string.IsNullOrWhiteSpace(patronymic))
            return false;


        // TODO
        return true;
    }
    
    public static bool IsValidBirthDate(string date)
    {
        return (DateOnly.TryParse(date, out DateOnly birthdate)
                && birthdate.Year < DateTime.Now.Year - 15);
    }

    public static bool IsValidRateInputString(string arg)
    {
        return true;
        throw new NotImplementedException();
    }

    public static bool IsValidApplicationInputString(string arg)
    {
        return true;
        throw new NotImplementedException();
    }
}