using Infrastructure.Model;

namespace Infrastructure.Builder;

public class ApplicantBuilder : IApplicantBuilder
{
    private Applicant _applicant;

    public ApplicantBuilder()
    {
        _applicant = new Applicant(){Id = Applicant.ApplicantsCreated};
    }

    public IApplicantBuilder SetName(string name)
    {
        _applicant.Name = name;
        return this;
    }
    
    public IApplicantBuilder SetSurname(string name)
    {
        _applicant.Surname = name;
        return this;
    }
    
    public IApplicantBuilder SetPatronymic(string patronymic)
    {
        _applicant.Patronymic = patronymic;
        return this;
    }
    
    public IApplicantBuilder SetBirthDate(DateOnly birthdate)
    {
        _applicant.BirthDate = birthdate;
        return this;
    }
    
    public IApplicantBuilder SetEducationLevel(EducationLevel level)
    {
        _applicant.EducationLevel = level;
        return this;
    }
    
    public IApplicantBuilder AddRate(Subject subject, float rate)
    {
        if (rate is >= 0 and <= 200)
            _applicant.Rates.Add((subject, rate));
        return this;
    }
    
    public IApplicantBuilder AddApplication(ushort speciality, int institutionId, EducationForm form)
    {
        if (speciality is >= 11 and <= 275)
            _applicant.Applications.Add((speciality, institutionId, form));
        return this;
    }

    public Applicant Build()
    {
        Applicant.ApplicantsCreated++;
        return _applicant;
    }
}