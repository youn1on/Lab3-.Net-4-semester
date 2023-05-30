using Infrastructure.Model;

namespace Infrastructure.DbContext;

public class DbContext
{
    private static DbContext? _instance;
    public List<HigherEducationInstitution> Institutions;
    public List<Applicant> Applicants;

    private DbContext(List<HigherEducationInstitution> institutions, List<Applicant> applicants)
    {
        Applicants = applicants;
        Institutions = institutions;
    }
    
    private DbContext()
    {
        Institutions = new();
        Applicants = new();
    }
    
    public static DbContext InitializeDb()
    {
        if (_instance is not null)
            return _instance;
        
        var applicants = DbContextFiller.GetApplicants();
        var institutions = DbContextFiller.GetInstitutions();

        return new DbContext(institutions, applicants);
    } 
}