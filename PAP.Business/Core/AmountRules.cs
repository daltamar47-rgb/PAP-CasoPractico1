namespace PAP.Business.Core
{
    /// <summary>
    /// Common numeric validations used by several managers.
    /// </summary>
    public static class AmountRules
    {
        public static void RequirePositive(decimal value, string fieldName)
        {
            if (value <= 0)
            {
                throw new BusinessException(string.Format("{0} debe ser mayor a 0.", fieldName));
            }
        }

        public static void RequirePositive(int value, string fieldName)
        {
            if (value <= 0)
            {
                throw new BusinessException(string.Format("{0} debe ser mayor a 0.", fieldName));
            }
        }

        public static void RequireNotNegative(decimal value, string fieldName)
        {
            if (value < 0)
            {
                throw new BusinessException(string.Format("{0} no puede ser negativo.", fieldName));
            }
        }

        public static void RequireNotNegative(int value, string fieldName)
        {
            if (value < 0)
            {
                throw new BusinessException(string.Format("{0} no puede ser negativo.", fieldName));
            }
        }
    }
}
