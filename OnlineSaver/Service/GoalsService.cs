using OnlineSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSaver.Service
{
    public interface IGoalsService
    {
        List<GoalViewModel> SortGoalList(List<GoalViewModel> list, string sortOrder);
        List<SavingGoal> GetUserGoals(string userId);
    }
    public class GoalsService : IGoalsService
    {
        public List<SavingGoal> GetUserGoals(string userId)
        {
            using (var ctx = new OnlineSaverEntities()) {
              return  ctx.SavingGoals.Where(x => x.UserId == userId).ToList();
            } 
        }

        public List<GoalViewModel> SortGoalList(List<GoalViewModel> list, string sortOrder)
        {
            switch (sortOrder)
            {
                case "title_desc":
                    list = list.OrderByDescending(s => s.Title).ToList();
                    break;
                case "title_asc":
                    list = list.OrderBy(s => s.Title).ToList();
                    break;
                case "date_desc":
                    list = list.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case "date_asc":
                    list = list.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "savedamount_desc":
                    list = list.OrderByDescending(s => s.SavedAmount).ToList();
                    break;
                case "savedamount_asc":
                    list = list.OrderBy(s => s.SavedAmount).ToList();
                    break;
                case "goal_desc":
                    list = list.OrderByDescending(s => s.SaveGoal).ToList();
                    break;
                case "goal_asc":
                    list = list.OrderBy(s => s.SaveGoal).ToList();
                    break;
                case "progress_desc":
                    list = list.OrderByDescending(s => s.progress).ToList();
                    break;
                case "progress_asc":
                    list = list.OrderBy(s => s.progress).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.SavedAmount).ToList();
                    break;
            }
            return list;
        }
    }
}