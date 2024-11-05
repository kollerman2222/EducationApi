//using fgssr.Models;
//using fgssr.UnitOFWork;
//using fgssr.ViewModels;
//using MailKit.Search;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using System.Drawing.Printing;

//namespace fgssr.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly IUnitOfWork _unitofwork;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        public HomeController(IUnitOfWork unitofwork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            _unitofwork = unitofwork;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }


//        public IActionResult HowToApply()
//        {
//            return View();
//        }

//        public IActionResult AboutUs()
//        {
//            return View();
//        }

//        public IActionResult RemoteMessages()
//        {
//            return View();
//        }


//        public IActionResult CollegeDefinition()
//        {
//            return View();
//        }



//        public IActionResult CollegeAdministrators()
//        {
//            return View();
//        }

//        public IActionResult PreviousCollegeAdministrators()
//        {
//            return View();
//        }


//        public IActionResult CollegeMap()
//        {
//            return View();
//        }

//        public IActionResult CollegeDistinctive()
//        {
//            return View();
//        }

//        public IActionResult HonoredStaff()
//        {
//            return View();
//        }

//        public IActionResult TopRasayel()
//        {
//            return View();
//        }


//        public IActionResult PublishedResearchGuide()
//        {
//            return View();
//        }


//        public IActionResult PublishMagazine()
//        {
//            return View();
//        }

//        public IActionResult Researchplans()
//        {
//            return View();
//        }

//        public IActionResult CommunityService()
//        {
//            return View();
//        }


//        public IActionResult TrainingProgram()
//        {
//            return View();
//        }

//        public IActionResult CrisisManagment()
//        {
//            return View();
//        }

//        public IActionResult InformaticsCenter()
//        {
//            return View();
//        }

//        public IActionResult DemographicCenter()
//        {
//            return View();
//        }


//        public IActionResult ConsultingCenter()
//        {
//            return View();
//        }


//        public IActionResult CollegeConference()
//        {
//            return View();
//        }

//        public IActionResult CollegeRelease()
//        {
//            return View();
//        }



//        public IActionResult CollegeMagazine()
//        {
//            return View();
//        }


//        public IActionResult CollegeDealing()
//        {
//            return View();
//        }

//        public IActionResult CollegeLibrary()
//        {
//            return View();
//        }


//        public IActionResult EgyptianMagazine()
//        {
//            return View();  
//        }




//        public IActionResult EgyptianStatisticalMagazine()
//        {
//            return View();
//        }

//        public IActionResult EgyptianComputerMagazine()
//        {
//            return View();
//        }

//        public IActionResult GetAllUsersFolders()
//        {
//            var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/gradcerts");
//            var foldersNames = Directory.GetDirectories(mainPath).Select(Path.GetFileName).ToList();
//            return View(foldersNames);
//        }



//        public IActionResult specificUserFolder(string userfolderName)
//        {
//            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/gradcerts", userfolderName);

//            var files = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();

//            ViewBag.UserFolderName = userfolderName;

//            return View(files);
//        }











//        public async Task<ActionResult> GetChatMessage() 
//        { 
//            var msgs = await _unitofwork.ChatMessages.GetAllAsync();
//            var result = msgs.Select(m => new
//            {
//                Content = m.Content,
//                SenderName = m.SenderName,
//                ProfileImage = m.ProfileImage,
               
//            });
//            return Json(result);
//        }


//        public IActionResult Search( string query)
//        {

//            return RedirectToAction("Diplomas", new { query });
//        }


//        public async  Task<IActionResult> Homepage()
//        {
//            var home = new DashboardViewModel
//            {
//                //Departments = await _unitofwork.DiplomasDepartments.GetAllAsync(),
//                Sections = await _unitofwork.Sections.GetAllAsync(),
//                Events = await _unitofwork.Events.GetAllAsync(),
//                News = await _unitofwork.News.GetAllAsync(),
//            };

//            return View(home);
            
//        }

//        [HttpGet("main/dashboard")]
//		[Authorize(Roles = "Admin")]

//		public async Task<IActionResult> Dashboard()
//        {
         

//            var getall = new DashboardViewModel
//            {
//                Departments = await _unitofwork.DiplomasDepartments.GetAllAsync(),
//                Sections = await _unitofwork.Sections.GetAllAsync(),
//                Events = await _unitofwork.Events.GetAllAsync(),
//                News = await _unitofwork.News.GetAllAsync(),
//                Staff = await _unitofwork.Staffs.GetAllAsync(),
//                Users = await _userManager.Users.ToListAsync()
//            };

//            return View(getall);
//        }



//        [HttpGet("main/diplomas")]
//        public async Task<IActionResult> Diplomas(string departmentSearch, string query, int pageNumber = 1)
//        {
//            var sections = await _unitofwork.Sections.GetAllAsync();
//            if (!String.IsNullOrEmpty(departmentSearch) && String.IsNullOrEmpty(query))
//            {
//                sections = sections.Where(x => x.Department?.DepartmentName == departmentSearch);
//            }
//            if (String.IsNullOrEmpty(departmentSearch) && !String.IsNullOrEmpty(query))
//            {
//                sections = sections.Where(x => x.SectionName.ToLower().Contains(query));
//            }
//            var totalItems = sections.Count();
//            var totalPages = (int)Math.Ceiling((decimal) totalItems / 6);
//            var pagination = sections.Skip((pageNumber - 1) * 6).Take(6).ToList();
//            //var pagination = _unitofwork.Sections.PaginateAsync(pageNumber, 6);
//            //var dsvm = sections.Select(sec => new DiplomasSectionsViewModel
//            //{
//            //    SectionName = sec.SectionName,
//            //    SecId = sec.SectionId,
//            //    Description = sec.Description,
//            //    isActive = sec.isActive,
//            //    Department = sec.Department,
//            //    SectionImage = sec.SectionImage
//            //}).ToList();

