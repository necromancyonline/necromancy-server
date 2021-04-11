

using Necromancy.Server.Common;

namespace Necromancy.Server.Model
{
    public class Experience
    {
        public uint level { get; set; }
        public ulong cumulativeExperience { get; set; }

        public Experience()
        {
            level = 0;
            cumulativeExperience = 0;

        }

        public Experience CalculateLevelUp(uint level)
        {
            switch (level)
            {
                case 1: this.cumulativeExperience = 0; break;
                case 2: this.cumulativeExperience = 100; break;
                case 3: this.cumulativeExperience = 315; break;
                case 4: this.cumulativeExperience = 745; break;
                case 5: this.cumulativeExperience = 1605; break;
                case 6: this.cumulativeExperience = 3325; break;
                case 7: this.cumulativeExperience = 6765; break;
                case 8: this.cumulativeExperience = 13473; break;
                case 9: this.cumulativeExperience = 26218; break;
                case 10: this.cumulativeExperience = 49797; break;
                case 11: this.cumulativeExperience = 92238; break;
                case 12: this.cumulativeExperience = 166511; break;
                case 13: this.cumulativeExperience = 292774; break;
                case 14: this.cumulativeExperience = 501109; break;
                case 15: this.cumulativeExperience = 834445; break;
                case 16: this.cumulativeExperience = 1351115; break;
                case 17: this.cumulativeExperience = 2126121; break;
                case 18: this.cumulativeExperience = 3288629; break;
                case 19: this.cumulativeExperience = 5032391; break;
                case 20: this.cumulativeExperience = 7648034; break;
                case 21: this.cumulativeExperience = 11440717; break;
                case 22: this.cumulativeExperience = 16940107; break;
                case 23: this.cumulativeExperience = 24639253; break;
                case 24: this.cumulativeExperience = 35033100; break;
                case 25: this.cumulativeExperience = 49064794; break;
                case 26: this.cumulativeExperience = 67305996; break;
                case 27: this.cumulativeExperience = 91019558; break;
                case 28: this.cumulativeExperience = 119475833; break;
                case 29: this.cumulativeExperience = 153623363; break;
                case 30: this.cumulativeExperience = 193917448; break;
                case 31: this.cumulativeExperience = 241464469; break;
                case 32: this.cumulativeExperience = 296619013; break;
                case 33: this.cumulativeExperience = 360598284; break;
                case 34: this.cumulativeExperience = 434174445; break;
                case 35: this.cumulativeExperience = 518787031; break;
                case 36: this.cumulativeExperience = 615765379; break;
                case 37: this.cumulativeExperience = 725207895; break;
                case 38: this.cumulativeExperience = 849465539; break;
                case 39: this.cumulativeExperience = 989876676; break;
                case 40: this.cumulativeExperience = 1147137150; break;
                case 41: this.cumulativeExperience = 1323268881; break;
                case 42: this.cumulativeExperience = 1518775102; break;
                case 43: this.cumulativeExperience = 1735787007; break;
                case 44: this.cumulativeExperience = 1976670222; break;
                case 45: this.cumulativeExperience = 2244050590; break;
                case 46: this.cumulativeExperience = 2540842799; break;
                case 47: this.cumulativeExperience = 2870282151; break;
                case 48: this.cumulativeExperience = 3235959832; break;
                case 49: this.cumulativeExperience = 3641862058; break;
                case 50: this.cumulativeExperience = 4092413528; break;
                case 51: this.cumulativeExperience = 4597031175; break;
                case 52: this.cumulativeExperience = 5162202939; break;
                case 53: this.cumulativeExperience = 5800847033; break;
                case 54: this.cumulativeExperience = 6528901300; break;
                case 55: this.cumulativeExperience = 7358883165; break;
                case 56: this.cumulativeExperience = 8313362309; break;
                case 57: this.cumulativeExperience = 9411013325; break;
                case 58: this.cumulativeExperience = 10673311993; break;
                case 59: this.cumulativeExperience = 12137578448; break;
                case 60: this.cumulativeExperience = 13836127535; break;
                case 61: this.cumulativeExperience = 15823429968; break;
                case 62: this.cumulativeExperience = 18148573814; break;
                case 63: this.cumulativeExperience = 20892243553; break;
                case 64: this.cumulativeExperience = 24129773844; break;
                case 65: this.cumulativeExperience = 27982434891; break;
                case 66: this.cumulativeExperience = 32567101537; break;
                case 67: this.cumulativeExperience = 38068701511; break;
                case 68: this.cumulativeExperience = 44670621481; break;
                case 69: this.cumulativeExperience = 52592925445; break;
                case 70: this.cumulativeExperience = 62921685493; break;
                case 71: this.cumulativeExperience = 75316197550; break;
                case 72: this.cumulativeExperience = 90189612020; break;
                case 73: this.cumulativeExperience = 108037709383; break;
                case 74: this.cumulativeExperience = 129455426218; break;
                case 75: this.cumulativeExperience = 155156686421; break;
                case 76: this.cumulativeExperience = 185998198664; break;
                case 77: this.cumulativeExperience = 223008013356; break;
                case 78: this.cumulativeExperience = 267419790986; break;
                case 79: this.cumulativeExperience = 320713924142; break;
                case 80: this.cumulativeExperience = 384666883930; break;
                case 81: this.cumulativeExperience = 453736080501; break;
                case 82: this.cumulativeExperience = 528330812797; break;
                case 83: this.cumulativeExperience = 608893123677; break;
                case 84: this.cumulativeExperience = 695900419427; break;
                case 85: this.cumulativeExperience = 789868298837; break;
                case 86: this.cumulativeExperience = 889868298837; break;
                case 87: this.cumulativeExperience = 999868298837; break;
                case 88: this.cumulativeExperience = 1124868298837; break;
                case 89: this.cumulativeExperience = 1259868298837; break;
                case 90: this.cumulativeExperience = 1385242114520; break;
            }
            return this;
        }

    }
}
