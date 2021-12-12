namespace Cultivation_Way.Utils
{
    class OthersHelper
    {
        public static int getBeOppsitedBy(int type)
        {
            switch (type)
            {
                case 0:
                    return 3;
                case 1:
                    return 1;
                case 2:
                    return 4;
                case 3:
                    return 2;
                default:
                    return 0;
            }
        }
        public static int getBePromotedBy(int type)
        {
            switch (type)
            {
                case 0:
                    return 4;
                case 1:
                    return 2;
                case 2:
                    return 0;
                case 3:
                    return 1;
                default:
                    return 3;
            }
        }
    }
}
