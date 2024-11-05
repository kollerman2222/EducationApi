using FgssrApi.Data;
using FgssrApi.IRepository;
using FgssrApi.Models;
using FgssrApi.Repository;

namespace FgssrApi.UnitOFWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IDiplomasDepartmentsRepository DiplomasDepartments { get; private set; }
        public IEventsRepository Events { get; private set; }
        public IStaffRepository Staffs { get; private set; }
        public INewsRepository News { get; private set; }
        public IDiplomasSectionsRepository Sections { get; }
        public ISubjectsRepository Subjects { get; private set; }
        public ISubjectsMarksRepository SubjectMarks { get; private set; }
        public IChatMessagesRepository ChatMessages { get; private set; }
        public UnitOfWork(ApplicationDbContext context , IWebHostEnvironment webHostEnvironment)
        {
             _context = context;
            _webHostEnvironment = webHostEnvironment;
            DiplomasDepartments = new DiplomasDepartmentsRepository(_context);
            News = new NewsRepository(_context);
            Staffs = new StaffRepository(_context , _webHostEnvironment);
            Events = new EventsRepository(_context , _webHostEnvironment);
            Sections = new DiplomasSectionsRepository(_context, _webHostEnvironment);
            Subjects = new SubjectsRepository(_context);
            ChatMessages = new ChatMessagesRepository(_context);
            SubjectMarks = new SubjectsMarksRepository(_context);
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
           _context.SaveChanges();
        }


    }
}
