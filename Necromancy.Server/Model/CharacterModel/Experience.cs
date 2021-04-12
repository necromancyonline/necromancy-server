namespace Necromancy.Server.Model
{
    public class Experience
    {
        public Experience()
        {
            level = 0;
            cumulativeExperience = 0;
        }

        public uint level { get; set; }
        public ulong cumulativeExperience { get; set; }

        public Experience CalculateLevelUp(uint level)
        {
            switch (level)
            {
                case 1:
                    cumulativeExperience = 0;
                    break;
                case 2:
                    cumulativeExperience = 100;
                    break;
                case 3:
                    cumulativeExperience = 315;
                    break;
                case 4:
                    cumulativeExperience = 745;
                    break;
                case 5:
                    cumulativeExperience = 1605;
                    break;
                case 6:
                    cumulativeExperience = 3325;
                    break;
                case 7:
                    cumulativeExperience = 6765;
                    break;
                case 8:
                    cumulativeExperience = 13473;
                    break;
                case 9:
                    cumulativeExperience = 26218;
                    break;
                case 10:
                    cumulativeExperience = 49797;
                    break;
                case 11:
                    cumulativeExperience = 92238;
                    break;
                case 12:
                    cumulativeExperience = 166511;
                    break;
                case 13:
                    cumulativeExperience = 292774;
                    break;
                case 14:
                    cumulativeExperience = 501109;
                    break;
                case 15:
                    cumulativeExperience = 834445;
                    break;
                case 16:
                    cumulativeExperience = 1351115;
                    break;
                case 17:
                    cumulativeExperience = 2126121;
                    break;
                case 18:
                    cumulativeExperience = 3288629;
                    break;
                case 19:
                    cumulativeExperience = 5032391;
                    break;
                case 20:
                    cumulativeExperience = 7648034;
                    break;
                case 21:
                    cumulativeExperience = 11440717;
                    break;
                case 22:
                    cumulativeExperience = 16940107;
                    break;
                case 23:
                    cumulativeExperience = 24639253;
                    break;
                case 24:
                    cumulativeExperience = 35033100;
                    break;
                case 25:
                    cumulativeExperience = 49064794;
                    break;
                case 26:
                    cumulativeExperience = 67305996;
                    break;
                case 27:
                    cumulativeExperience = 91019558;
                    break;
                case 28:
                    cumulativeExperience = 119475833;
                    break;
                case 29:
                    cumulativeExperience = 153623363;
                    break;
                case 30:
                    cumulativeExperience = 193917448;
                    break;
                case 31:
                    cumulativeExperience = 241464469;
                    break;
                case 32:
                    cumulativeExperience = 296619013;
                    break;
                case 33:
                    cumulativeExperience = 360598284;
                    break;
                case 34:
                    cumulativeExperience = 434174445;
                    break;
                case 35:
                    cumulativeExperience = 518787031;
                    break;
                case 36:
                    cumulativeExperience = 615765379;
                    break;
                case 37:
                    cumulativeExperience = 725207895;
                    break;
                case 38:
                    cumulativeExperience = 849465539;
                    break;
                case 39:
                    cumulativeExperience = 989876676;
                    break;
                case 40:
                    cumulativeExperience = 1147137150;
                    break;
                case 41:
                    cumulativeExperience = 1323268881;
                    break;
                case 42:
                    cumulativeExperience = 1518775102;
                    break;
                case 43:
                    cumulativeExperience = 1735787007;
                    break;
                case 44:
                    cumulativeExperience = 1976670222;
                    break;
                case 45:
                    cumulativeExperience = 2244050590;
                    break;
                case 46:
                    cumulativeExperience = 2540842799;
                    break;
                case 47:
                    cumulativeExperience = 2870282151;
                    break;
                case 48:
                    cumulativeExperience = 3235959832;
                    break;
                case 49:
                    cumulativeExperience = 3641862058;
                    break;
                case 50:
                    cumulativeExperience = 4092413528;
                    break;
                case 51:
                    cumulativeExperience = 4597031175;
                    break;
                case 52:
                    cumulativeExperience = 5162202939;
                    break;
                case 53:
                    cumulativeExperience = 5800847033;
                    break;
                case 54:
                    cumulativeExperience = 6528901300;
                    break;
                case 55:
                    cumulativeExperience = 7358883165;
                    break;
                case 56:
                    cumulativeExperience = 8313362309;
                    break;
                case 57:
                    cumulativeExperience = 9411013325;
                    break;
                case 58:
                    cumulativeExperience = 10673311993;
                    break;
                case 59:
                    cumulativeExperience = 12137578448;
                    break;
                case 60:
                    cumulativeExperience = 13836127535;
                    break;
                case 61:
                    cumulativeExperience = 15823429968;
                    break;
                case 62:
                    cumulativeExperience = 18148573814;
                    break;
                case 63:
                    cumulativeExperience = 20892243553;
                    break;
                case 64:
                    cumulativeExperience = 24129773844;
                    break;
                case 65:
                    cumulativeExperience = 27982434891;
                    break;
                case 66:
                    cumulativeExperience = 32567101537;
                    break;
                case 67:
                    cumulativeExperience = 38068701511;
                    break;
                case 68:
                    cumulativeExperience = 44670621481;
                    break;
                case 69:
                    cumulativeExperience = 52592925445;
                    break;
                case 70:
                    cumulativeExperience = 62921685493;
                    break;
                case 71:
                    cumulativeExperience = 75316197550;
                    break;
                case 72:
                    cumulativeExperience = 90189612020;
                    break;
                case 73:
                    cumulativeExperience = 108037709383;
                    break;
                case 74:
                    cumulativeExperience = 129455426218;
                    break;
                case 75:
                    cumulativeExperience = 155156686421;
                    break;
                case 76:
                    cumulativeExperience = 185998198664;
                    break;
                case 77:
                    cumulativeExperience = 223008013356;
                    break;
                case 78:
                    cumulativeExperience = 267419790986;
                    break;
                case 79:
                    cumulativeExperience = 320713924142;
                    break;
                case 80:
                    cumulativeExperience = 384666883930;
                    break;
                case 81:
                    cumulativeExperience = 453736080501;
                    break;
                case 82:
                    cumulativeExperience = 528330812797;
                    break;
                case 83:
                    cumulativeExperience = 608893123677;
                    break;
                case 84:
                    cumulativeExperience = 695900419427;
                    break;
                case 85:
                    cumulativeExperience = 789868298837;
                    break;
                case 86:
                    cumulativeExperience = 889868298837;
                    break;
                case 87:
                    cumulativeExperience = 999868298837;
                    break;
                case 88:
                    cumulativeExperience = 1124868298837;
                    break;
                case 89:
                    cumulativeExperience = 1259868298837;
                    break;
                case 90:
                    cumulativeExperience = 1385242114520;
                    break;
            }

            return this;
        }
    }
}
