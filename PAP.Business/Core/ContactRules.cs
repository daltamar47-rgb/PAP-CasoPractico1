namespace PAP.Business.Core
{
    /// <summary>
    /// Validations and normalization for people data (email, password).
    /// </summary>
    public static class ContactRules
    {
        public static string NormalizeEmail(string email)
        {
            var normalized = (email ?? string.Empty).Trim().ToLowerInvariant();
            if (!normalized.Contains("@") || normalized.StartsWith("@") || normalized.EndsWith("@"))
            {
                throw new BusinessException("El correo electrónico no tiene un formato válido.");
            }

            return normalized;
        }

        /// <summary>
        /// Hashes the password typed in the form. An empty value on edit keeps the current password.
        /// </summary>
        public static string HashTypedPassword(string typedPassword, bool isNew)
        {
            if (string.IsNullOrWhiteSpace(typedPassword))
            {
                if (isNew)
                {
                    throw new BusinessException("La contraseña es obligatoria.");
                }

                return typedPassword;
            }

            return PasswordHasher.Hash(typedPassword);
        }
    }
}
