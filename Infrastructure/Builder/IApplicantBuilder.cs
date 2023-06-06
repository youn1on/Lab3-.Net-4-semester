using Infrastructure.Model;

namespace Infrastructure.Builder;

public interface IApplicantBuilder
{
    public IApplicantBuilder SetName(string name);
    public IApplicantBuilder SetSurname(string name);
    public IApplicantBuilder SetPatronymic(string patronymic);
    public IApplicantBuilder SetBirthDate(DateOnly birthdate);
    public IApplicantBuilder SetEducationLevel(EducationLevel level);
    public IApplicantBuilder AddRate(Subject subject, float rate);
    public IApplicantBuilder AddApplication(ushort speciality, int institutionId, EducationForm form);
    public Applicant Build();
}