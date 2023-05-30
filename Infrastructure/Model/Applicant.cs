using Infrastructure.Interfaces;

namespace Infrastructure.Model;

public class Applicant : IDbModel
{
    public static int ApplicantsCreated;
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly BirthDate { get; set; }
    public EducationLevel EducationLevel { get; set; }
    public List<(Subject, float)> Rates { get; set; }
    public List<(int, int, EducationForm)> Applications { get; set; }

    public Applicant()
    {
        Rates = new();
        Applications = new();
    }

    public Applicant(string csvLine)
    {
        Rates = new();
        Applications = new();
        string[] data = csvLine.Split(';');
        if (data.Length != 8 || !int.TryParse(data[0], out int id) || !DateOnly.TryParse(data[4], out DateOnly bDate) || !Enum.TryParse(data[5], out EducationLevel educationLevel))
            throw new ArgumentException("Incorrect institution`s csv");
        Id = id;
        Surname = data[1];
        Name = data[2];
        if (data[3].Length > 0)
            Patronymic = data[3];
        BirthDate = bDate;
        EducationLevel = educationLevel;
        
        string[] rates = data[6].Split(',');
        if (rates.Length is < 3 or > 4 )
            throw new ArgumentException("Incorrect institution`s csv (rates)");

        foreach (var pair in rates)
        {
            var rate = pair.Split(':');
            if (rate.Length != 2 || !Enum.TryParse(rate[0], out Subject subj) || !float.TryParse(rate[1], out float r))
                throw new ArgumentException("Incorrect institution`s csv (rate)");
            Rates.Add((subj, r));
        }
        string[] applications = data[7].Split(',');
        if (applications.Length is < 1 or > 20 )
            throw new ArgumentException("Incorrect institution`s csv (applications)");

        foreach (var tuple in applications)
        {
            var application = tuple.Split(':');
            if (application.Length != 3 || !int.TryParse(application[0], out int speciality) || !int.TryParse(application[1], out int institutionId) || !Enum.TryParse(application[2], out EducationForm form))
                throw new ArgumentException("Incorrect institution`s csv (app)");
            Applications.Add((speciality, institutionId, form));
        }
    }

    public string ToCsvString()
    {
        string[] components = new string[]
        {
            Id.ToString(), 
            Surname,
            Name,
            Patronymic ?? "",
            BirthDate.ToString(),
            EducationLevel.ToString(),
            string.Join(',', Rates.Select(a => ""+a.Item1+':'+a.Item2)),
            string.Join(',', Applications.Select(a => ""+a.Item1+':'+a.Item2+':'+a.Item3))
        };
        return string.Join(';', components);
    }
}