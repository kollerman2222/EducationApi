using FgssrApi.IRepository;

namespace FgssrApi.UnitOFWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDiplomasDepartmentsRepository DiplomasDepartments { get; }
        IEventsRepository Events { get; }
        IStaffRepository Staffs { get; }
        INewsRepository News { get; }
        IDiplomasSectionsRepository Sections { get; }
        ISubjectsRepository Subjects { get; }
        IChatMessagesRepository ChatMessages { get; }
        ISubjectsMarksRepository SubjectMarks { get; }
        void SaveChanges();
    }
}
