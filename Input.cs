using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeG
{
    internal class Input
    {
        //List olemasolevatest nuppudest
        private static Hashtable keyTable = new Hashtable();

        //tagastab true/false olenevalt sellest kas vajutatud nupul on väärtus
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }

            return (bool)keyTable[key];
        }

        //Kontrollib kas nuppu vajutati
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
