using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbit.UWP.Models
{
    class EmotionModel
    {
        public FaceRectangleModel FaceRectangle { get; set; }
        public ScoresModel Scores { get; set; }
    }
}
