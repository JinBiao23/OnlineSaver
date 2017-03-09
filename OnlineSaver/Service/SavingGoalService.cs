using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSaver.Service
{
    public interface ISavingGoalService {
        void CreateSavingGoal(SavingGoal model);
    }
    public class SavingGoalService : ISavingGoalService
    {
        public void CreateSavingGoal(SavingGoal model)
        {
            using (var ctx = new OnlineSaverEntities()) {
                ctx.SavingGoals.Add(model);
                ctx.SaveChanges();
            }
        }
    }
}