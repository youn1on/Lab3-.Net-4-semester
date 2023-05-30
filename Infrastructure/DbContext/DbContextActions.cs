using Infrastructure.Model;

namespace Infrastructure.DbContext;

public partial class DbContext
{
    public static Applicant? GetApplicant(int id)
    {
        _instance = InitializeDb();
        return _instance.Applicants.FirstOrDefault(a => a.Id == id);
    }
    
    public static List<Applicant> GetAllApplicants()
    {
        _instance = InitializeDb();
        return _instance.Applicants;
    }

    public static void AddApplicant()
    {
        // TODO
    }
}