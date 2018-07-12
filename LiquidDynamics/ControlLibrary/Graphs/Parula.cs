using System.Drawing;
using System.Linq;

namespace ControlLibrary.Graphs
{
   internal static class Parula
   {
      private static readonly Color[] Colors;

      static Parula()
      {
         #region Parula colors

         var parula = new[]
         {
            new {r = 0.2081F, g = 0.1663F, b = 0.5292F},
            new {r = 0.2091F, g = 0.1721F, b = 0.5411F},
            new {r = 0.2101F, g = 0.1779F, b = 0.5530F},
            new {r = 0.2109F, g = 0.1837F, b = 0.5650F},
            new {r = 0.2116F, g = 0.1895F, b = 0.5771F},
            new {r = 0.2121F, g = 0.1954F, b = 0.5892F},
            new {r = 0.2124F, g = 0.2013F, b = 0.6013F},
            new {r = 0.2125F, g = 0.2072F, b = 0.6135F},
            new {r = 0.2123F, g = 0.2132F, b = 0.6258F},
            new {r = 0.2118F, g = 0.2192F, b = 0.6381F},
            new {r = 0.2111F, g = 0.2253F, b = 0.6505F},
            new {r = 0.2099F, g = 0.2315F, b = 0.6629F},
            new {r = 0.2084F, g = 0.2377F, b = 0.6753F},
            new {r = 0.2063F, g = 0.2440F, b = 0.6878F},
            new {r = 0.2038F, g = 0.2503F, b = 0.7003F},
            new {r = 0.2006F, g = 0.2568F, b = 0.7129F},
            new {r = 0.1968F, g = 0.2632F, b = 0.7255F},
            new {r = 0.1921F, g = 0.2698F, b = 0.7381F},
            new {r = 0.1867F, g = 0.2764F, b = 0.7507F},
            new {r = 0.1802F, g = 0.2832F, b = 0.7634F},
            new {r = 0.1728F, g = 0.2902F, b = 0.7762F},
            new {r = 0.1641F, g = 0.2975F, b = 0.7890F},
            new {r = 0.1541F, g = 0.3052F, b = 0.8017F},
            new {r = 0.1427F, g = 0.3132F, b = 0.8145F},
            new {r = 0.1295F, g = 0.3217F, b = 0.8269F},
            new {r = 0.1147F, g = 0.3306F, b = 0.8387F},
            new {r = 0.0986F, g = 0.3397F, b = 0.8495F},
            new {r = 0.0816F, g = 0.3486F, b = 0.8588F},
            new {r = 0.0646F, g = 0.3572F, b = 0.8664F},
            new {r = 0.0482F, g = 0.3651F, b = 0.8722F},
            new {r = 0.0329F, g = 0.3724F, b = 0.8765F},
            new {r = 0.0213F, g = 0.3792F, b = 0.8796F},
            new {r = 0.0136F, g = 0.3853F, b = 0.8815F},
            new {r = 0.0086F, g = 0.3911F, b = 0.8827F},
            new {r = 0.0060F, g = 0.3965F, b = 0.8833F},
            new {r = 0.0051F, g = 0.4017F, b = 0.8834F},
            new {r = 0.0054F, g = 0.4066F, b = 0.8831F},
            new {r = 0.0067F, g = 0.4113F, b = 0.8825F},
            new {r = 0.0089F, g = 0.4159F, b = 0.8816F},
            new {r = 0.0116F, g = 0.4203F, b = 0.8805F},
            new {r = 0.0148F, g = 0.4246F, b = 0.8793F},
            new {r = 0.0184F, g = 0.4288F, b = 0.8779F},
            new {r = 0.0223F, g = 0.4329F, b = 0.8763F},
            new {r = 0.0264F, g = 0.4370F, b = 0.8747F},
            new {r = 0.0306F, g = 0.4410F, b = 0.8729F},
            new {r = 0.0349F, g = 0.4449F, b = 0.8711F},
            new {r = 0.0394F, g = 0.4488F, b = 0.8692F},
            new {r = 0.0437F, g = 0.4526F, b = 0.8672F},
            new {r = 0.0477F, g = 0.4564F, b = 0.8652F},
            new {r = 0.0514F, g = 0.4602F, b = 0.8632F},
            new {r = 0.0549F, g = 0.4640F, b = 0.8611F},
            new {r = 0.0582F, g = 0.4677F, b = 0.8589F},
            new {r = 0.0612F, g = 0.4714F, b = 0.8568F},
            new {r = 0.0640F, g = 0.4751F, b = 0.8546F},
            new {r = 0.0666F, g = 0.4788F, b = 0.8525F},
            new {r = 0.0689F, g = 0.4825F, b = 0.8503F},
            new {r = 0.0710F, g = 0.4862F, b = 0.8481F},
            new {r = 0.0729F, g = 0.4899F, b = 0.8460F},
            new {r = 0.0746F, g = 0.4937F, b = 0.8439F},
            new {r = 0.0761F, g = 0.4974F, b = 0.8418F},
            new {r = 0.0773F, g = 0.5012F, b = 0.8398F},
            new {r = 0.0782F, g = 0.5051F, b = 0.8378F},
            new {r = 0.0789F, g = 0.5089F, b = 0.8359F},
            new {r = 0.0794F, g = 0.5129F, b = 0.8341F},
            new {r = 0.0795F, g = 0.5169F, b = 0.8324F},
            new {r = 0.0793F, g = 0.5210F, b = 0.8308F},
            new {r = 0.0788F, g = 0.5251F, b = 0.8293F},
            new {r = 0.0778F, g = 0.5295F, b = 0.8280F},
            new {r = 0.0764F, g = 0.5339F, b = 0.8270F},
            new {r = 0.0746F, g = 0.5384F, b = 0.8261F},
            new {r = 0.0724F, g = 0.5431F, b = 0.8253F},
            new {r = 0.0698F, g = 0.5479F, b = 0.8247F},
            new {r = 0.0668F, g = 0.5527F, b = 0.8243F},
            new {r = 0.0636F, g = 0.5577F, b = 0.8239F},
            new {r = 0.0600F, g = 0.5627F, b = 0.8237F},
            new {r = 0.0562F, g = 0.5677F, b = 0.8234F},
            new {r = 0.0523F, g = 0.5727F, b = 0.8231F},
            new {r = 0.0484F, g = 0.5777F, b = 0.8228F},
            new {r = 0.0445F, g = 0.5826F, b = 0.8223F},
            new {r = 0.0408F, g = 0.5874F, b = 0.8217F},
            new {r = 0.0372F, g = 0.5922F, b = 0.8209F},
            new {r = 0.0342F, g = 0.5968F, b = 0.8198F},
            new {r = 0.0317F, g = 0.6012F, b = 0.8186F},
            new {r = 0.0296F, g = 0.6055F, b = 0.8171F},
            new {r = 0.0279F, g = 0.6097F, b = 0.8154F},
            new {r = 0.0265F, g = 0.6137F, b = 0.8135F},
            new {r = 0.0255F, g = 0.6176F, b = 0.8114F},
            new {r = 0.0248F, g = 0.6214F, b = 0.8091F},
            new {r = 0.0243F, g = 0.6250F, b = 0.8066F},
            new {r = 0.0239F, g = 0.6285F, b = 0.8039F},
            new {r = 0.0237F, g = 0.6319F, b = 0.8010F},
            new {r = 0.0235F, g = 0.6352F, b = 0.7980F},
            new {r = 0.0233F, g = 0.6384F, b = 0.7948F},
            new {r = 0.0231F, g = 0.6415F, b = 0.7916F},
            new {r = 0.0230F, g = 0.6445F, b = 0.7881F},
            new {r = 0.0229F, g = 0.6474F, b = 0.7846F},
            new {r = 0.0227F, g = 0.6503F, b = 0.7810F},
            new {r = 0.0227F, g = 0.6531F, b = 0.7773F},
            new {r = 0.0232F, g = 0.6558F, b = 0.7735F},
            new {r = 0.0238F, g = 0.6585F, b = 0.7696F},
            new {r = 0.0246F, g = 0.6611F, b = 0.7656F},
            new {r = 0.0263F, g = 0.6637F, b = 0.7615F},
            new {r = 0.0282F, g = 0.6663F, b = 0.7574F},
            new {r = 0.0306F, g = 0.6688F, b = 0.7532F},
            new {r = 0.0338F, g = 0.6712F, b = 0.7490F},
            new {r = 0.0373F, g = 0.6737F, b = 0.7446F},
            new {r = 0.0418F, g = 0.6761F, b = 0.7402F},
            new {r = 0.0467F, g = 0.6784F, b = 0.7358F},
            new {r = 0.0516F, g = 0.6808F, b = 0.7313F},
            new {r = 0.0574F, g = 0.6831F, b = 0.7267F},
            new {r = 0.0629F, g = 0.6854F, b = 0.7221F},
            new {r = 0.0692F, g = 0.6877F, b = 0.7173F},
            new {r = 0.0755F, g = 0.6899F, b = 0.7126F},
            new {r = 0.0820F, g = 0.6921F, b = 0.7078F},
            new {r = 0.0889F, g = 0.6943F, b = 0.7029F},
            new {r = 0.0956F, g = 0.6965F, b = 0.6979F},
            new {r = 0.1031F, g = 0.6986F, b = 0.6929F},
            new {r = 0.1104F, g = 0.7007F, b = 0.6878F},
            new {r = 0.1180F, g = 0.7028F, b = 0.6827F},
            new {r = 0.1258F, g = 0.7049F, b = 0.6775F},
            new {r = 0.1335F, g = 0.7069F, b = 0.6723F},
            new {r = 0.1418F, g = 0.7089F, b = 0.6669F},
            new {r = 0.1499F, g = 0.7109F, b = 0.6616F},
            new {r = 0.1585F, g = 0.7129F, b = 0.6561F},
            new {r = 0.1671F, g = 0.7148F, b = 0.6507F},
            new {r = 0.1758F, g = 0.7168F, b = 0.6451F},
            new {r = 0.1849F, g = 0.7186F, b = 0.6395F},
            new {r = 0.1938F, g = 0.7205F, b = 0.6338F},
            new {r = 0.2033F, g = 0.7223F, b = 0.6281F},
            new {r = 0.2128F, g = 0.7241F, b = 0.6223F},
            new {r = 0.2224F, g = 0.7259F, b = 0.6165F},
            new {r = 0.2324F, g = 0.7275F, b = 0.6107F},
            new {r = 0.2423F, g = 0.7292F, b = 0.6048F},
            new {r = 0.2527F, g = 0.7308F, b = 0.5988F},
            new {r = 0.2631F, g = 0.7324F, b = 0.5929F},
            new {r = 0.2735F, g = 0.7339F, b = 0.5869F},
            new {r = 0.2845F, g = 0.7354F, b = 0.5809F},
            new {r = 0.2953F, g = 0.7368F, b = 0.5749F},
            new {r = 0.3064F, g = 0.7381F, b = 0.5689F},
            new {r = 0.3177F, g = 0.7394F, b = 0.5630F},
            new {r = 0.3289F, g = 0.7406F, b = 0.5570F},
            new {r = 0.3405F, g = 0.7417F, b = 0.5512F},
            new {r = 0.3520F, g = 0.7428F, b = 0.5453F},
            new {r = 0.3635F, g = 0.7438F, b = 0.5396F},
            new {r = 0.3753F, g = 0.7446F, b = 0.5339F},
            new {r = 0.3869F, g = 0.7454F, b = 0.5283F},
            new {r = 0.3986F, g = 0.7461F, b = 0.5229F},
            new {r = 0.4103F, g = 0.7467F, b = 0.5175F},
            new {r = 0.4218F, g = 0.7473F, b = 0.5123F},
            new {r = 0.4334F, g = 0.7477F, b = 0.5072F},
            new {r = 0.4447F, g = 0.7482F, b = 0.5021F},
            new {r = 0.4561F, g = 0.7485F, b = 0.4972F},
            new {r = 0.4672F, g = 0.7487F, b = 0.4924F},
            new {r = 0.4783F, g = 0.7489F, b = 0.4877F},
            new {r = 0.4892F, g = 0.7491F, b = 0.4831F},
            new {r = 0.5000F, g = 0.7491F, b = 0.4786F},
            new {r = 0.5106F, g = 0.7492F, b = 0.4741F},
            new {r = 0.5212F, g = 0.7492F, b = 0.4698F},
            new {r = 0.5315F, g = 0.7491F, b = 0.4655F},
            new {r = 0.5418F, g = 0.7490F, b = 0.4613F},
            new {r = 0.5519F, g = 0.7489F, b = 0.4571F},
            new {r = 0.5619F, g = 0.7487F, b = 0.4531F},
            new {r = 0.5718F, g = 0.7485F, b = 0.4490F},
            new {r = 0.5816F, g = 0.7482F, b = 0.4451F},
            new {r = 0.5913F, g = 0.7479F, b = 0.4412F},
            new {r = 0.6009F, g = 0.7476F, b = 0.4374F},
            new {r = 0.6103F, g = 0.7473F, b = 0.4335F},
            new {r = 0.6197F, g = 0.7469F, b = 0.4298F},
            new {r = 0.6290F, g = 0.7465F, b = 0.4261F},
            new {r = 0.6382F, g = 0.7460F, b = 0.4224F},
            new {r = 0.6473F, g = 0.7456F, b = 0.4188F},
            new {r = 0.6564F, g = 0.7451F, b = 0.4152F},
            new {r = 0.6653F, g = 0.7446F, b = 0.4116F},
            new {r = 0.6742F, g = 0.7441F, b = 0.4081F},
            new {r = 0.6830F, g = 0.7435F, b = 0.4046F},
            new {r = 0.6918F, g = 0.7430F, b = 0.4011F},
            new {r = 0.7004F, g = 0.7424F, b = 0.3976F},
            new {r = 0.7091F, g = 0.7418F, b = 0.3942F},
            new {r = 0.7176F, g = 0.7412F, b = 0.3908F},
            new {r = 0.7261F, g = 0.7405F, b = 0.3874F},
            new {r = 0.7346F, g = 0.7399F, b = 0.3840F},
            new {r = 0.7430F, g = 0.7392F, b = 0.3806F},
            new {r = 0.7513F, g = 0.7385F, b = 0.3773F},
            new {r = 0.7596F, g = 0.7378F, b = 0.3739F},
            new {r = 0.7679F, g = 0.7372F, b = 0.3706F},
            new {r = 0.7761F, g = 0.7364F, b = 0.3673F},
            new {r = 0.7843F, g = 0.7357F, b = 0.3639F},
            new {r = 0.7924F, g = 0.7350F, b = 0.3606F},
            new {r = 0.8005F, g = 0.7343F, b = 0.3573F},
            new {r = 0.8085F, g = 0.7336F, b = 0.3539F},
            new {r = 0.8166F, g = 0.7329F, b = 0.3506F},
            new {r = 0.8246F, g = 0.7322F, b = 0.3472F},
            new {r = 0.8325F, g = 0.7315F, b = 0.3438F},
            new {r = 0.8405F, g = 0.7308F, b = 0.3404F},
            new {r = 0.8484F, g = 0.7301F, b = 0.3370F},
            new {r = 0.8563F, g = 0.7294F, b = 0.3336F},
            new {r = 0.8642F, g = 0.7288F, b = 0.3300F},
            new {r = 0.8720F, g = 0.7282F, b = 0.3265F},
            new {r = 0.8798F, g = 0.7276F, b = 0.3229F},
            new {r = 0.8877F, g = 0.7271F, b = 0.3193F},
            new {r = 0.8954F, g = 0.7266F, b = 0.3156F},
            new {r = 0.9032F, g = 0.7262F, b = 0.3117F},
            new {r = 0.9110F, g = 0.7259F, b = 0.3078F},
            new {r = 0.9187F, g = 0.7256F, b = 0.3038F},
            new {r = 0.9264F, g = 0.7256F, b = 0.2996F},
            new {r = 0.9341F, g = 0.7256F, b = 0.2953F},
            new {r = 0.9417F, g = 0.7259F, b = 0.2907F},
            new {r = 0.9493F, g = 0.7264F, b = 0.2859F},
            new {r = 0.9567F, g = 0.7273F, b = 0.2808F},
            new {r = 0.9639F, g = 0.7285F, b = 0.2754F},
            new {r = 0.9708F, g = 0.7303F, b = 0.2696F},
            new {r = 0.9773F, g = 0.7326F, b = 0.2634F},
            new {r = 0.9831F, g = 0.7355F, b = 0.2570F},
            new {r = 0.9882F, g = 0.7390F, b = 0.2504F},
            new {r = 0.9922F, g = 0.7431F, b = 0.2437F},
            new {r = 0.9952F, g = 0.7476F, b = 0.2373F},
            new {r = 0.9973F, g = 0.7524F, b = 0.2310F},
            new {r = 0.9986F, g = 0.7573F, b = 0.2251F},
            new {r = 0.9991F, g = 0.7624F, b = 0.2195F},
            new {r = 0.9990F, g = 0.7675F, b = 0.2141F},
            new {r = 0.9985F, g = 0.7726F, b = 0.2090F},
            new {r = 0.9976F, g = 0.7778F, b = 0.2042F},
            new {r = 0.9964F, g = 0.7829F, b = 0.1995F},
            new {r = 0.9950F, g = 0.7880F, b = 0.1949F},
            new {r = 0.9933F, g = 0.7931F, b = 0.1905F},
            new {r = 0.9914F, g = 0.7981F, b = 0.1863F},
            new {r = 0.9894F, g = 0.8032F, b = 0.1821F},
            new {r = 0.9873F, g = 0.8083F, b = 0.1780F},
            new {r = 0.9851F, g = 0.8133F, b = 0.1740F},
            new {r = 0.9828F, g = 0.8184F, b = 0.1700F},
            new {r = 0.9805F, g = 0.8235F, b = 0.1661F},
            new {r = 0.9782F, g = 0.8286F, b = 0.1622F},
            new {r = 0.9759F, g = 0.8337F, b = 0.1583F},
            new {r = 0.9736F, g = 0.8389F, b = 0.1544F},
            new {r = 0.9713F, g = 0.8441F, b = 0.1505F},
            new {r = 0.9692F, g = 0.8494F, b = 0.1465F},
            new {r = 0.9672F, g = 0.8548F, b = 0.1425F},
            new {r = 0.9654F, g = 0.8603F, b = 0.1385F},
            new {r = 0.9638F, g = 0.8659F, b = 0.1343F},
            new {r = 0.9623F, g = 0.8716F, b = 0.1301F},
            new {r = 0.9611F, g = 0.8774F, b = 0.1258F},
            new {r = 0.9600F, g = 0.8834F, b = 0.1215F},
            new {r = 0.9593F, g = 0.8895F, b = 0.1171F},
            new {r = 0.9588F, g = 0.8958F, b = 0.1126F},
            new {r = 0.9586F, g = 0.9022F, b = 0.1082F},
            new {r = 0.9587F, g = 0.9088F, b = 0.1036F},
            new {r = 0.9591F, g = 0.9155F, b = 0.0990F},
            new {r = 0.9599F, g = 0.9225F, b = 0.0944F},
            new {r = 0.9610F, g = 0.9296F, b = 0.0897F},
            new {r = 0.9624F, g = 0.9368F, b = 0.0850F},
            new {r = 0.9641F, g = 0.9443F, b = 0.0802F},
            new {r = 0.9662F, g = 0.9518F, b = 0.0753F},
            new {r = 0.9685F, g = 0.9595F, b = 0.0703F},
            new {r = 0.9710F, g = 0.9673F, b = 0.0651F},
            new {r = 0.9736F, g = 0.9752F, b = 0.0597F},
            new {r = 0.9763F, g = 0.9831F, b = 0.0538F}
         };

         #endregion
         
         Colors = parula
            .Select(c => Color.FromArgb((int) (c.r * 255), (int) (c.g * 255), (int) (c.b * 255)))
            .ToArray();
      }

      public static Color GetColor(float value, float min, float max)
      {
         int idx = (int) ((Colors.Length - 1) * (value - min) / (max - min));
         return Colors[idx];
      }
   }
}