//            var dsvm = new DiplomasSectionsViewModel
//            {
//                secs = pagination,
//                TotalPages = totalPages,
//                Page = pageNumber,
//                departmentSearch=departmentSearch,
//                query=query
//            };
//            return View(dsvm);
//        }


//        [HttpGet("main/diplomadetails")]
//        public async Task<IActionResult> DiplomaDetails(int? id)
//        {
//            if (id == null)
//            {
//                return BadRequest();
//            }

//            var section = await _unitofwork.Sections.GetByIdAsync(id);
//            var subjects = await _unitofwork.Subjects.GetAllAsync();
//            var filterSubjects = subjects.Where(s => s.SectionId == section?.SectionId).ToList();
//            if (section == null)
//            {
//                return NotFound();
//            }
//            var dsvm = new DiplomasSectionsViewModel()
//            {
//                SectionName = section.SectionName,
//                SecId = section.SectionId,
//                Description = section.Description,
//                isActive = section.isActive,
//                Department = section.Department,
//                SectionImage = section.SectionImage,
//                Subjects=filterSubjects,
//            };
//            return View(dsvm);
//        }



//        [HttpGet("main/staffs")]
//        public async Task<IActionResult> Staff()
//        {

//            var staffs = await _unitofwork.Staffs.GetAllAsync();
//            var svm = staffs.Select(staff => new StaffViewModel
//            {
//                StID = staff.StaffId,
//                Biograpghy = staff.Biograpghy,
//                StaffName = staff.StaffName,
//                StaffPosition = staff.StaffPosition,
//                StaffImageName = staff.StaffImage,
//                Email = staff.Email,
//            }).ToList();

//            return View(svm);
//        }

//        [HttpGet("main/staffdetails")]
//        public async Task<IActionResult> StaffDetails(int? id)
//        {
//            if (id == null)
//            {
//                return BadRequest();
//            }

//            var staff = await _unitofwork.Staffs.GetByIdAsync(id);
//            if (staff == null)
//            {
//                return NotFound();
//            }
//            var svm = new StaffViewModel()
//            {
//                StID = staff.StaffId,
//                Biograpghy = staff.Biograpghy,
//                StaffName = staff.StaffName,
//                StaffPosition = staff.StaffPosition,
//                StaffImageName = staff.StaffImage,
//                Email = staff.Email,
//            };
//            return View(svm);
//        }

//        [HttpGet("main/events")]
//        public async Task<IActionResult> Events()
//        {

//            var events = await _unitofwork.Events.GetAllAsync();
//            var evm = events.Select(eve => new EventsViewModel
//            {
//                EveId = eve.EventId,
//                EventTitle = eve.EventTitle,
//                EventDescription = eve.EventDescription,
//                DateDay = eve.DateDay,
//                EventLocation = eve.EventLocation,
//                DateMonth = eve.DateMonth,
//                EventImageName = eve.EventImage,
//                Time = eve.Time,

//            }).ToList();

//            return View(evm);
//        }


//        [HttpGet("main/eventdetails")]
//        public async Task<IActionResult> EventDetails(int? id)
//        {
//            if (id == null)
//            {
//                return BadRequest();
//            }

//            var events = await _unitofwork.Events.GetByIdAsync(id);
//            if (events == null)
//            {
//                return NotFound();
//            }
//            var evm = new EventsViewModel()
//            {
//                EveId = events.EventId,
//                EventTitle = events.EventTitle,
//                EventDescription = events.EventDescription,
//                EventLocation = events.EventLocation,
//                DateDay = events.DateDay,
//                DateMonth = events.DateMonth,
//                EventImageName = events.EventImage,
//                Time = events.Time,
//            };
//            return View(evm);
//        }

//        [HttpGet("main/news")]
//        public async Task<IActionResult> News()
//        {

//            var news = await _unitofwork.News.GetAllAsync();
//            var nvm = news.Select(nn => new NewsViewModel
//            {
//                NewsDescription = nn.NewsDescription,
//                NewsTitle = nn.NewsTitle,
//                NId = nn.NewsId,
//                NewsDate = nn.NewsDate,

//            }).ToList();

//            return View(nvm);
//        }

//        [HttpGet("main/newsdetails")]
//        public async Task<IActionResult> NewsDetails(int? id)
//        {
//            if (id == null)
//            {
//                return BadRequest();
//            }

//            var news = await _unitofwork.News.GetByIdAsync(id);
//            if (news == null)
//            {
//                return NotFound();
//            }
//            var nvm = new NewsViewModel()
//            {
//                NewsDate = news.NewsDate,
//                NewsDescription = news.NewsDescription,
//                NewsTitle = news.NewsTitle,
//                NId = news.NewsId
//            };

//            return View(nvm);
//        }

//        [HttpGet("main/departments")]
//        public async Task<IActionResult> Departments()
//        {

//            var departments = await _unitofwork.DiplomasDepartments.GetAllAsync();
//            var ddvm = departments.Select(dep => new DiplomasDepartmentsViewModel
//            {
//                DepId = dep.DepartmentId,
//                DepartmentName = dep.DepartmentName,
//                Description = dep.Description
//            }).ToList();

//            return View(ddvm);
//        }


//    }
//}
