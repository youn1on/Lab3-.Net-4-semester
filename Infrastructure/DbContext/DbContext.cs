using System.Text;
using Infrastructure.Interfaces;
using Infrastructure.Model;

namespace Infrastructure.DbContext;

public partial class DbContext
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

    public static void Save()
    {
        if (_instance is null)
            return;

        foreach (var institution in _instance.Institutions)
        {
            using (StreamWriter sw = new StreamWriter(DbContextFiller.InstitutionsSource))
            {
                sw.Write(institution.ToCsvString());
            }
        }
        
        foreach (var applicant in _instance.Applicants)
        {
            using (StreamWriter sw = new StreamWriter(DbContextFiller.ApplicantsSource))
            {
                sw.Write(applicant.ToCsvString());
            }
        }
    }
}