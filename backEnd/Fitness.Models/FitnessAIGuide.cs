using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class FitnessSuggestion
    {
        public int screenshotID { get; set; }

        public int userID { get; set; }

        public string createTime { get; set; }

        public string exerciseName { get; set; }

        public string? suggestions { get; set; }

        public string screenshotUrl { get; set; }
    }

    public class FitnessEquiOperation
    {
        public string equipmentName { get; set; }

        public string equipmentGuide { get; set; }

        public string createTime { get; set; }

        public string lastUpdateTime { get; set; }

        public float? userRatings { get; set; }
    }
}
