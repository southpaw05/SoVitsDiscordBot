using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoVitsBot
{
    // Here we have the class that handles the conversion/generation from the AI
    // It was designed to be swappable for other solutions if somebody so wanted that
    public class AiConverter : SingletonBase<AiConverter>
    {
        public void Generate(string name)
        {

        }
    }
}
