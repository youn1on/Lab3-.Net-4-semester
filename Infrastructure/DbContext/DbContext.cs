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
    
    public static DbContext GetDbInstance()
    {
        if (_instance is null)
        {
            var applicants = DbContextFiller.GetApplicants();
            var institutions = DbContextFiller.GetInstitutions();

            _instance = new DbContext(institutions, applicants);
            Applicant.ApplicantsCreated = _instance.Applicants.Count;
        }

        return _instance;
    }

    public static void Save()
    {
        if (_instance is null)
            return;

        using (StreamWriter sw = new StreamWriter(DbContextFiller.InstitutionsSource))
        {
            foreach (var institution in _instance.Institutions)
            {
                sw.WriteLine(institution.ToCsvString());
            }
        }
        
        using (StreamWriter sw = new StreamWriter(DbContextFiller.ApplicantsSource))
        {
            foreach (var applicant in _instance.Applicants)
            {
                sw.WriteLine(applicant.ToCsvString());
            }
        }
    }
}