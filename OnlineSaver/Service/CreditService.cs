using OnlineSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSaver.Service
{
    public interface ICreditService
    {
        bool EnbleToCreateCredit(CreditViewModel model,string userId, out decimal needAllocatedAmount);
        void CreateCredit(CreditViewModel model);
        void AllocationCredit(CreditViewModel model,string userId);
    } 
    public class CreditService : ICreditService
    {
        public void CreateCredit(CreditViewModel model)
        {
            using (OnlineSaverEntities context = new OnlineSaverEntities())
            {
                context.Credits.Add(new Credit()
                {
                    Descrption = model.Description,
                    ImportAmount = model.CreditAmount,
                    UserId = model.UserId
                });
                context.SaveChanges();
            }
        }

        public void AllocationCredit(CreditViewModel model, string userId) {
            using (OnlineSaverEntities context = new OnlineSaverEntities())
            {
                //var creditList = context.SavingGoals.Where(x => (!x.CreditAllocations.Any() || x.CreditAllocations.Sum(y => y.AllocatedAmount) < x.Goal));
                var allocateGoalList = context.SavingGoals.Where(x => x.SavedAmount < x.Goal && x.UserId == userId);
                if (allocateGoalList.Any()) {
                    allocateGoalList = allocateGoalList.OrderBy(x => x.CreateDate);
                    foreach (var goal in allocateGoalList)
                    {
                        var amountNeedBeAllocated = goal.Goal - goal.SavedAmount;
                        if (model.CreditAmount >= amountNeedBeAllocated)
                        {
                            goal.SavedAmount += amountNeedBeAllocated;
                            model.CreditAmount -= amountNeedBeAllocated.Value;
                        }
                        else
                        {
                            goal.SavedAmount += model.CreditAmount;
                            model.CreditAmount = 0;
                        }
                        if (model.CreditAmount <= 0)
                        {
                            break;
                        }
                    }
                    context.SaveChanges();
                } 
            }
        }

        public bool EnbleToCreateCredit(CreditViewModel model,string userId, out decimal needAllocatedAmount)
        {
            using (OnlineSaverEntities context = new OnlineSaverEntities())
            {
                var goals = context.SavingGoals.Where(x => x.UserId == userId);
                if (goals.Any()) {
                    needAllocatedAmount = goals.Sum(x => x.Goal) - goals.Sum(x => x.SavedAmount).Value;
                    return model.CreditAmount <= needAllocatedAmount;
                }
                needAllocatedAmount = 0;
                return false;
            } 
        }
    }


}