using Infrastructure.Interfaces;
using Infrastructure.Model;

namespace Infrastructure.DbContext;

public static class DbContextFiller
{
    public const string ApplicantsSource = "data/Applicants.csv";
    public const string InstitutionsSource = "data/Institutions.csv";

    private static List<T> GetModels<T>(string sourcePath, Func<string, T> parserFunc) where T : IDbModel
    {
        using StreamReader sr = new StreamReader(sourcePath);
        List<T> data = new List<T>();
        foreach (var csvLine in sr.ReadToEnd().Split('\n'))
        {
            data.Add(parserFunc(csvLine));
        }

        return data;
    }

    public static List<Applicant> GetApplicants()
    {
        return GetModels(ApplicantsSource, csvLine => new Applicant(csvLine));
    }
    
    public static List<HigherEducationInstitution> GetInstitutions()
    {
        return GetModels(InstitutionsSource, csvLine => new HigherEducationInstitution(csvLine));
    }
}