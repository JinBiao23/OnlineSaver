using Microsoft.AspNet.Identity;
using OnlineSaver.Models;
using OnlineSaver.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineSaver.Controllers
{
    [Authorize]
    public class HomeController : Controller
    { 
        private ICreditService creditService ;
        private IGoalsService goalsService;
        private ISavingGoalService savingGoalService;
        public HomeController(ICreditService creditService, IGoalsService goalsService, ISavingGoalService savingGoalService)
        {
            this.creditService = creditService;
            this.goalsService = goalsService;
            this.savingGoalService = savingGoalService;
        }

        public ActionResult Index()
        {
            if (TempData["GoalCreated"] != null)
            {
                ViewBag.GoalCreated = true;
            }

            return View(new GoalViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGoal(GoalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newGoal = new SavingGoal()
                {
                    Description = model.Description,
                    CreateDate = DateTime.Now,
                    SavedAmount = 0,
                    Title = model.Title,
                    Goal = model.SaveGoal,
                    UserId = User.Identity.GetUserId()
                };
                savingGoalService.CreateSavingGoal(newGoal);
                TempData["GoalCreated"] = true;
                return RedirectToAction("Index", "Home");
            }
            return View("Index", model);
        }
         
        public ActionResult CreateCredit()
        {

            if (TempData["CreditCreated"] != null)
            {
                ViewBag.CreditCreated = true;
            }
            return View(new CreditViewModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCredit(CreditViewModel model)
        {
            if (ModelState.IsValid)
            {
                decimal needAllocateAmount;
                var userId = User.Identity.GetUserId();
                if (creditService.EnbleToCreateCredit(model, userId, out needAllocateAmount))
                {
                    model.UserId = userId;
                    creditService.CreateCredit(model);
                    creditService.AllocationCredit(model,model.UserId);
                    TempData["CreditCreated"] = true;
                    return RedirectToAction("CreateCredit", "Home");
                }
                else {
                    ViewBag.CreditCreated = false;
                    ViewBag.NeedAllocateAmount = needAllocateAmount; 
                    return View("CreateCredit", model);
                }
            }
            return View("CreateCredit", model);
        }


        public ActionResult GetlGoals(string sortOrder)
        {

            ViewBag.SaveGoalSort = string.IsNullOrEmpty(sortOrder) || sortOrder == "sortgoal_desc" ? "sortgoal_asc" : "sortgoal_desc";

            ViewBag.TitleSort = sortOrder == "title_asc" ? "title_desc" : "title_asc";
            ViewBag.DateSort = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.SaveGoal = sortOrder == "goal_asc" ? "goal_desc" : "goal_asc";
            ViewBag.SavedAmountSort = sortOrder == "savedamount_asc" ? "savedamount_desc" : "savedamount_asc";
            ViewBag.ProgressSort = sortOrder == "progress_asc" ? "progress_desc" : "progress_asc";
             
            var goalList = goalsService.GetUserGoals(User.Identity.GetUserId()); 
            if (goalList.Any())
            {
                List<GoalViewModel> returnList = new List<GoalViewModel>();

                foreach (var goal in goalList)
                {
                    returnList.Add(new GoalViewModel()
                    {
                        Title = goal.Title,
                        Description = goal.Description,
                        CreateDate = goal.CreateDate.Value,
                        SavedAmount = goal.SavedAmount.Value,
                        SaveGoal = goal.Goal,
                        progress = decimal.ToInt32((goal.SavedAmount.Value / goal.Goal) * 100)
                    });
                } 
                return View(new GoalListViewModel()
                {
                    Goals = goalsService.SortGoalList(returnList, sortOrder)
                });
            }
            return View(new GoalListViewModel());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        } 
    }
}