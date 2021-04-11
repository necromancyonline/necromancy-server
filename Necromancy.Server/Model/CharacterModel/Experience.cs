

using Necromancy.Server.Common;

namespace Necromancy.Server.Model
{
    public class Experience
    {
        public uint Level { get; set; }
        public ulong CumulativeExperience { get; set; }

        public Experience()
        {
            Level = 0;
            CumulativeExperience = 0;

        }

        public Experience CalculateLevelUp(uint level)
        {
            switch (level)
            {
                case 1: this.CumulativeExperience = 0; break;
                case 2: this.CumulativeExperience = 100; break;
                case 3: this.CumulativeExperience = 315; break;
                case 4: this.CumulativeExperience = 745; break;
                case 5: this.CumulativeExperience = 1605; break;
                case 6: this.CumulativeExperience = 3325; break;
                case 7: this.CumulativeExperience = 6765; break;
                case 8: this.CumulativeExperience = 13473; break;
                case 9: this.CumulativeExperience = 26218; break;
                case 10: this.CumulativeExperience = 49797; break;
                case 11: this.CumulativeExperience = 92238; break;
                case 12: this.CumulativeExperience = 166511; break;
                case 13: this.CumulativeExperience = 292774; break;
                case 14: this.CumulativeExperience = 501109; break;
                case 15: this.CumulativeExperience = 834445; break;
                case 16: this.CumulativeExperience = 1351115; break;
                case 17: this.CumulativeExperience = 2126121; break;
                case 18: this.CumulativeExperience = 3288629; break;
                case 19: this.CumulativeExperience = 5032391; break;
                case 20: this.CumulativeExperience = 7648034; break;
                case 21: this.CumulativeExperience = 11440717; break;
                case 22: this.CumulativeExperience = 16940107; break;
                case 23: this.CumulativeExperience = 24639253; break;
                case 24: this.CumulativeExperience = 35033100; break;
                case 25: this.CumulativeExperience = 49064794; break;
                case 26: this.CumulativeExperience = 67305996; break;
                case 27: this.CumulativeExperience = 91019558; break;
                case 28: this.CumulativeExperience = 119475833; break;
                case 29: this.CumulativeExperience = 153623363; break;
                case 30: this.CumulativeExperience = 193917448; break;
                case 31: this.CumulativeExperience = 241464469; break;
                case 32: this.CumulativeExperience = 296619013; break;
                case 33: this.CumulativeExperience = 360598284; break;
                case 34: this.CumulativeExperience = 434174445; break;
                case 35: this.CumulativeExperience = 518787031; break;
                case 36: this.CumulativeExperience = 615765379; break;
                case 37: this.CumulativeExperience = 725207895; break;
                case 38: this.CumulativeExperience = 849465539; break;
                case 39: this.CumulativeExperience = 989876676; break;
                case 40: this.CumulativeExperience = 1147137150; break;
                case 41: this.CumulativeExperience = 1323268881; break;
                case 42: this.CumulativeExperience = 1518775102; break;
                case 43: this.CumulativeExperience = 1735787007; break;
                case 44: this.CumulativeExperience = 1976670222; break;
                case 45: this.CumulativeExperience = 2244050590; break;
                case 46: this.CumulativeExperience = 2540842799; break;
                case 47: this.CumulativeExperience = 2870282151; break;
                case 48: this.CumulativeExperience = 3235959832; break;
                case 49: this.CumulativeExperience = 3641862058; break;
                case 50: this.CumulativeExperience = 4092413528; break;
                case 51: this.CumulativeExperience = 4597031175; break;
                case 52: this.CumulativeExperience = 5162202939; break;
                case 53: this.CumulativeExperience = 5800847033; break;
                case 54: this.CumulativeExperience = 6528901300; break;
                case 55: this.CumulativeExperience = 7358883165; break;
                case 56: this.CumulativeExperience = 8313362309; break;
                case 57: this.CumulativeExperience = 9411013325; break;
                case 58: this.CumulativeExperience = 10673311993; break;
                case 59: this.CumulativeExperience = 12137578448; break;
                case 60: this.CumulativeExperience = 13836127535; break;
                case 61: this.CumulativeExperience = 15823429968; break;
                case 62: this.CumulativeExperience = 18148573814; break;
                case 63: this.CumulativeExperience = 20892243553; break;
                case 64: this.CumulativeExperience = 24129773844; break;
                case 65: this.CumulativeExperience = 27982434891; break;
                case 66: this.CumulativeExperience = 32567101537; break;
                case 67: this.CumulativeExperience = 38068701511; break;
                case 68: this.CumulativeExperience = 44670621481; break;
                case 69: this.CumulativeExperience = 52592925445; break;
                case 70: this.CumulativeExperience = 62921685493; break;
                case 71: this.CumulativeExperience = 75316197550; break;
                case 72: this.CumulativeExperience = 90189612020; break;
                case 73: this.CumulativeExperience = 108037709383; break;
                case 74: this.CumulativeExperience = 129455426218; break;
                case 75: this.CumulativeExperience = 155156686421; break;
                case 76: this.CumulativeExperience = 185998198664; break;
                case 77: this.CumulativeExperience = 223008013356; break;
                case 78: this.CumulativeExperience = 267419790986; break;
                case 79: this.CumulativeExperience = 320713924142; break;
                case 80: this.CumulativeExperience = 384666883930; break;
                case 81: this.CumulativeExperience = 453736080501; break;
                case 82: this.CumulativeExperience = 528330812797; break;
                case 83: this.CumulativeExperience = 608893123677; break;
                case 84: this.CumulativeExperience = 695900419427; break;
                case 85: this.CumulativeExperience = 789868298837; break;
                case 86: this.CumulativeExperience = 889868298837; break;
                case 87: this.CumulativeExperience = 999868298837; break;
                case 88: this.CumulativeExperience = 1124868298837; break;
                case 89: this.CumulativeExperience = 1259868298837; break;
                case 90: this.CumulativeExperience = 1385242114520; break;
            }
            return this;
        }

    }
}
