using Infrastructure.Builder;
using Infrastructure.Model;

namespace Infrastructure.DbContext;

public partial class DbContext
{
    public static Applicant? GetApplicant(int id)
    {
        _instance = GetDbInstance();
        return _instance.Applicants.FirstOrDefault(a => a.Id == id);
    }
    
    public static List<Applicant> GetAllApplicants()
    {
        _instance = GetDbInstance();
        return _instance.Applicants;
    }

    public static HigherEducationInstitution? GetInstitutionById(int id)
    {
        _instance = GetDbInstance();
        return _instance.Institutions.FirstOrDefault(i => i.Id == id);
    }
    
    public static HigherEducationInstitution? GetInstitutionByName(string name)
    {
        _instance = GetDbInstance();
        return _instance.Institutions.FirstOrDefault(i => i.Name == name);
    }

    public static void AddApplicant(Applicant applicant)
    {
        _instance = GetDbInstance();
        _instance.Applicants.Add(applicant);
    }
}