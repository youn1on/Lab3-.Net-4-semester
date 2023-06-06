using System.Text.RegularExpressions;
using ConsoleTables;
using Infrastructure.Builder;
using Infrastructure.DbContext;
using Infrastructure.Model;

namespace ConsoleApp;

public static class UserInterface
{
    public static void MainLoop()
    {
        while (true)
        {
            Console.WriteLine("Type 'add' to add new applicant, 'get all' to get the list of all the applicants, 'get {id}' to get information about concrete applicant, or 'quit' to exit:");
            Console.Write("\t>>> ");
            string input = Console.ReadLine()!;
            switch (input)
            {
                case "quit":
                    return;
                case "get all":
                    OutputAllApplicants();
                    break;
                case "add":
                    AddApplicant();
                    break;
                default:
                    OutputApplicant(input);
                    break;
            }
        }
    }

    private static void OutputApplicant(string command)
    {
        if (!Regex.IsMatch(command, @"^get \d+$"))
        {
            Console.WriteLine("Unresolved command!");
            return;
        }

        int id = int.Parse(command.Split(' ')[1]);
        var applicant = DbContext.GetApplicant(id);
        if (applicant is null)
        {
            Console.WriteLine("No such applicant found!");
                return;
        }

        Console.WriteLine("\t\tApplicant information:");
        Console.WriteLine("Id: " + applicant.Id);
        Console.WriteLine("Full name: " + applicant.Surname + ' ' + applicant.Name + ' ' + (applicant.Patronymic ?? ""));
        Console.WriteLine("Birth Date: " + applicant.BirthDate);
        Console.WriteLine("Education Level: " + applicant.EducationLevel);
        Console.WriteLine("Rates:");
        foreach (var ratePair in applicant.Rates)
        {
            Console.WriteLine("\t" + ratePair.Item1 + " : " + ratePair.Item2);
        }
        Console.WriteLine();
        
        Console.WriteLine("Applications:");
        foreach (var application in applicant.Applications)
        {
            var institution = DbContext.GetInstitutionById(application.Item2);
            Console.WriteLine("\t" + application.Item1 + " speciality at " + (institution?.Name ?? "Unknown") + " (" +
                              application.Item3 + ')');
        }
        Console.WriteLine();
    }

    private static void AddApplicant()
    {
        DbContext.GetDbInstance();
        var builder = new ApplicantBuilder();
        
        var name = GetInput("name", Validations.IsValidName);
        if (name is null) return;
        builder.SetName(name);

        var surname = GetInput("surname", Validations.IsValidName);
        if (surname is null) return;
        builder.SetSurname(surname);
        
        var patronymic = GetOptionalInput("patronymic", Validations.IsValidPatronymic);
        if (patronymic is null) return;
        if (patronymic != "") builder.SetPatronymic(patronymic);
        
        var birthDate = GetInput("birth date", Validations.IsValidBirthDate);
        if (birthDate is null) return;
        builder.SetBirthDate(DateOnly.Parse(birthDate));
        
        var rates = GetSerialInput("rate", "Ukrainian : 195", Validations.IsValidRateInputString, 3, 4);
        if (rates is null) return;
        foreach (var ratePair in rates)
        {
            var rate = ratePair.Split(":");
            builder.AddRate(Enum.Parse<Subject>(rate[0]), float.Parse(rate[1].Replace('.', ',')));
        }
        
        var applications = GetSerialInput("application", "121 : NNTU KPI : Budgetary", Validations.IsValidApplicationInputString, 1, 20);
        if (applications is null) return;
        foreach (var appTuples in applications)
        {
            var application = appTuples.Split(":");
            ushort sp = ushort.Parse(application[0]);
            HigherEducationInstitution inst = DbContext.GetInstitutionByName(application[1].Trim())!;
            int id = inst.Id;
            
            builder.AddApplication(ushort.Parse(application[0]), DbContext.GetInstitutionByName(application[1].Trim())!.Id, Enum.Parse<EducationForm>(application[2]));
        }

        DbContext.AddApplicant(builder.Build());
        Console.WriteLine("Applicant #"+(Applicant.ApplicantsCreated-1)+" created");
    }

    private static string? GetInput(string fieldName, Func<string, bool> validationFunction)
    {
        Console.WriteLine($"Enter the {fieldName} or 'q' to exit: ");
        
        var field = Console.ReadLine()!;
        if (field == "q")
            return null;
        
        while (!validationFunction(field))
        {
            Console.WriteLine($"Incorrect {fieldName} structure! Try again:");
            field = Console.ReadLine()!;
            if (field == "q")
                return null;
        }

        return field;
    }
    
    private static List<string>? GetSerialInput(string fieldName, string inFormat, Func<string, bool> validationFunction, int minAmount, int maxAmount)
    {
        Console.WriteLine($"Enter from {minAmount} to {maxAmount} of {fieldName}s in format '{inFormat}' or 'q' to exit: ");

        List<string> fieldParts = new List<string>();
        for (int i = 0; i < maxAmount; i++)
        {
            string? fieldPart = GetOptionalInput(fieldName, validationFunction);
            if (fieldPart is null)
                return null;
            if (fieldPart == "")
            {
                if (i < minAmount)
                {
                    Console.WriteLine($"You should input at least {minAmount} {fieldName}s!");
                    i--;
                    continue;
                }

                return fieldParts;
            }

            fieldParts.Add(fieldPart);
        }

        return fieldParts;
    }
    
    private static string? GetOptionalInput(string fieldName, Func<string, bool> validationFunction)
    {
        Console.WriteLine($"Enter the {fieldName} or 'q' to exit (leave empty to skip): ");
        
        var field = Console.ReadLine()!;
        if (field == "q")
            return null;
        
        while (!validationFunction(field))
        {
            Console.WriteLine($"Incorrect {fieldName} structure! Try again:");
            field = Console.ReadLine()!;
            if (field == "q")
                return null;
        }

        return field;
    }

    private static void OutputAllApplicants()
    {
        var table = new ConsoleTable("Id", "Name", "Surname", "Education Level", "Avg Rate");

        var applicants = DbContext.GetAllApplicants();
        foreach (var applicant in applicants)
        {
            table.AddRow(applicant.Id, applicant.Name, applicant.Surname, applicant.EducationLevel,
                applicant.Rates.Select(r => r.Item2).Average());
        }
        
        table.Write();
        Console.WriteLine();
    }
}